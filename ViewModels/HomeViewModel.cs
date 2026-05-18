using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnCSharpApp.Services;
using LearnCSharpApp.Views;

namespace LearnCSharpApp.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IProgressService _progressService;

    [ObservableProperty]
    private int completedLessons;

    [ObservableProperty]
    private int totalLessons;

    [ObservableProperty]
    private double progressValue;

    public HomeViewModel(IProgressService progressService)
    {
        _progressService = progressService;
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
            var lessons = await _progressService.GetLessonsAsync();

            TotalLessons = lessons.Count;
            CompletedLessons = lessons.Count(lesson => lesson.IsCompleted);
            ProgressValue = TotalLessons == 0 ? 0 : (double)CompletedLessons / TotalLessons;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OpenLessonsAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(LessonsPage)}");
    }
}
