using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnCSharpApp.Models;
using LearnCSharpApp.Services;
using LearnCSharpApp.Views;

namespace LearnCSharpApp.ViewModels;

public partial class LessonsViewModel : BaseViewModel
{
    private readonly ILessonService _lessonService;
    private readonly List<Lesson> _allLessons = [];

    [ObservableProperty]
    private string searchText = string.Empty;

    public ObservableCollection<Lesson> Lessons { get; } = [];

    public LessonsViewModel(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    public async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var lessons = await _lessonService.GetLessonsAsync();

            _allLessons.Clear();
            _allLessons.AddRange(lessons);
            ApplySearch();
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplySearch();
    }

    [RelayCommand]
    private async Task OpenLessonAsync(Lesson lesson)
    {
        if (lesson is null)
        {
            return;
        }

        var parameters = new Dictionary<string, object>
        {
            [nameof(Lesson)] = lesson
        };

        await Shell.Current.GoToAsync(nameof(LessonDetailPage), parameters);
    }

    private void ApplySearch()
    {
        var query = SearchText.Trim();
        var filteredLessons = string.IsNullOrWhiteSpace(query)
            ? _allLessons
            : _allLessons
                .Where(lesson =>
                    lesson.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    lesson.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    lesson.Difficulty.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

        Lessons.Clear();
        foreach (var lesson in filteredLessons)
        {
            Lessons.Add(lesson);
        }
    }
}
