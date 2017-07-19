using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace RailwayEssential17
{
    [ProvideEditorExtension(typeof(TrackEditor.TrackEditorFactory), ".track", 151, /* priority */ ProjectGuid = "{D31B3FF9-61C1-49C2-9518-E62C91F0AC63}", TemplateDir = @"Templates\Projects", NameResourceID = 113, DefaultName = "RailwayEssential17")]
    [ProvideKeyBindingTable(TrackEditor.Globals.RailwayEssentialGlobals.GuidRailwayEssentialPackageTrackEditorFactoryString, 114)]
    [ProvideEditorLogicalView(typeof(TrackEditor.TrackEditorFactory), VSConstants.LOGVIEWID.UserChooseView_string)]

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(RailwayEssential17.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class RailwayEssential17 : Package
    {
        public const string PackageGuidString = "3761a69a-bf01-4a33-a360-86294ccb1028";

        public RailwayEssential17()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            RegisterEditorFactory(new TrackEditor.TrackEditorFactory());
        }
    }
}
