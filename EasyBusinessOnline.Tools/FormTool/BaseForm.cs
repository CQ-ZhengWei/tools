using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public class BaseForm:Form
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Icon = Properties.Resources.favicon;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true;
            this.Hide();
        }
        public void AllSelect(TextBox textBox)
        {
            textBox.KeyDown += TextBox_KeyDown;
            textBox.ScrollBars = ScrollBars.Both;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                ((TextBox)sender).SelectAll();
        }
    }
}
