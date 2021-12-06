using Newtonsoft.Json;
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
    public partial class JsonToCsharp : BaseForm
    {
        public JsonToCsharp()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string content = textBox1.Text;
            if (string.IsNullOrEmpty(content)) return;
            try
            {
                var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                string str = string.Join("\r\n", obj.Select(it => $@"
        /// <summary>
        /// {it.Value}
        /// </summary>
        public {GetType(it.Value)} {it.Key.GetCamelCase(true)}{{get;set;}}"));
                label1.Text = str;
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label1.Text);
        }

        public string GetType(object value)
        {
            Type type = value?.GetType();
            if (type == typeof(long))
            {
                return "long";
            }
            else if (type == typeof(string))
            {
                return "string";
            }
            else if (type == typeof(int))
            {
                return "int";
            }
            return "object";
        }
    }
}
