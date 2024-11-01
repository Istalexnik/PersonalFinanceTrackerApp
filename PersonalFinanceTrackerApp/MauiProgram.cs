﻿using Microsoft.Extensions.Logging;
using PersonalFinanceTrackerApp.Data;
using PersonalFinanceTrackerApp.Services;

namespace PersonalFinanceTrackerApp
{
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<TransactionRepository>();


            return builder.Build();
        }
    }
}
