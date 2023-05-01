using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Data.Context.Language;
using Microsoft.AspNetCore.Localization;

namespace Bom_Dev.Models
{
    public class UpdateUserLanguageMiddleware
    {        
        private readonly RequestDelegate _next;

        public UpdateUserLanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserLanguage userLanguage)
        {
            var culture = context.Features.Get<IRequestCultureFeature>()?.RequestCulture?.Culture;
            if (culture != null)
            {
                userLanguage.Language = culture.Name;
            }

            await _next(context);
        }        
    }
}
