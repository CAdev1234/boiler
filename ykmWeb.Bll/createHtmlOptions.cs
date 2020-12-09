using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Bll
{
    public class createHtmlOptions
    {
        public static string getOptions(List<ykmWeb.Models.selectOpitonModel> l,string selectValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<option value=\"\">请选择</option>");
            if(l.Count() > 0)
            {
                foreach (var item in l)
                {
                    var selected = "";
                    if (item.Value == selectValue)
                    {
                        selected = "selected";
                    }
                    sb.Append("<option value=\""+item.Value+"\" "+ selected + " >"+item.Text+"</option>");
                }
            }
            return sb.ToString();
        }
        public static string getCheckBox(string checkName, List<ykmWeb.Models.selectOpitonModel> l, string checkValue)
        {
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                sb.Append("<input type=\"hidden\" id=\"" + checkName + "\" name=\"" + checkName + "\" value=\""+ checkValue + "\" />");
                var _checkValue = "," + checkValue + ",";
                foreach (var item in l)
                {
                    var selected = "";
                    if (_checkValue.IndexOf(item.Value) != -1)
                    {
                        selected = "checked";
                    }
                    sb.Append("<label class=\"label\"><input class=\"check\" id=\"" + checkName+"_"+item.Value+"\" name=\"_"+ checkName + "\" type=\"checkbox\" value=\""+ item.Value + "\" " + selected + "> "+ item.Text + "</label>");
                }
                sb.Append("<script type=\"text/javascript\">$(\"input[name='_" + checkName + "']\").click(function(){var str=$(\"#" + checkName + "\").val();var _val=$(this).val();var _chk=$(this).is(':checked');var nums=[];if(str!=\"\"){if(str.indexOf(\",\")!=-1){nums=str.split(\",\")}else{nums.push(str)}}if(_chk==true){var index=nums.indexOf(_val);if(index==-1){nums.push(_val)}}else{var index=nums.indexOf(_val);if(index>-1){nums.splice(index,1)}}nums=nums.sort();str=\"\";for(var i=0;i<nums.length;i++){if(i!=0){str+=\",\"}str+=nums[i]}$(\"#" + checkName + "\").val(str)})</script>");
            }
            return sb.ToString();
        }
        public static string getRadioBtn(string checkName, List<ykmWeb.Models.selectOpitonModel> l, string checkValue)
        {
            //<label class="label"><input class="radio" id="is_train_0" name="is_train" value="0" type="radio" />否</label>
            StringBuilder sb = new StringBuilder();
            if (l.Count() > 0)
            {
                var _checkValue = "," + checkValue + ",";
                foreach (var item in l)
                {
                    var selected = "";
                    if (_checkValue.IndexOf(item.Value) != -1)
                    {
                        selected = "checked";
                    }
                    sb.Append("<label class=\"label\"><input class=\"radio\" id=\"" + checkName + "_" + item.Value + "\" name=\"" + checkName + "\" type=\"radio\" value=\"" + item.Value + "\" " + selected + "> " + item.Text + "</label>");
                }
            }
            return sb.ToString();
        }
    }
}
