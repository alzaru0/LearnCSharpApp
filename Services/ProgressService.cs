using LearnCSharpApp.Models;

namespace LearnCSharpApp.Services;

public class ProgressService : IProgressService
{
    private readonly ILessonService _lessonService;
    private readonly DatabaseService _databaseService;

    public ProgressService(ILessonService lessonService, DatabaseService databaseService)
    {
        _lessonService = lessonService;
        _databaseService = databaseService;
    }

    public async Task<IReadOnlyList<LessonProgress>> GetLessonsAsync()
    {
        var lessons = await _lessonService.GetLessonsAsync();

        // Сводный прогресс пересчитывается из фактических данных, а не хранится вручную.
        await _databaseService.RecalculateUserProgressAsync(lessons.Count);

        return lessons
            .Select(lesson => new LessonProgress
            {
                LessonKey = lesson.Id.ToString(),
                Title = lesson.Title,
                Description = lesson.Description,
                Order = lesson.Id,
                IsCompleted = lesson.IsCompleted,
                UpdatedAt = DateTime.UtcNow
            })
            .ToList();
    }

    public async Task CompleteNextLessonAsync()
    {
        var lessons = await GetLessonsAsync();
        var nextLesson = lessons.FirstOrDefault(lesson => !lesson.IsCompleted);

        if (nextLesson is null)
        {
            return;
        }

        await _lessonService.MarkLessonCompletedAsync(nextLesson.Order);
    }

    public async Task ResetProgressAsync()
    {
        await _lessonService.ResetLessonProgressAsync();
        await _databaseService.ResetQuizResultsAsync();
    }
}
