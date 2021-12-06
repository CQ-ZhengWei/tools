using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public partial class IconfontPath : BaseForm
    {
        public IconfontPath()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {

                MatchCollection matchCollection= new Regex(" d=\".*?\"").Matches(textBox1.Text);
                //<svg t="1594827454594" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="1743" width="200" height="200"><path d="M712.9088 950.6816H311.0912c-52.0192 0-94.4128-42.3936-94.4128-94.4128V167.7312c0-52.0192 42.3936-94.4128 94.4128-94.4128h402.0224c52.0192 0 94.4128 42.3936 94.4128 94.4128v688.7424a94.8224 94.8224 0 0 1-94.6176 94.208z m-401.8176-839.68c-31.1296 0-56.7296 25.3952-56.7296 56.7296v688.7424c0 31.1296 25.3952 56.5248 56.7296 56.5248h402.0224c31.1296 0 56.5248-25.3952 56.5248-56.5248V167.7312c0-31.1296-25.3952-56.7296-56.5248-56.7296H311.0912z" p-id="1744"></path><path d="M614.4 198.8608H409.6c-11.264 0-20.48-9.216-20.48-20.48s9.216-20.48 20.48-20.48h204.8c11.264 0 20.48 9.216 20.48 20.48s-9.216 20.48-20.48 20.48zM512 905.4208c-45.2608 0-81.92-36.6592-81.92-81.92s36.6592-81.92 81.92-81.92 81.92 36.6592 81.92 81.92-36.6592 81.92-81.92 81.92z m0-122.88c-22.528 0-40.96 18.432-40.96 40.96s18.432 40.96 40.96 40.96 40.96-18.432 40.96-40.96-18.432-40.96-40.96-40.96z" p-id="1745"></path></svg>
                List<string> values = new List<string>();
                foreach (var item in matchCollection)
                {
                    var str = item.ToString().Replace(" d=\"","").Replace("\"","");
                    values.Add(str);
                }
                textBox2.Text = "<PathGeometry x:Key=\""+ textBox3 .Text+ "\">"+string.Join(" ", values)+ "</PathGeometry>";
            }
            else
            {
                textBox2.Text = "";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(null,null);
        }
    }
}
