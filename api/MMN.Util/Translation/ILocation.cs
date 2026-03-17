using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Util.Translation
{
    public interface ILocation
    {
        string GetTranslation(string chaveTraducao, System.Globalization.CultureInfo cultura = null);
    }
}
