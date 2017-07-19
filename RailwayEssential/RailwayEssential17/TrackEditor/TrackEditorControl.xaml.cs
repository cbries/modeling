using System.Diagnostics;
using System.Windows.Controls;

namespace RailwayEssential17.TrackEditor
{
    public partial class TrackEditorControl : UserControl
    {
        public TrackEditorControl()
        {
            InitializeComponent();
        }

        public void LoadFile(string filePath)
        {
            Trace.WriteLine("File: " + filePath);
        }
    }
}
