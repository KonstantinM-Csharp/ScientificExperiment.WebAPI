using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using ScientificExperiment.WebAPI.Models;
using static ScientificExperiment.WebAPI.Exceptions.CustomExceptions;

namespace ScientificExperiment.WebAPI.Services
{
    public class GetDataFromDbService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;
        public GetDataFromDbService(DataContext context, IMapper mapper, FileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }
        /// <summary>
        /// Получает записи из Results по заданным фильтрам.
        /// </summary>
        /// <param name="filter">Модель фильтра для поиска результатов.</param>
        /// <returns>Лист записей Results</returns>
        public async Task<IEnumerable<ResultModel>> GetResults(FilterModel filter)
        {
            IQueryable<Result> dbResult = _context.Results.AsNoTracking();

            if (!string.IsNullOrEmpty(filter.FileName))
            {
              int? filterFileId = await _fileService.GetFileId(filter.FileName);
              dbResult = dbResult.Where(x => x.FileId == filterFileId);
            }
            if (filter.AverageIndicator_From >= 0)
                dbResult = dbResult.Where(x => x.AverageIndicator >= filter.AverageIndicator_From);

            if (filter.AverageIndicator_To >= 0)
                dbResult = dbResult.Where(x => x.AverageIndicator <= filter.AverageIndicator_To);

            if (filter.AverageTimeWork_From >= 0)
                dbResult = dbResult.Where(x => x.AverageTimeWork >= filter.AverageTimeWork_From);

            if (filter.AverageTimeWork_To >= 0)
                dbResult = dbResult.Where(x => x.AverageTimeWork <= filter.AverageTimeWork_To);


            var results = dbResult
                .AsNoTracking()
                .OrderByDescending(x => x.MinTimeWork);

            List<ResultModel> resultModels = new List<ResultModel>();
            //устанавливается имя файла модели
            foreach(var result in results)
            {
               string fileName = await _fileService.GetFileName(result.FileId);
                var resultModel = _mapper.Map<Result, ResultModel>(result);
                resultModel.FileName = fileName;
                resultModels.Add(resultModel);
            }
            return resultModels;
        }
        /// <summary>
        /// Получает записи из таблицы Values по имени файла, в котором эти данные расположены.
        /// </summary>
        /// <param name="fileName">Имя файла, значения экспериментов которого необходимо найти в базе.</param>
        /// <returns>Лист записей Values.</returns>
        public async Task<IEnumerable<ValueModel>> GetValues(string fileName)
        {

            int? fileId = await _fileService.GetFileId(fileName);
            if (fileId != null)
            { 
                var values = await _context.Values.AsNoTracking()
                        .Where(x => x.FileId == fileId)
                        .OrderByDescending(x => x.StartDateTime)
                        .Select(x => _mapper.Map<Value, ValueModel>(x))
                        .ToListAsync();
            foreach (var value in values)
            {
                value.FileName = fileName;
            }
            return values;
            }   
            else 
            { 
                throw new NotFoundException() { Model = fileName};
            }
        }
    }
}
