using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBusinessOnline.Tools
{
    public static class Extends
    {
        public static string GetCamelCase(this string str, bool firstUpper = false, char char1 = '_')
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            StringBuilder stringBuilder = new StringBuilder();
            bool lastIsSymbol = false;
            for (int i = 0; i < str.Length; i++)
            {
                char s = str[i];
                bool isSymbol = s == char1;
                //如果是特定符号 则不增加内容
                //如果上次是特定符号，并且本次不是
                //如果都不是特定符号，则显示原符号
                if (isSymbol)
                {
                    goto result;
                }
                if (lastIsSymbol || (i == 0 && stringBuilder.Length == 0 && firstUpper))
                {
                    stringBuilder.Append(s.ToString().ToUpper());
                    goto result;
                }
                stringBuilder.Append(s);
            result: lastIsSymbol = isSymbol;
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 判断字符是否为大写字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>       
        private static bool IsUpper(char c)
        {
            if (c >= 'A' && c <= 'Z')   {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string ReCamelCase(this string str, char char1 = '_')
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char s = str[i];
                bool isUpper = IsUpper(s);
                //如果是大写则追加特定符号+小写字母
                if (isUpper)
                {
                    stringBuilder.Append(char1 + s.ToString().ToLower());
                }
                else
                {
                    stringBuilder.Append(s);
                }
                //if (lastIsSymbol || (i == 0 && stringBuilder.Length == 0 && firstUpper))
                //{
                //    stringBuilder.Append(s.ToString().ToUpper());
                //    goto result;
                //}
                
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string ToFirstLetterLower(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (str.Length == 1)
            {
                return str.ToLower();
            }

            var length = -1;
            for (int i = 0; i < str.Length; i++)
            {
                if (IsUpper(str[i]))
                {
                    length = i;
                }
                else
                {
                    break;
                }
            }
            if (length == -1)
            {
                return str;
            }
            else
            {
                return str.Substring(0, length+1).ToLower()+str.Substring(length+1);
            }
        }
    }
}
