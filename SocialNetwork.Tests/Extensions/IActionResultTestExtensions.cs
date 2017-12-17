namespace SocialNetwork.Tests.Extensions
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;

    public static class IActionResultTestExtensions
    {
        public static void AssertNotFoundView(this IActionResult result)
        {
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Should()
                .Match(s => s.As<ViewResult>().ViewName.ToLower().Contains("notfound"));
        }

        public static void AssertAccessDeniedView(this IActionResult result)
        {
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Should()
                .Match(s => s.As<ViewResult>().ViewName.ToLower().Contains("accessdenied"));
        }
    }
}
