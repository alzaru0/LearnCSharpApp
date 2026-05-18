using Microsoft.Extensions.Logging;
using LearnCSharpApp.Services;
using LearnCSharpApp.ViewModels;
using LearnCSharpApp.Views;

namespace LearnCSharpApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<ThemeService>();
		builder.Services.AddSingleton<ILessonService, LessonService>();
		builder.Services.AddSingleton<IQuizService, QuizService>();
		builder.Services.AddSingleton<IProgressService, ProgressService>();
		builder.Services.AddTransient<HomeViewModel>();
		builder.Services.AddTransient<LessonsViewModel>();
		builder.Services.AddTransient<LessonDetailViewModel>();
		builder.Services.AddTransient<QuizViewModel>();
		builder.Services.AddTransient<ProgressViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<LessonsPage>();
		builder.Services.AddTransient<LessonDetailPage>();
		builder.Services.AddTransient<QuizPage>();
		builder.Services.AddTransient<ProgressPage>();
		builder.Services.AddTransient<SettingsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
