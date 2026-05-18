using LearnCSharpApp.Models;

namespace LearnCSharpApp.Services;

public interface IProgressService
{
    Task<IReadOnlyList<LessonProgress>> GetLessonsAsync();

    Task CompleteNextLessonAsync();

    Task ResetProgressAsync();
}
