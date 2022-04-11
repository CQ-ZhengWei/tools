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
    public partial class DatabaseAddField : Form
    {
        public DatabaseAddField()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Apply();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Apply();
        }
        private void Apply()
        {
            textBox2.Text = string.Empty;
            string data = textBox1.Text;
            string tableName = textBox3.Text;
            if (string.IsNullOrEmpty(data))
            {
                return;
            }
            var lines = data
                .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(it => it.IndexOf("||") != -1);
            if (lines.Count() == 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append($@"{data}
");
            foreach (var line in lines)
            {
                try
                {
                    var strs = line.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    string fieldInfo = strs[0];
                    string fieldName = fieldInfo.Substring(0, fieldInfo.IndexOf(" "));
                    string fieldDesc = strs[1];
                    string fieldData = $@"IF NOT EXISTS(SELECT * FROM [SYSCOLUMNS] WHERE [ID]=OBJECT_ID('{tableName}') AND [NAME]='{fieldName}')
    BEGIN
		--增加{fieldDesc}
		ALTER TABLE {tableName} ADD {fieldInfo}
		--增加{fieldDesc}注释
		EXEC sys.sp_addextendedproperty @name=N'MS_Description'
		, @value=N'{fieldDesc}' 
		, @level0type=N'SCHEMA'
		,@level0name=N'dbo'
		, @level1type=N'TABLE'
		,@level1name=N'{tableName}'
		, @level2type=N'COLUMN'
		,@level2name=N'{fieldName}'

		--CREATE NONCLUSTERED INDEX {tableName}_{fieldName} on [{tableName}]([{fieldName}]);
		print '成功增加{fieldDesc}字段'
	END
";
                    sb.Append(fieldData);
                }
                catch (Exception)
                {
                }
            }
            textBox2.Text = sb.ToString();
        }
    }
}
