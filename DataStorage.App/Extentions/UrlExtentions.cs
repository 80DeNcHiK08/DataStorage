using DataStorage.App.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string token, string scheme)
        {
            return urlHelper.Action(
                action: "ConfirmEmail",
                controller: "Account",
                values: new { userId, token },
                protocol: scheme);
        }
        public static string ResetPasswordLink(this IUrlHelper urlHelper, string userId, string token, string scheme)
        {
            return urlHelper.Action(
                action: "ResetPassword",
                controller: "Account",
                values: new { userId, token },
                protocol: scheme);
        }
    }
}
