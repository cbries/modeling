using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace RailwayEssential.TrackEditor
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    [ComSourceInterfaces(typeof(IVsTextViewEvents))]
    [ComVisible(true)]
    public class TrackEditorPane :
        WindowPane,
        IVsPersistDocData,
        IPersistFileFormat
    {
        private const uint TrackEditorFormat = 0;
        private const string TrackEditorExtension = ".track";

        private string _fileName = string.Empty;

        public string FileName => _fileName;

        public TrackEditorPane() : base(null)
        {
            Trace.WriteLine("<TrackEditor> *** CONSTRUCT *** ");
        }

        public int Load(string pszFilename, uint grfMode, int fReadOnly)
        {
            if (pszFilename == null)
                return VSConstants.E_INVALIDARG;

            Trace.WriteLine("<TrackEditor> Load: " + pszFilename);

            int hr = VSConstants.S_OK;
            try
            {
                IVsUIShell VsUiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                if (VsUiShell != null)
                {
                    // Note: we don't want to throw or exit if this call fails, so
                    // don't check the return code.
                    hr = VsUiShell.SetWaitCursor();
                }

                //string projectDirectory = GetCurrentProjectDirName();
                //if (string.IsNullOrEmpty(projectDirectory))
                //    return VSConstants.E_INVALIDARG;
                //bool failed = projectDirectory.Length <= 0;
                //if (failed)
                //    return VSConstants.E_INVALIDARG;

                //TcHmiSvgEditorControl svgEditor = this.Content as TcHmiSvgEditorControl;
                //if (svgEditor != null)
                //    svgEditor.LoadFile(this, pszFilename);

                //FileAttributes fileAttrs = File.GetAttributes(pszFilename);

                //int isReadOnly = (int)fileAttrs & (int)FileAttributes.ReadOnly;

                //// Set readonly if either the file is readonly for the user or on the file system
                //if (0 == isReadOnly && 0 == fReadOnly)
                //    SetReadOnly(false);
                //else
                //    SetReadOnly(true);
            }
            finally
            {
                // ignore
            }

            return hr;
        }

        public int Save(string pszFilename, int fRemember, uint nFormatIndex)
        {
            // TODO
            Trace.WriteLine("<TrackEditor> Save: " + pszFilename);

            return VSConstants.S_OK;
        }

        public int GetGuidEditorType(out Guid pClassID)
        {
            return ((IPersistFileFormat)this).GetClassID(out pClassID);
        }

        public int IsDocDataDirty(out int pfDirty)
        {
            return ((IPersistFileFormat)this).IsDirty(out pfDirty);
        }

        public int SetUntitledDocPath(string pszDocDataPath)
        {
            return ((IPersistFileFormat)this).InitNew(TrackEditorFormat);
        }

        public int LoadDocData(string pszMkDocument)
        {
            return ((IPersistFileFormat)this).Load(pszMkDocument, 0, 0);
        }

        public int SaveDocData(VSSAVEFLAGS dwSave, out string pbstrMkDocumentNew, out int pfSaveCanceled)
        {
            pbstrMkDocumentNew = null;
            pfSaveCanceled = 0;
            return VSConstants.S_OK;
        }

        public int Close()
        {
            return VSConstants.S_OK;
        }

        public int OnRegisterDocData(uint docCookie, IVsHierarchy pHierNew, uint itemidNew)
        {
            return VSConstants.S_OK;
        }

        public int RenameDocData(uint grfAttribs, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int IsDocDataReloadable(out int pfReloadable)
        {
            pfReloadable = 1;
            return VSConstants.S_OK;
        }

        public int ReloadDocData(uint grfFlags)
        {
            return ((IPersistFileFormat)this).Load(_fileName, grfFlags, 0);
        }

        int IPersist.GetClassID(out Guid pClassID)
        {
            pClassID = Globals.RailwayEssentialGlobals.GuidRailwayEssentialPackageTrackEditorFactory;
            return VSConstants.S_OK;
        }

        public int IsDirty(out int pfIsDirty)
        {
            pfIsDirty = 0;
            return VSConstants.S_OK;
        }

        public int InitNew(uint nFormatIndex)
        {
            if (nFormatIndex != TrackEditorFormat)
                return VSConstants.E_INVALIDARG;
            return VSConstants.S_OK;
        }

        public int SaveCompleted(string pszFilename)
        {
            return VSConstants.S_OK;
        }

        public int GetCurFile(out string ppszFilename, out uint pnFormatIndex)
        {
            pnFormatIndex = TrackEditorFormat;
            ppszFilename = _fileName;
            return VSConstants.S_OK;
        }

        public int GetFormatList(out string ppszFormatList)
        {
            char Endline = (char)'\n';
            string formatList = string.Format(CultureInfo.InvariantCulture, "Railway Essential Track Editor (*{0}){1}*{0}{1}{1}", TrackEditorExtension, Endline);
            ppszFormatList = formatList;
            return VSConstants.S_OK;
        }

        int IPersistFileFormat.GetClassID(out Guid pClassID)
        {
            ErrorHandler.ThrowOnFailure(((IPersist)this).GetClassID(out pClassID));
            return VSConstants.S_OK;
        }
    }
}
