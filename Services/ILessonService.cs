using LearnCSharpApp.Models;

namespace LearnCSharpApp.Services;

public interface ILessonService
{
    Task<IReadOnlyList<Lesson>> GetLessonsAsync();

    Task MarkLessonStartedAsync(int lessonId);

    Task MarkLessonCompletedAsync(int lessonId);

    Task ResetLessonProgressAsync();
}
