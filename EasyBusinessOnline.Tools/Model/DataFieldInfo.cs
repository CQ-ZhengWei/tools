using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBusinessOnline.Tools.Model
{
    public class DataFieldInfo
    {
        public string desc { get; set; }
        public string table { get; set; }
        public List<DataField> dataFields { get; set; }
    }
}
