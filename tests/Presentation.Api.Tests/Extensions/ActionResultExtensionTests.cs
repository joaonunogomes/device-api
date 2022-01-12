using Microsoft.AspNetCore.Mvc;

namespace DeviceApi.Presentation.Api.Tests.Extensions
{

    public static class ActionResultExtensionTests
    {
        public static T GetValueFromOkObjectResult<T>(this IActionResult actionResult) where T : class
        {
            var result = actionResult as OkObjectResult;

            return result?.Value as T;
        }

        public static T GetValueFromCreatedAtActionResult<T>(this IActionResult actionResult) where T : class
        {
            var result = actionResult as CreatedAtActionResult;

            return result?.Value as T;
        }
    }
}
