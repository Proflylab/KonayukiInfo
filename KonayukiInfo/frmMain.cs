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
using Microsoft.WindowsAPICodePack.Shell;
using System.Reflection;
using System.Text.RegularExpressions;

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
            lblVersion.Text = $"{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            MediaInfo MI = new MediaInfo();
            MI.Open(file);
            MI.Option("Complete", "0");
            foreach (var line in Regex.Split(MI.Inform(), "\r\n"))
            {
                switch (line)
                {
                    case "General":
                    case "Video":
                    case "Audio":
                    case "Text":
                    case "Other":
                    case "Image":
                    case "Menu":
                        rtbInfo.SelectionFont = new Font(rtbInfo.SelectionFont.FontFamily, 10f, FontStyle.Bold);
                        rtbInfo.SelectionColor = Color.FromArgb(255, 51, 153);
                        rtbInfo.SelectedText = line + "\r\n";
                        break;
                    default:
                        rtbInfo.SelectionColor = Color.Black;
                        rtbInfo.SelectedText += line + "\r\n";
                        break;
                }
            }
            MI.Close();
            ShellFile shellFile = ShellFile.FromFilePath(file);
            ShellThumbnail thumbnail = shellFile.Thumbnail;
            picThumbnail.Image = thumbnail.Bitmap;
            lblFile.Text = Path.GetFileName(file);
            lblDateCreated.Text = "Created : " + new FileInfo(file).CreationTime.ToString();
            lblDateModified.Text = "Modified : " + new FileInfo(file).LastWriteTime.ToString();
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