using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Application.Environment;
using ReSharePoint.Common;

namespace ReSharePoint.Activation
{
    [ZoneMarker]
    public class ZoneMarker 
    {
    }

    [ZoneActivator]
    public class ProductActivator : IActivate<IBasicProductZone>, IActivate<IProProductZone>
    {
        bool IActivate<IBasicProductZone>.ActivatorEnabled()
        {
            return true;
        }

        bool IActivate<IProProductZone>.ActivatorEnabled()
        {
            return CheckLicenseType();
        }

        private bool CheckLicenseType()
        {
            // check the license and return valid result
            return true;
        }
    }
   
}
