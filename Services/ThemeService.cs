namespace LearnCSharpApp.Services;

public class ThemeService
{
    private const string ThemePreferenceKey = "IsDarkTheme";

    public bool IsDarkTheme
    {
        get => Preferences.Get(ThemePreferenceKey, Application.Current?.RequestedTheme == AppTheme.Dark);
        set
        {
            Preferences.Set(ThemePreferenceKey, value);

            if (Application.Current is not null)
            {
                Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
            }
        }
    }

    public void ApplySavedTheme()
    {
        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = IsDarkTheme ? AppTheme.Dark : AppTheme.Light;
        }
    }
}
