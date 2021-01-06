using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Go2Share.General
{
    public static class ExpressionExtension
    {
        public static Int32 ToInt32IC(this object val)
        {
            int _val = 0;
            if (val == null)
                return 0;
            else
                Int32.TryParse(val.ToString(), out _val);

            return _val;
        }
        public static Int16 ToInt16IC(this object val)
        {
            Int16 _val = 0;
            if (val == null)
                return 0;
            else
                Int16.TryParse(val.ToString(), out _val);

            return _val;
        }
        public static string ToStingIC(this object val)
        {
            if (val == null)
                return string.Empty;
            else
                return val.ToString();
        }
        public static DateTime ToDateTimeIC(this object val)
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                if (val == null)
                    return DateTime.Now;
                else
                {
                    dateTime = DateTime.ParseExact(val.ToString().Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            { }
            return dateTime;
        }
        public static DateTime ToJoinDateTimeIC(this object val, string time)
        {
            DateTime dateTime = DateTime.Now;
            try
            {

                if (val == null || string.IsNullOrEmpty(val.ToString()))
                    return DateTime.Now;
                else
                {
                    dateTime = string.IsNullOrEmpty(time) ? DateTime.ParseExact(val.ToString().Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        : DateTime.ParseExact(val.ToString().Replace("-", "/") + " " + time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            { }
            return dateTime;
        }
    }
}
