using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;
using System.Text.RegularExpressions;

namespace ykmWeb.common
{
   public  class hz_to_py
    {
        /// <summary> 
        /// 汉字转化为拼音
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>全拼</returns> 
        public static string GetPinyin(string str)
        {
            Regex reg = new Regex(@"^[\u4e00-\u9fa5_a-zA-Z0-9]+$");
            str = reg.Replace(str,"");
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary> 
        /// 汉字转化为拼音首字母
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>首字母</returns> 
        public static string GetFirstPinyin(string str)
        {

            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }

            if (string.IsNullOrEmpty(r) == false)
            {
                Regex re = new Regex(@"[a-zA-Z0-9_]");
                MatchCollection m = re.Matches(r);
                string _s = "";
                for (int j = 0; j < m.Count; j++)
                {
                    _s += m[j].ToString();
                }
                r = _s;
            }
            return r;
        }

    }
}
