using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Skimur.Web.Infrastructure
{
    public static class Routes
    {
        public static object UrlParameter { get; private set; }

        public static void Register(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapRouteShim(
                name: "Subs",
                url: "subs",
                defaults: new { controller = "Subs", action = "Popular" });

            endpoints.MapRouteShim(
                name: "Frontpage",
                url: "",
                defaults: new { controller = "Posts", action = "Frontpage" });

            endpoints.MapRouteShim(
                name: "Avatar",
                url: "avatar/{key}",
                defaults: new { controller = "Avatar", action = "Key" });
            
            endpoints.MapRouteShim(
                name: "Default",
                url: "{controller}/{action}/{id?}",
                defaults: new { controller = "Home", action = "Index" }
            ); 
        }

        public static void MapRouteShim(this IEndpointRouteBuilder endpoints, string name, string url, object defaults)
        {
            endpoints.MapControllerRoute(name: name, pattern: url, defaults: defaults);
        }
    }
}
