using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Data.Context.Language;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;

namespace Bom_Dev.Models
{
    public class UpdateUserLanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestLocalizationOptions _options;

        public UpdateUserLanguageMiddleware(RequestDelegate next, IOptions<RequestLocalizationOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context, UserLanguage userLanguage)
        {
            var lang = context.Features.Get<IRequestCultureFeature>()?.RequestCulture?.Culture?.Name;
            var path = context.Request.Path.Value;     
            
            if(path.Contains(SupportedCultures.ChangeURLParameter, StringComparison.OrdinalIgnoreCase))
            {
                string returnUrl = "/";
                string newCulture = SupportedCultures.DefaultLanguage;

                if(context.Request.Query.TryGetValue("culture", out var cultures))
                {
                    newCulture = cultures.FirstOrDefault();
                }
                if (context.Request.Query.TryGetValue("returnUrl", out var urls))
                {
                    returnUrl = urls.FirstOrDefault();
                }

                // Change language request
                context.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(newCulture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                context.Response.Redirect(returnUrl);

                return;
            }

            if (!path.StartsWith($"/{lang}", System.StringComparison.OrdinalIgnoreCase))
            {                                
                var targetUrl = $"/{lang.ToLower()}";

                if (context.Request.Path.HasValue && !context.Request.Path.Value.Equals("/"))
                {
                    SupportedCultures.GetCultures().ForEach(x => {
                        path = path.Replace($"/{x.Name}", string.Empty, System.StringComparison.OrdinalIgnoreCase);
                    });

                    targetUrl += path;
                }

                context.Response.Redirect(targetUrl);
                
                return;
            }

            if (lang != null)
            {
                userLanguage.Language = lang;
            }

            await _next(context);
        }
    }
}
