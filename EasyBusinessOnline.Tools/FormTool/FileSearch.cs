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
    public partial class FileSearch : Form
    {
        public FileSearch()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text= folderBrowserDialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string word = textBox2.Text;
            string path = textBox1.Text;
            if (string.IsNullOrEmpty(path)|| string.IsNullOrEmpty(word))
            {
                return;
            }
            FileInfo[] files = new DirectoryInfo(path).GetFiles();
            string str = "";
            foreach (var item in files)
            {
                string content = File.ReadAllText(item.FullName);
                if (content.Contains(word))
                {
                    str += item.Name+"\r\n";
                }
            }
            textBox3.Text = str;
        }
    }
}
