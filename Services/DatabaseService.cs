using LearnCSharpApp.Models;
using SQLite;

namespace LearnCSharpApp.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private bool _isInitialized;

    public DatabaseService()
    {
        SQLitePCL.Batteries_V2.Init();

        // Единая локальная база приложения: прогресс уроков, тесты и сводная статистика.
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "learn_csharp_app.db3");
        _database = new SQLiteAsyncConnection(databasePath);
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        await _initializationLock.WaitAsync();
        try
        {
            if (_isInitialized)
            {
                return;
            }

            // Таблицы создаются один раз при первом обращении к базе.
            await _database.CreateTableAsync<StartedLesson>();
            await _database.CreateTableAsync<CompletedLesson>();
            await _database.CreateTableAsync<QuizResult>();
            await _database.CreateTableAsync<UserProgress>();

            _isInitialized = true;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    public async Task<HashSet<int>> GetCompletedLessonIdsAsync()
    {
        await InitializeAsync();

        var lessons = await _database.Table<CompletedLesson>().ToListAsync();
        return lessons.Select(lesson => lesson.LessonId).ToHashSet();
    }

    public async Task<HashSet<int>> GetStartedLessonIdsAsync()
    {
        await InitializeAsync();

        var lessons = await _database.Table<StartedLesson>().ToListAsync();
        return lessons.Select(lesson => lesson.LessonId).ToHashSet();
    }

    public async Task SaveStartedLessonAsync(int lessonId)
    {
        await InitializeAsync();

        await _database.InsertOrReplaceAsync(new StartedLesson
        {
            LessonId = lessonId,
            StartedAt = DateTime.UtcNow
        });
    }

    public async Task SaveCompletedLessonAsync(int lessonId)
    {
        await InitializeAsync();

        await SaveStartedLessonAsync(lessonId);
        await _database.InsertOrReplaceAsync(new CompletedLesson
        {
            LessonId = lessonId,
            CompletedAt = DateTime.UtcNow
        });
    }

    public async Task ResetCompletedLessonsAsync()
    {
        await InitializeAsync();

        await _database.DeleteAllAsync<StartedLesson>();
        await _database.DeleteAllAsync<CompletedLesson>();
    }

    public async Task SaveQuizResultAsync(QuizResult result)
    {
        await InitializeAsync();

        result.CompletedAt = DateTime.UtcNow;
        result.Percent = result.TotalQuestions == 0 ? 0 : (double)result.Score / result.TotalQuestions;
        await _database.ExecuteAsync("DELETE FROM QuizResult WHERE LessonId = ?", result.LessonId);
        await _database.InsertAsync(result);
    }

    public async Task<List<QuizResult>> GetQuizResultsAsync()
    {
        await InitializeAsync();

        var results = await _database
            .Table<QuizResult>()
            .OrderByDescending(result => result.CompletedAt)
            .ToListAsync();

        return results
            .GroupBy(result => result.LessonId)
            .Select(group => group.First())
            .ToList();
    }

    public async Task ResetQuizResultsAsync()
    {
        await InitializeAsync();

        await _database.DeleteAllAsync<QuizResult>();
    }

    public async Task<UserProgress> GetUserProgressAsync()
    {
        await InitializeAsync();

        return await _database.FindAsync<UserProgress>(1) ?? new UserProgress
        {
            UpdatedAt = DateTime.UtcNow
        };
    }

    public async Task<UserProgress> RecalculateUserProgressAsync(int totalLessons)
    {
        await InitializeAsync();

        var completedLessons = await _database.Table<CompletedLesson>().CountAsync();
        var quizResults = await GetQuizResultsAsync();
        var bestResult = quizResults
            .OrderByDescending(result => result.Percent)
            .ThenByDescending(result => result.Score)
            .FirstOrDefault();

        var progress = new UserProgress
        {
            Id = 1,
            CompletedLessons = completedLessons,
            TotalLessons = totalLessons,
            TestsTaken = quizResults.Count,
            BestQuizScore = bestResult?.Score ?? 0,
            BestQuizTotalQuestions = bestResult?.TotalQuestions ?? 0,
            OverallProgress = totalLessons == 0 ? 0 : (double)completedLessons / totalLessons,
            UpdatedAt = DateTime.UtcNow
        };

        await _database.InsertOrReplaceAsync(progress);
        return progress;
    }
}
