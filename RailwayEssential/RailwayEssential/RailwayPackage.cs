﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;

namespace RailwayEssential
{
    [ProvideEditorExtension(typeof(TrackEditor.TrackEditorFactory), ".track", 151, /* priority */ ProjectGuid = "{D31B3FF9-61C1-49C2-9518-E62C91F0AC63}", TemplateDir = @"Templates\Projects", NameResourceID = 113, DefaultName = "RailwayEssential")]
    [ProvideKeyBindingTable(TrackEditor.Globals.RailwayEssentialGlobals.GuidRailwayEssentialPackageTrackEditorFactoryString, 114)]
    [ProvideEditorLogicalView(typeof(TrackEditor.TrackEditorFactory), VSConstants.LOGVIEWID.UserChooseView_string)]

    [ProvideProjectFactory(typeof(RailwayProjectFactory), "RailwayEssential",
    "Railway Essential Project Files (*.railway);*.railway", "railway", "railway",
    @"Templates\Projects\Railway Essential", LanguageVsTemplate = "RailwayEssential")]
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

            RegisterEditorFactory(new TrackEditor.TrackEditorFactory());
        }
    }
}
