using System.Globalization;
using WebApi_CSV.Models;

namespace WebApi_CSV.Services
{
    public class ModelBuilderService
    {
        /// <summary>
        /// Получает из пришедшего файла данные: имя, автора, дата создания.
        /// </summary>
        /// <param name="file">Файл вида .csv.</param>
        /// <param name="author">Автор файла.</param>
        /// <param name="creationDateTime">Дата создания файла.</param>        
        /// <returns>Объект FileModel.</returns>
        public async Task<FileModel> GetFileModelFromData(IFormFile file, string? author, DateTime? creationDateTime)
        {
            FileModel fileModel = new FileModel
            {
                FileName = file.FileName,
                Author = author,
                CreationDateTime = creationDateTime
            };
            return fileModel;
        }
        /// <summary>
        /// Разделяет поля в строке, используя символ-разделитель delimiter, и присваивает значения полям ValueModel.
        /// </summary>
        /// <param name="csvLine">Строка с данными, соответвующая структуре ValueModel.</param>
        /// <param name="delimiter">Разделитель полей в строке.</param>
        /// <returns>Объект ValueModel с данными из строки.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ValueModel> ConvertCsvLine(string csvLine, string delimiter)
        {
            string[] parts = csvLine.Split(delimiter);
            ValueModel model = new ValueModel();
            List<string> errors = new List<string>();
            try
            {
                if (parts.Length != 3)
                {
                    throw new ArgumentException("Неверное количество полей в строке CSV");
                }
                // Первое поле - дата и время в формате строки
                if (!DateTime.TryParseExact(parts[0], "yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDateTime))
                {
                    errors.Add("Неверный формат даты и времени");
                }
                model.StartDateTime = startDateTime;

                // Второе поле - время работы (целое число)
                if (!int.TryParse(parts[1], out int workTime))
                {
                    errors.Add("Неверный формат времени работы");
                }
                model.WorkTime = workTime;

                //Третье поле - значение показателя (дробное число)
                if (!double.TryParse(parts[2], out double indicator))
                {
                    errors.Add("Неверный формат значения показателя");
                }
                model.Indicator = indicator;

            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка при конвертации строки CSV: {ex.Message}");
                model.Errors = errors;
            }
            return model;
        }
        /// <summary>
        /// Вычислияет результаты из значений экспериментов коллекции List<ValueModel>.
        /// </summary>
        /// <param name="valueModels">Коллекция значений экспериментов List<ValueModel> из .csv файла.</param>
        /// <returns>Объект ResultModel, содержащий данные вычислений.</returns>
        public Task<ResultModel> CalculationResult(List<ValueModel> valueModels)
        {
            string fileName = valueModels[0].FileName;
            TimeSpan firstTime = valueModels.Select(x => x.StartDateTime.TimeOfDay).Min();
            TimeSpan lastTime = valueModels.Select(x => x.StartDateTime.TimeOfDay).Max();
            int maxTimeWork = valueModels.Select(x => x.WorkTime).Max();
            int minTimeWork = valueModels.Select(x => x.WorkTime).Min();
            double avgTimeWork = valueModels.Select(x => x.WorkTime).Average();
            double avgIndicator = valueModels.Select(x => x.Indicator).Average();

            var orderedIndicators = valueModels.Select(x => x.Indicator).OrderBy(x => x);
            int countIndicators = orderedIndicators.Count();
            double medianIndicators;
            if (countIndicators % 2 == 0)
            {
                // Если количество элементов четное
                medianIndicators = (orderedIndicators.ElementAt(countIndicators / 2 - 1) + orderedIndicators.ElementAt(countIndicators / 2)) / 2.0;
            }
            else
            {
                // Если количество элементов нечетное
                medianIndicators = orderedIndicators.ElementAt(countIndicators / 2);
            }

            double maxIndicator = valueModels.Select(x => x.Indicator).Max();
            double minIndicator = valueModels.Select(x => x.Indicator).Min();
            int countOfExperiments = valueModels.Count();

            ResultModel resultModel = new ResultModel
            {
                FileName = fileName,
                FirstTime = firstTime,
                LastTime = lastTime,
                MaxTimeWork = maxTimeWork,
                MinTimeWork = maxTimeWork,
                AverageTimeWork = avgTimeWork,
                AverageIndicator = avgIndicator,
                MedianByIndicator = medianIndicators,
                MaxIndicator = maxIndicator,
                MinIndicator = minIndicator,
                CountOfExperiments = countOfExperiments

            };
            return Task.FromResult(resultModel);
        }
    }
}
