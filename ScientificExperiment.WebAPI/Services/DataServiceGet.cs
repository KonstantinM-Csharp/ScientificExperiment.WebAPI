using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi_CSV.Models;

namespace WebApi_CSV.Services
{
    public class DataServiceGet
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;
        public DataServiceGet(DataContext context, IMapper mapper, FileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }
        /// <summary>
        /// Получает записи из Results по заданным фильтрам.
        /// </summary>
        /// <param name="filter"></param>
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

            return await dbResult
                .AsNoTracking()
                .OrderByDescending(x => x.MinTimeWork)
                .Select(x => _mapper.Map<ResultModel>(x))
                .ToListAsync();
            
        }
        /// <summary>
        /// Получает записи из таблицы Values по имени файла, в котором эти данные расположены.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Лист записей Values.</returns>
        public async Task<IEnumerable<ValueModel>> GetValues(string fileName)
        {
            int? fileId = await _fileService.GetFileId(fileName);
            return await _context.Values.AsNoTracking()
                    .Where(x => x.FileId == fileId)
                    .OrderByDescending(x => x.StartDateTime)
                    .Select(x => _mapper.Map<ValueModel>(x))
                    .ToListAsync();
        }
    }
}
