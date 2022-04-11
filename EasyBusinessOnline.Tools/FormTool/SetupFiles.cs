using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public partial class SetupFiles : Form
    {
        public SetupFiles()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string path = textBox1.Text+"\\";
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (!directoryInfo.Exists)
                {
                    return;
                }
                var files = directoryInfo.GetFiles();
                files = files.Where(it => it.Name!= "setup.exe" && it.Extension != ".iss" && it.Extension != ".pdb").ToArray();
                string str = string.Join(Environment.NewLine,
                    files.Select(it => $"Source: \"{it.FullName.Replace(path, "")}\"; DestDir: \"{{app}}\"; Flags: ignoreversion"));
                textBox2.Text = str;
                var template = new FileInfo(path+"template.iss");
                var config = new FileInfo(path + "config.iss");
                if (template.Exists)
                {
                    string content=File.ReadAllText(template.FullName, Encoding.UTF8);
                    content=content.Replace("{files}", str);
                    File.WriteAllText(config.FullName, content, Encoding.UTF8);
                    MessageBox.Show("自动替换"+ config.FullName);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
