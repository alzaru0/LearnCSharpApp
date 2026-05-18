using CommunityToolkit.Mvvm.ComponentModel;
using LearnCSharpApp.Services;

namespace LearnCSharpApp.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly ThemeService _themeService;

    [ObservableProperty]
    private bool isDarkTheme;

    public SettingsViewModel(ThemeService themeService)
    {
        _themeService = themeService;
        isDarkTheme = _themeService.IsDarkTheme;
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        _themeService.IsDarkTheme = value;
    }
}
