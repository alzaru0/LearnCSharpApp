using LearnCSharpApp.Models;

namespace LearnCSharpApp.Services;

public class LessonService : ILessonService
{
    private readonly DatabaseService _databaseService;

    // Учебный контент хранится в коде, а пользовательский прогресс подтягивается из SQLite.
    private static readonly Lesson[] Lessons =
    [
        new()
        {
            Id = 1,
            Title = "Переменные",
            Description = "Типы данных, объявление переменных и работа со значениями.",
            Content = "Переменная - это именованная область памяти, в которой хранится значение. В C# каждая переменная имеет тип: например, int для целых чисел, double для дробных чисел, string для текста и bool для логических значений. Тип помогает компилятору понимать, какие операции можно выполнять с данными. Обычно переменная объявляется через тип, имя и начальное значение. Для значений, которые не должны изменяться, используется ключевое слово const.",
            Difficulty = "Легко",
            IsCompleted = false
        },
        new()
        {
            Id = 2,
            Title = "Условия",
            Description = "Операторы if, else, switch и логика ветвления программы.",
            Content = "Условия позволяют программе выбирать разные действия в зависимости от ситуации. В C# для этого используются if, else if, else и switch. Условное выражение должно возвращать bool: true или false. Такой подход помогает создавать интерактивную логику: проверять ввод пользователя, сравнивать значения, выбирать режим работы приложения.",
            Difficulty = "Легко",
            IsCompleted = false
        },
        new()
        {
            Id = 3,
            Title = "Циклы",
            Description = "Повторение действий с помощью for, while, do while и foreach.",
            Content = "Циклы используются, когда один и тот же блок кода нужно выполнить несколько раз. Цикл for удобен, когда известно количество повторений. while применяется, когда повторение зависит от условия. foreach используется для перебора коллекций и массивов. Важно следить, чтобы условие цикла когда-нибудь становилось ложным, иначе программа может попасть в бесконечный цикл.",
            Difficulty = "Средне",
            IsCompleted = false
        },
        new()
        {
            Id = 4,
            Title = "Методы",
            Description = "Создание переиспользуемых блоков кода с параметрами и результатом.",
            Content = "Метод - это отдельный блок кода, который выполняет конкретную задачу. Методы помогают не повторять один и тот же код, делают программу понятнее и проще для сопровождения. Метод может принимать параметры, возвращать результат или просто выполнять действие. Хорошее имя метода должно описывать его назначение.",
            Difficulty = "Средне",
            IsCompleted = false
        },
        new()
        {
            Id = 5,
            Title = "Массивы",
            Description = "Хранение наборов однотипных данных и обращение к элементам по индексу.",
            Content = "Массив хранит несколько значений одного типа под одним именем. Каждый элемент имеет индекс, а индексация начинается с нуля. Массивы удобны для списков оценок, чисел, имен и других однотипных данных. Размер массива задается при создании, а количество элементов можно получить через свойство Length.",
            Difficulty = "Средне",
            IsCompleted = false
        },
        new()
        {
            Id = 6,
            Title = "Основы ООП",
            Description = "Классы, объекты, свойства, методы и базовые принципы ООП.",
            Content = "Объектно-ориентированное программирование строится вокруг классов и объектов. Класс описывает структуру: свойства и методы. Объект является конкретным экземпляром класса. ООП помогает моделировать реальные сущности в программе, разделять ответственность и писать более поддерживаемый код. Базовые принципы ООП: инкапсуляция, наследование и полиморфизм.",
            Difficulty = "Сложно",
            IsCompleted = false
        }
    ];

    public LessonService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<IReadOnlyList<Lesson>> GetLessonsAsync()
    {
        var startedLessonIds = await _databaseService.GetStartedLessonIdsAsync();
        var completedLessonIds = await _databaseService.GetCompletedLessonIdsAsync();

        var lessons = Lessons
            .Select(lesson => new Lesson
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description,
                Content = lesson.Content,
                Difficulty = lesson.Difficulty,
                IsStarted = startedLessonIds.Contains(lesson.Id) || completedLessonIds.Contains(lesson.Id),
                IsCompleted = completedLessonIds.Contains(lesson.Id)
            })
            .ToList();

        return lessons;
    }

    public async Task MarkLessonStartedAsync(int lessonId)
    {
        await _databaseService.SaveStartedLessonAsync(lessonId);
    }

    public async Task MarkLessonCompletedAsync(int lessonId)
    {
        await _databaseService.SaveCompletedLessonAsync(lessonId);
        await _databaseService.RecalculateUserProgressAsync(Lessons.Length);
    }

    public async Task ResetLessonProgressAsync()
    {
        await _databaseService.ResetCompletedLessonsAsync();
        await _databaseService.RecalculateUserProgressAsync(Lessons.Length);
    }
}
