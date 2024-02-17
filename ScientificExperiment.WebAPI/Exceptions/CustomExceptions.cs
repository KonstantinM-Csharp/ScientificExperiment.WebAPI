namespace ScientificExperiment.WebAPI.Exceptions
{
    public class CustomExceptions
    {
        public class NotFoundException : Exception
        {
            public string? Model { get; set; }
            public override string Message => $"{Model} не найден!";
        }

        public class CSVException : Exception
        {
            public string? Model { get; set; }
            public override string Message => $"Необходим файл с разрешением .csv";
        }

        public class ValidationException : Exception
        {
            public string? Model { get; set; }
            public override string Message => $"Файл не прошел валидацию!Ошибка связана с {Model}!";
        }

        public class SizeValidationException : Exception
        {
            public string? Model { get; set; }
            public override string Message => $"Файл не прошел валидацию!Ошибка связана с {Model}!";

        }

        public class RowSizeValidationException : SizeValidationException
        {
            public RowSizeValidationException()
            {
                Model = "неккоректным количеством строк(Количество строк не может быть меньше 1 и больше 10 000)!";
            }
        }

        public class ColSizeValidationException : SizeValidationException
        {
            public ColSizeValidationException()
            {
                Model = "неккоректным количеством столбцов(Количество столбцов = 3)!";
            }
        }

        public class DataTypeValidationException : ValidationException
        {
            public DataTypeValidationException()
            {
                Model = "использованием неверных типов данных";
            }
        }

        public class ValueValidationException : ValidationException
        {
            public ValueValidationException()
            {
                Model = "некорректно введеным значением параметра(выход за границы)";
            }
        }

        public class testNotFoundException : NotFoundException
        {
            public testNotFoundException()
            {
                Model = "test";
            }
        }

    }
}
