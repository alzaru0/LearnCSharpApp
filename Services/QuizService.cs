using LearnCSharpApp.Models;

namespace LearnCSharpApp.Services;

public class QuizService : IQuizService
{
    public Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync()
    {
        var questions = new List<QuizQuestion>
        {
            Create(1, 1, "Переменные", "Какой тип данных хранит целые числа?", ["int", "string", "bool", "double"], 0),
            Create(2, 1, "Переменные", "Как объявить строковую переменную?", ["int name = 5;", "string name = \"Aliya\";", "bool name = true;", "double name = 1.5;"], 1),
            Create(3, 1, "Переменные", "Какое ключевое слово используется для константы?", ["let", "static", "const", "fixed"], 2),
            Create(4, 1, "Переменные", "Какой тип хранит true или false?", ["char", "bool", "decimal", "string"], 1),
            Create(5, 1, "Переменные", "Что обязательно есть у переменной C#?", ["Тип", "Иконка", "Папка", "Цвет"], 0),

            Create(6, 2, "Условия", "Какой оператор проверяет условие?", ["for", "if", "class", "using"], 1),
            Create(7, 2, "Условия", "Что возвращает условное выражение?", ["string", "int", "bool", "array"], 2),
            Create(8, 2, "Условия", "Какой блок выполняется, если if ложен?", ["else", "main", "return", "new"], 0),
            Create(9, 2, "Условия", "Что удобно использовать для выбора из многих вариантов?", ["switch", "var", "namespace", "foreach"], 0),
            Create(10, 2, "Условия", "Как записать сравнение на равенство?", ["=", "==", "!=", ">="], 1),

            Create(11, 3, "Циклы", "Какой цикл удобен при известном числе повторений?", ["for", "if", "switch", "class"], 0),
            Create(12, 3, "Циклы", "Какой цикл перебирает коллекцию?", ["do", "foreach", "switch", "else"], 1),
            Create(13, 3, "Циклы", "Что может вызвать бесконечный цикл?", ["Ложное условие", "Отсутствие изменения условия", "Метод Main", "Массив"], 1),
            Create(14, 3, "Циклы", "Какое свойство массива часто используют в цикле?", ["CountText", "Length", "Index", "SizeOf"], 1),
            Create(15, 3, "Циклы", "Какой цикл сначала выполняет тело, а потом проверяет условие?", ["do while", "foreach", "if", "switch"], 0),

            Create(16, 4, "Методы", "Для чего нужны методы?", ["Для повторного использования кода", "Для удаления проекта", "Для изменения темы IDE", "Для создания папок"], 0),
            Create(17, 4, "Методы", "Что может возвращать метод?", ["Только текст", "Только число", "Значение указанного типа", "Только true"], 2),
            Create(18, 4, "Методы", "Какое ключевое слово возвращает результат?", ["break", "return", "new", "void"], 1),
            Create(19, 4, "Методы", "Что означает void?", ["Метод ничего не возвращает", "Метод приватный", "Метод содержит массив", "Метод является классом"], 0),
            Create(20, 4, "Методы", "Что передается методу для работы?", ["Параметры", "Папки", "Шрифты", "Иконки"], 0),

            Create(21, 5, "Массивы", "С какого индекса начинается массив?", ["0", "1", "-1", "10"], 0),
            Create(22, 5, "Массивы", "Что хранит массив?", ["Значения одного типа", "Только классы", "Только строки", "Только методы"], 0),
            Create(23, 5, "Массивы", "Как получить длину массива?", ["array.Size", "array.Length", "array.Count()", "array.Index"], 1),
            Create(24, 5, "Массивы", "Как объявить массив целых чисел?", ["int[] numbers", "int numbers[]()", "array int numbers", "numbers int[]"], 0),
            Create(25, 5, "Массивы", "Какой цикл часто используют для перебора массива?", ["foreach", "switch", "namespace", "using"], 0),

            Create(26, 6, "Основы ООП", "Что описывает класс?", ["Структуру объекта", "Только цвет интерфейса", "Только базу данных", "Только комментарий"], 0),
            Create(27, 6, "Основы ООП", "Что такое объект?", ["Экземпляр класса", "Название папки", "Оператор условия", "Тип цикла"], 0),
            Create(28, 6, "Основы ООП", "Что относится к принципам ООП?", ["Инкапсуляция", "Компиляция XAML", "Скачивание пакета", "Запуск эмулятора"], 0),
            Create(29, 6, "Основы ООП", "Что обычно хранит состояние объекта?", ["Свойства", "Комментарии", "Папки", "Маршруты Shell"], 0),
            Create(30, 6, "Основы ООП", "Что описывает действие объекта?", ["Метод", "Цвет", "Индекс", "Файл проекта"], 0)
        };

        return Task.FromResult<IReadOnlyList<QuizQuestion>>(questions);
    }

    private static QuizQuestion Create(
        int id,
        int lessonId,
        string lessonTitle,
        string questionText,
        string[] options,
        int correctAnswerIndex)
    {
        var question = new QuizQuestion
        {
            Id = id,
            LessonId = lessonId,
            LessonTitle = lessonTitle,
            QuestionText = questionText,
            CorrectAnswerIndex = correctAnswerIndex
        };

        for (var index = 0; index < options.Length; index++)
        {
            question.Options.Add(new QuizAnswerOption
            {
                QuestionId = id,
                Index = index,
                Text = options[index]
            });
        }

        return question;
    }
}
