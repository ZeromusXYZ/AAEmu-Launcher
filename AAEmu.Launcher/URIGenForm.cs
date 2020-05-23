using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAEmu.Launcher
{
    public partial class URIGenForm : Form
    {
        private bool BaseEncode { get; set; }

        public URIGenForm()
        {
            InitializeComponent();
        }

        public string encode(string s)
        {
            if (BaseEncode)
                return Uri.EscapeDataString(Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(s.ToArray()),Base64FormattingOptions.None));
            else
                return Uri.EscapeDataString(s);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            // update URI
            string u = LauncherForm.launcherProtocolSchema+"://" ;
            // User/Pass (not recommended)
            BaseEncode = false;
            if (tUserName.Text != string.Empty)
            {
                u += encode(tUserName.Text);
                u += "@";
            }
            u += tServerIP.Text ;
            BaseEncode = cbObfuscate.Checked;
            if (BaseEncode)
                u += "/A";
            if (tConfigName.Text != string.Empty)
                u += "/n" + encode(tConfigName.Text);
            if (tAuthDriver.Text != string.Empty)
                u += "/a" + encode(tAuthDriver.Text);

            if (tPassword.Text != string.Empty)
            {
                if (!BaseEncode)
                {
                    u += "/A";
                    BaseEncode = true;
                    u += "/p";
                    u += encode(tPassword.Text);
                    BaseEncode = false;
                }
                else
                {
                    u += "/p";
                    u += encode(tPassword.Text);
                }
            }

            if (tWebsite.Text != string.Empty)
                u += "/w" + encode(tWebsite.Text);
            if (tNews.Text != string.Empty)
                u += "/f" + encode(tNews.Text);
            if (tPatchLocation.Text != string.Empty)
                u += "/x" + encode(tPatchLocation.Text);

            u += "/";
            tCompiledURI.Text = u ;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tCompiledURI.Text);
        }
    }
}
