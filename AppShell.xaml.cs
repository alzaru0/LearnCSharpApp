using LearnCSharpApp.Views;

namespace LearnCSharpApp;

public partial class AppShell : Shell
{
	public AppShell(
		HomePage homePage,
		LessonsPage lessonsPage,
		ProgressPage progressPage,
		SettingsPage settingsPage)
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(LessonDetailPage), typeof(LessonDetailPage));

		// Нижняя навигация оставляет основные разделы приложения доступными в один тап.
		Items.Add(new TabBar
		{
			Items =
			{
				CreateTab("Главная", nameof(HomePage), "tab_home.svg", homePage),
				CreateTab("Уроки", nameof(LessonsPage), "tab_lessons.svg", lessonsPage),
				CreateTab("Прогресс", nameof(ProgressPage), "tab_progress.svg", progressPage),
				CreateTab("Настройки", nameof(SettingsPage), "tab_settings.svg", settingsPage)
			}
		});
	}

	private static Tab CreateTab(string title, string route, string icon, ContentPage page)
	{
		return new Tab
		{
			Title = title,
			Route = route,
			Icon = icon,
			Items =
			{
				new ShellContent
				{
					Title = title,
					Icon = icon,
					Content = page
				}
			}
		};
	}
}
