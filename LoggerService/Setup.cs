using NLog;
using Microsoft.Extensions.DependencyInjection;
using Contracts;

namespace LoggerService;

public static class Setup
{
    public static void AddLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();
}