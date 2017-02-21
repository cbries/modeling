using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;

namespace RailwayEssential
{
    [ProvideProjectFactory(typeof(RailwayProjectFactory), "Railway Essential Project",
    "Railway Essential Project Files (*.railway);*.railway", "railway", "railway",
    @"Templates\Projects\Railway Essential", LanguageVsTemplate = "Railway Essential")]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(RailwayDefaults.GuidRailwayProjectPkgString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class RailwayPackage : ProjectPackage
    {
        public override string ProductUserContext => RailwayDefaults.ProviderName;

        public RailwayPackage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
