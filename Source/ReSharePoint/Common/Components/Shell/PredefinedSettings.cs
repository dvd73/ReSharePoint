using System.IO;
using System.Reflection;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;

namespace ReSharePoint.Common.Components.Shell
{
    [ShellComponent]
    public class PredefinedReSharePointSettings : IHaveDefaultSettingsStream
    {
        public Stream GetDefaultSettingsStream(Lifetime lifetime)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ReSharePoint.Common.Options.PredefinedReSharePointSettings.xml");
            Assertion.AssertNotNull(stream, "stream == null");
            lifetime.OnTermination(stream);

            return stream;
        }


        string IHaveDefaultSettingsStream.Name => "Predefined reSP Settings";
    }
}
