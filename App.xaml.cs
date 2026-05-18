namespace LearnCSharpApp;

public partial class App : Application
{
	private readonly AppShell _appShell;

	public App(AppShell appShell, Services.ThemeService themeService)
	{
		InitializeComponent();
		themeService.ApplySavedTheme();
		_appShell = appShell;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(_appShell);
	}
}