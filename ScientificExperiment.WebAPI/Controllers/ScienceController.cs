using Microsoft.AspNetCore.Mvc;
using ScientificExperiment.WebAPI.Models;
using ScientificExperiment.WebAPI.Services;
using static ScientificExperiment.WebAPI.Exceptions.CustomExceptions;

namespace ScientificExperiment.WebAPI.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScienceController : ControllerBase
    {

        private readonly ProcessorValues _processorValuesService;
        private readonly GetDataFromDbService _dataServiceGet;

        public ScienceController(ProcessorValues processorValuesService, GetDataFromDbService dataServiceGet)
        {
            _processorValuesService = processorValuesService;
            _dataServiceGet = dataServiceGet;
        }

        /// <summary>
        /// Принимает файл вида *.csv. В данном файле на каждой новой строке представлено значение следующего вида: {Дата и время в формате ГГГГ-ММ-ДД_чч-мм-сс};{Целочисленное значение времени в секундах};{ Показатель в виде числа с плавающей запятой}
        /// </summary>
        /// <param name="file">Файл вида *.csv.</param>
        /// <param name="author">Автор файла.</param>
        /// <param name="creationDateTime">Дата и время создания файла.</param>
        /// <returns></returns>
        /// <exception cref="CSVException"></exception>        
        [HttpPost("/science/files")]
        public async Task<IActionResult> UploadFile(IFormFile file, string? author, DateTime? creationDateTime)
        {
            //Проверка файла на пустоту.
            if (file.Length == 0)
            {
                return BadRequest("Файл не должен быть пустым.");
            }
            //Проверка типа файла.
            if (file.ContentType != "text/csv")
            {
                throw new CSVException();
            }
            else
            {
                 await _processorValuesService.DataProcessingAsync(file, ";", author, creationDateTime);
                return Ok("Данные прошедшие валидацию добавлены. Значения не прошедшие валидацию были отброшены.");
            }
        }

        /// <summary>
        /// Возвращает вычисленные (обработанные) данные о научных экспериментах из таблицы Result базы данных c применением фильтров filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Коллекция объектов ResultModel.</returns>
        [HttpGet("/science/results")]
        public async Task<IEnumerable<ResultModel>> GetResults([FromQuery] FilterModel filter)
             => await _dataServiceGet.GetResults(filter);

        /// <summary>
        /// Получает записи из таблицы Values по имени файла fileName, в котором эти данные расположены.
        /// </summary>
        /// <param name="fileName">Имя файла, содержащего в себе данные об экспериментах.</param>
        /// <returns>Коллекция объектов ValueModel.</returns>
        [HttpGet("/science/values")]
        public async Task<IEnumerable<ValueModel>> GetValues(string fileName)
             => await _dataServiceGet.GetValues(fileName);
    }
}
