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
    public partial class Hump : BaseForm
    {
        public Hump()
        {
            InitializeComponent();
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            this.textBox2.TextChanged -= new EventHandler(TextBox2_TextChanged);
            textBox2.Text = text.GetCamelCase();
            this.textBox2.TextChanged += new EventHandler(TextBox2_TextChanged);
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            var text = textBox2.Text;
            this.textBox1.TextChanged -= new EventHandler(TextBox1_TextChanged);
            textBox1.Text = text.ReCamelCase();
            this.textBox1.TextChanged += new EventHandler(TextBox1_TextChanged);
        }
    }
}
