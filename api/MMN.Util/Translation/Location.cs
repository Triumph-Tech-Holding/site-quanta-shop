using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MMN.Util.Translation.Resources;
using System.Globalization;

namespace MMN.Util.Translation
{
    public class Location : ILocation
    {
        private readonly IStringLocalizer _localizer;
        public Location()
        {
            var options = Options.Create(new LocalizationOptions());
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<Traducoes>(factory);
            _localizer = localizer;
        }

        public  string GetTranslation(string chaveTraducao, CultureInfo cultura = null)
        {
            if (string.IsNullOrEmpty(chaveTraducao)) return "";
            
            //if (cultura != null)
            //    return _localizer.WithCulture(cultura)[chaveTraducao];

            return _localizer[chaveTraducao];
        }
    }
}
