using LearnCSharpApp.Models;

namespace LearnCSharpApp.Services;

public interface IQuizService
{
    Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync();
}
