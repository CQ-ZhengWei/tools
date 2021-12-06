using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public partial class HtmlToJs : BaseForm
    {
        public HtmlToJs()
        {
            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            bool changeLine = checkBox1.Checked;
            string text = textBox1.Text;
            string content = "";
            if (!string.IsNullOrEmpty(text))
            {
                var arr = text.Split('\n');
                foreach (var item in arr)
                {
                    if (changeLine)
                    {
                        content += "+'"+ item.Trim() + "'"+"\r\n";
                    }
                    else
                    {
                        content += item.Trim();
                    }
                }
            }
            if(changeLine)
            {
                textBox2.Text = content;
            }
            else
            {
                textBox2.Text = content.Replace("'", "\\'");
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void HtmlToJs_Load(object sender, EventArgs e)
        {
            AllSelect(textBox1);
            AllSelect(textBox2);
        }
    }
}
