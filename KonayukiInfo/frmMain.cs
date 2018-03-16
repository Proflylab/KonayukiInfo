using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using MediaInfoLib;
using MetroFramework.Forms;

namespace KonayukiInfo
{
    public partial class frmMain : MetroForm
    {
        string file = string.Empty;
        public frmMain(string file)
        {
            InitializeComponent();
            this.file = file;
        }
        private void SetToolTip(Control control, string text)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(control, text);
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            MediaInfo MI = new MediaInfo();
            MI.Open(file);
            MI.Option("Complete", "0");
            rtbInfo.Text = MI.Inform();
            MI.Close();
            lblFile.Text = Path.GetFileName(file);
            SetToolTip(lblFile, Path.GetFileName(file));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var sfdExport = new SaveFileDialog
            {
                Title = "Export",
                Filter = "Konayuki Info|*.txt",
                FileName = Path.GetFileName(file) + ".txt"
            };
            if (sfdExport.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllText(sfdExport.FileName, rtbInfo.Text.Replace("\n", "\r\n"));
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
