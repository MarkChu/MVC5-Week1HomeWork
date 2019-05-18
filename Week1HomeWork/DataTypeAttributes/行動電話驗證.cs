using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Week1HomeWork.DataTypeAttributes
{
    public class 行動電話驗證Attribute : DataTypeAttribute
    {
        public 行動電話驗證Attribute() : base(DataType.Text)
        {
            ErrorMessage = "格式錯誤:必須為 xxxx-xxxxxx !!";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string str = (string)value;

            Regex rgx = new Regex(@"\d{4}-\d{6}$");

            return rgx.IsMatch(str);
        }

    }
}