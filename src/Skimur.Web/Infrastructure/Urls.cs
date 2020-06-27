using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace Skimur.Web.Infrastructure
{
    public static class Urls
    {
        public static string ModifyQuery(this HttpContext context, string name, string value)
        {
            var query = new Dictionary<string, string>();

            if (context.Request.Query != null && context.Request.Query.Count > 0)
            {
                foreach (var q in context.Request.Query)
                {
                    query[q.Key] = q.Value;
                }
            }

            query[name] = value;

            return QueryHelpers.AddQueryString(context.Request.Path, query);
        }

        public static string ModifyQuery(string url, string name, string value)
        {
            var query = new Dictionary<string, string>();

            var currentQuery = QueryHelpers.ParseQuery(url.Contains("?") ? url.Substring(url.IndexOf("?")) : string.Empty);

            if (currentQuery != null && currentQuery.Count > 0)
            {
                foreach (var q in currentQuery)
                {
                    query[q.Key] = q.Value;
                }
            }

            query[name] = value;

            return QueryHelpers.AddQueryString(url.Contains("?") ? url.Substring(0, url.IndexOf("?")) : string.Empty, query);
        }


        public static string Subs(this IUrlHelper urlHelper, string query = null)
        {
            return urlHelper.RouteUrl("Subs", new { query });
        }

        public static string Login(this IUrlHelper urlHelper)
        {
            return urlHelper.Action("Login", "Account");
        }

        public static string Register(this IUrlHelper urlHelper)
        {
            return urlHelper.Action("Register", "Account");
        }

        public static string ForgotPassword(this IUrlHelper urlHelper)
        {
            return urlHelper.Action("ForgotPassword", "Account");
        }

        public static string AvatarUrl(this IUrlHelper urlHelper, string avatarIdentifier)
        {
            if (string.IsNullOrEmpty(avatarIdentifier))
            {
                return urlHelper.Content("~/img/avatar.jpg");
            }

            return urlHelper.Content("~/avatars/" + avatarIdentifier);
        }

        // policies
        public static string Privacy(this IUrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Privacy");
        }

        public static string RouteUrl(this IUrlHelper urlHelper, string routeName)
        {
            return urlHelper.RouteUrl(new UrlRouteContext { RouteName = routeName });
        }

        public static string RouteUrl(this IUrlHelper urlHelper, string routeName, object values)
        {
            return urlHelper.RouteUrl(new UrlRouteContext { RouteName = routeName, Values = values });
        }
    }
}
