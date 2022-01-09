using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DeviceApi.Infrastructure.CrossCutting.Extensions
{
    public static class LogExtensions
    {
        public static void Error(
            this ILogger logger,
            string message,
            object additionalInfo,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
                AdditionalInfo = additionalInfo
            };

            logger.LogError($"{message} => {{data}}", data);
        }

        public static void Error(
            this ILogger logger,
            string message,
            object additionalInfo,
            Exception exception,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
                AdditionalInfo = additionalInfo,
                Exception = exception
            };

            logger.LogError($"{message} => {{data}}", data);
        }

        public static void Error(
            this ILogger logger,
            string message,
            Exception exception,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
                Exception = exception
            };

            logger.LogError($"{message} => {{data}}", data);
        }

        public static void Error(
            this ILogger logger,
            string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName
            };

            logger.LogError($"{message} => {{data}}", data);
        }

        public static void Info(
            this ILogger logger,
            string message,
            object additionalInfo,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
                AdditionalInfo = additionalInfo
            };

            logger.LogInformation($"{message} => {{data}}", data);
        }

        public static void Info(
            this ILogger logger,
            string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
            };

            logger.LogInformation($"{message} => {{data}}", data);
        }

        public static void Warning(
            this ILogger logger,
            string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
            };

            logger.LogWarning($"{message} => {{data}}", data);
        }

        public static void Warning(
            this ILogger logger,
            string message,
            object additionalInfo,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
                AdditionalInfo = additionalInfo
            };

            logger.LogWarning($"{message} => {{data}}", data);
        }

        public static void Warning(
            this ILogger logger,
            string message,
            object additionalInfo,
            Exception exception,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "")
        {
            var data = new
            {
                ClassName = GetCallerClassName(callerFilePath),
                MethodName = callerName,
                Exception = exception,
                AdditionalInfo = additionalInfo
            };

            logger.LogWarning($"{message} => {{data}}", data);
        }

        private static string GetCallerClassName(string callerFilePath)
        {
            return callerFilePath?
                    .Split(new string[] { "\\", "/" }, StringSplitOptions.None)?
                    .Last()?
                    .Replace(".cs", string.Empty);
        }
    }
}
