using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopStation
{
    public partial class UrlForm : Form
    {
        public UrlForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel aLabel = sender as LinkLabel;


            /* Urlを開く */
            if (aLabel.Text != "")
            {
                System.Diagnostics.Process.Start(aLabel.Text);

                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            String aQRUrl = "http://chart.apis.google.com/chart?cht=qr&choe=UTF-8&chs=320x320&chld=h&chl=" + linkLabel1.Text + "";


            
                System.Diagnostics.Process.Start(aQRUrl);

                Close();
        }
    }
}
