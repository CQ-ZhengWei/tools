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
    public partial class StringCompression : Form
    {
        public StringCompression()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "压缩前（长度：" + textBox1.Text.Length + "）：";
            label2.Text = "压缩后（长度：" + textBox2.Text.Length + "）：";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "压缩前（长度：" + textBox1.Text.Length + "）：";
            label2.Text = "压缩后（长度：" + textBox2.Text.Length + "）：";
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string text = textBox1.Text;
                if (string.IsNullOrEmpty(text))
                {
                    textBox2.Text = "";
                    return;
                }
                var str = StringCompressionUtils.Compression(text);
                textBox2.Text = str;
            }
            catch (Exception)
            {
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string text = textBox2.Text;
                if (string.IsNullOrEmpty(text))
                {
                    textBox1.Text = "";
                    return;
                }
                var str = StringCompressionUtils.DeCompression(text);
                textBox1.Text = str;
            }
            catch (Exception)
            {
            }
        }
    }
}
