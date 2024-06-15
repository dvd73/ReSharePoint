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
    public class ProductActivator : IActivateDynamic<IBasicProductZone>, IActivateDynamic<IProProductZone>
    {
        bool IActivateDynamic<IBasicProductZone>.ActivatorEnabled()
        {
            return true;
        }

        bool IActivateDynamic<IProProductZone>.ActivatorEnabled()
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
