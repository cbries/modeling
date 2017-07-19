using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace RailwayEssential17.TrackEditor
{
    public sealed class TrackEditorFactory : IVsEditorFactory, IDisposable
    {
        private ServiceProvider _vsServiceProvider;

        public void Dispose()
        {
            if (_vsServiceProvider != null)
                _vsServiceProvider.Dispose();
        }

        public int CreateEditorInstance(uint grfCreateDoc, string pszMkDocument, string pszPhysicalView, IVsHierarchy pvHier,
            uint itemid, IntPtr punkDocDataExisting, out IntPtr ppunkDocView, out IntPtr ppunkDocData,
            out string pbstrEditorCaption, out Guid pguidCmdUi, out int pgrfCdw)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering {0} CreateEditorInstance()", this));

            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            pguidCmdUi = Globals.RailwayEssentialGlobals.GuidRailwayEssentialPackageTrackEditorFactory;
            pgrfCdw = 0;
            pbstrEditorCaption = null;

            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0)
            {
                return VSConstants.E_INVALIDARG;
            }
            if (punkDocDataExisting != IntPtr.Zero)
            {
                return VSConstants.VS_E_INCOMPATIBLEDOCDATA;
            }

            // Create the Document (editor)
            TrackEditorPane newEditor = new TrackEditorPane();
            ppunkDocView = Marshal.GetIUnknownForObject(newEditor);
            ppunkDocData = Marshal.GetIUnknownForObject(newEditor);
            pbstrEditorCaption = string.Empty;

            return VSConstants.S_OK;
        }

        public int SetSite(IServiceProvider psp)
        {
            _vsServiceProvider = new ServiceProvider(psp);

            return VSConstants.S_OK;
        }

        public object GetService(Type serviceType)
        {
            return _vsServiceProvider.GetService(serviceType);
        }

        public int Close()
        {
            return VSConstants.S_OK;
        }

        public int MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
        {
            pbstrPhysicalView = null;

            // we support only a single physical view
            if (VSConstants.LOGVIEWID_Primary == rguidLogicalView)
            {
                return VSConstants.S_OK;        // primary view uses NULL as pbstrPhysicalView
            }
            else if (VSConstants.LOGVIEWID.TextView_guid == rguidLogicalView)
            {
                // Our editor supports FindInFiles, therefore we need to declare support for LOGVIEWID_TextView.
                // In addition our EditorPane implements IVsCodeWindow and we also provide the 
                // VSSettings (pkgdef) metadata statement that we support LOGVIEWID_TextView via the following
                // attribute on our Package class:
                // [ProvideEditorLogicalView(typeof(EditorFactory), VSConstants.LOGVIEWID.TextView_string)]

                return VSConstants.S_OK;
            }
            else
            {
                return VSConstants.E_NOTIMPL;   // you must return E_NOTIMPL for any unrecognized rguidLogicalView values
            }
        }

        #region Thirdparty Methods

        public EnvDTE.Project GetProject(IVsHierarchy hierarchy)
        {
            object project;
            ErrorHandler.ThrowOnFailure
            (hierarchy.GetProperty(
                VSConstants.VSITEMID_ROOT,
                (int)__VSHPROPID.VSHPROPID_ExtObject,
                out project));
            return (project as EnvDTE.Project);
        }

        #endregion
    }
}
