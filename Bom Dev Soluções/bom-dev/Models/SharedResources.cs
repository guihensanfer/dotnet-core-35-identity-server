using Microsoft.Extensions.Localization;

namespace Bom_Dev.Models
{
    public class SharedResources
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SharedResources(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public string GetLocalizedString(string key)
        {
            return _localizer[key];
        }
    }
}
