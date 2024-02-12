
using System.Globalization;
using WebApi_CSV.Models;
using Microsoft.AspNetCore.Http;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DAL;
using System.Collections.Generic;
using WebApi_CSV.Services.Validation;
using FluentValidation;

namespace WebApi_CSV.Services
{
    public class ProcessorValues
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;
        private readonly ModelBuilderService _modelBuilderService;
        private readonly int allowedNumberOfRecords = 10000;
        public ProcessorValues(DataContext context, IMapper mapper, FileService fileService, ModelBuilderService modelBuilderService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
            _modelBuilderService = modelBuilderService;
        }
        
        public async Task DataProcessingAsync(IFormFile formFile, string delimiter, string? author, DateTime? creationDateTime)
        {

            FileModel fileModel = await _modelBuilderService.GetFileModelFromData(formFile, author, creationDateTime);
            var correctAndIncorrectModels = await ParseDataToValueModelAsync(formFile, delimiter);
            List<ValueModel> values = correctAndIncorrectModels.values;
            List<ValueModel> incorrectValues = correctAndIncorrectModels.incorrectValues;
            if (values != null)
            {
                ResultModel resultModel = await _modelBuilderService.CalculationResult(values);
                await SavingDb(fileModel, values, resultModel);
            }
        }
        /// <summary>
        /// Считывает данные из файла построчно и записывает данные в объект ValueModel.
        /// </summary>
        /// <param name="formFile">Файл с данными.</param>
        /// <param name="delimiter">Разделитель полей в строке.</param>
        /// <returns>Коллекция ValueModel, прошедших валидациюю.</returns>
        /// <returns>Коллекция ValueModel, непрошедших валидациюю.</returns>
        private async Task<(List<ValueModel> values, List<ValueModel> incorrectValues)> ParseDataToValueModelAsync(IFormFile formFile, string delimiter)
        {
            List<ValueModel> values = new List<ValueModel>();
            List<ValueModel> incorrectValues = new List<ValueModel>();

            using (var streamReader = new StreamReader(formFile.OpenReadStream()))
            {
                string line;
                int i = 0;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    if(i==allowedNumberOfRecords)
                    { 
                        return (values, incorrectValues);
                    }
                    ValueModel valueModel = await _modelBuilderService.ConvertCsvLine(line, delimiter);
                    valueModel.FileName = formFile.FileName;
                    var validator = new ValueModelValidator();
                    var resultValidation = validator.Validate(valueModel);
                    if (!resultValidation.IsValid||valueModel.Errors?.Count > 0)
                    {
                        valueModel.Errors?.Add(resultValidation.Errors.ToString());
                        incorrectValues.Add(valueModel);
                    }
                    else values.Add(valueModel);
                    ++i;
                }
            }
            return (values, incorrectValues);
        }
       
        /// <summary>
        /// Сохраняет модели в базу данных. Если файл с таким именем существует, перезаписывает данные о файле, Value и Result.
        /// </summary>
        /// <param name="fileModel">Объект FileModel, содержащий информацию о файле.</param>
        /// <param name="valueModels">Коллекция объектов ValueModel, содержащий данные об экспериментах.</param>
        /// <param name="resultModel">Объект Result, содержащий вычисленные данные экспериментов.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task SavingDb(FileModel fileModel, List<ValueModel> valueModels, ResultModel resultModel)
        {

            var file = await _context.Files.FirstOrDefaultAsync(x=>x.FileName==fileModel.FileName);
            if (file != null)
            {
                file.CreationDateTime = fileModel.CreationDateTime;
                file.Author = fileModel.Author;
                
                var oldResult = await _context.Results
                                        .Where(x => x.FileId == file.Id)
                                        .ToListAsync();
                var oldValues = await _context.Values
                                        .Where(x => x.FileId == file.Id)
                                         .ToListAsync();
                if (oldResult.Count != 0)
                    _context.Results.RemoveRange(oldResult);

                if (oldValues.Count != 0 )
                    _context.Values.RemoveRange(oldValues);
            }
            else
            {
                var newFile = _mapper.Map<FileModel, DAL.Entities.File>(fileModel);
                await _context.Files.AddAsync(newFile);
                await _context.SaveChangesAsync();
            }

            // Map and save resultModel
            var result = _mapper.Map<ResultModel, Result>(resultModel);
            result.FileId = file?.Id ?? throw new InvalidOperationException("File not found.");
            await _context.Results.AddAsync(result);

            // Map and save valueModels
            var values = _mapper.Map<List<ValueModel>, List<Value>>(valueModels);
            foreach (var value in values)
            {
                value.FileId = file?.Id ?? throw new InvalidOperationException("File not found.");
            }
            await _context.Values.AddRangeAsync(values);

            await _context.SaveChangesAsync();
        }
    }
}
