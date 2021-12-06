using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EasyBusinessOnline.Common
{
    public class Utils
    {
        public static string MapPath(string file)
        {
            string path;
            if (HttpContext.Current != null)
            {
                path = HttpContext.Current.Server.MapPath(file);
            }
            else
            {
                path = Environment.CurrentDirectory + file;
            }
            return path;
        }
    }
}
