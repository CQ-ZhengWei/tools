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
	public partial class Lines : Form
	{
		public Lines()
		{
			InitializeComponent();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			string str = textBox1.Text;
			if(!string.IsNullOrEmpty(str))
			{
				string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(it=> it.Trim()).ToArray();
				var list = lines.Where(it =>new Regex("^[a-zA-Z0-9]+$").IsMatch(it) ).ToList();
				textBox2.Text = string.Join("\r\n,",list.Select(it => $"'{it}'"));
			}
		}
	}
}
