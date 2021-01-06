using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Go2Share.General
{
    public static class Utility
    {
        #region String Extension
        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>        
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (string.Compare(value, otherValue) == 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        /// <usage> 
        ///  var str = "This ia a logical sentence";
        ///  var op = str.Right(3,1)
        /// </usage>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }
        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        /// <usage> 
        ///  var str = "This ia a logical sentence";
        ///  var op = str.Left(3,1)
        /// </usage>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }
        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="arg0">An System.Object to format</param>
        /// <returns>A copy of format in which the first format item has been replaced by the
        /// System.String equivalent of arg0</returns>
        /// <usage> 
        /// 
        /// 
        /// </usage>
        public static string FormatString(this string value, object arg0)
        {
            return string.Format(value, arg0);
        }
        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the System.String
        /// equivalent of the corresponding instances of System.Object in args.</returns>
        /// <usage>  
        ///  var formatted = "{0} User.".FormatString("Hello");
        /// </usage>
        public static string FormatString(this string value, params object[] args)
        {
            return string.Format(value, args);
        }
        /// <summary>
        /// Converts a string to upper-case but checks for null strings first
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToUpperCheckForNull(this string input)
        {

            string retval = input;

            if (!String.IsNullOrEmpty(retval))
            {
                retval = retval.ToUpper();
            }

            return retval;

        }
        /// <summary>
        /// Perform a Trim() when the string is not null. If the string is null the method will return null.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string IsNotNullThenTrim(this string s)
        {
            if (!string.IsNullOrEmpty(s))
                return s.Trim();
            else
                return s;
        }

        #endregion

        #region Datetime

        /// <summary>
        /// Returns whether or not a DateTime is during a leap year.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLeapYear(this DateTime value)
        {
            return (System.DateTime.DaysInMonth(value.Year, 2) == 29);
        }
        /// <summary>
        /// A simple date range
        /// </summary>
        /// <param name="self"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetDateRangeTo(this DateTime self, DateTime toDate)
        {
            var range = Enumerable.Range(0, new TimeSpan(toDate.Ticks - self.Ticks).Days);

            return from p in range
                   select self.Date.AddDays(p);
        }
        #endregion

        #region Misc
        /// <summary>
        /// converts one type to another
        /// Example:
        /// var age = "28";
        /// var intAge = age.To<int>();
        /// var doubleAge = intAge.To<double>();
        /// var decimalAge = doubleAge.To<decimal>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T To<T>(this IConvertible value)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, t);
                }
            }

            catch
            {
                return default(T);
            }
        }
        public static T To<T>(this IConvertible value, IConvertible ifError)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return (T)ifError;

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                        return (T)(ifError.To<T>());

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {

                return (T)ifError;
            }

        }
        /// <summary>
        /// //Input: transpose [[1,2,3],[4,5,6],[7,8,9]]
        /// //Output: [[1,4,7],[2,5,8],[3,6,9]]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> values)
        {
            if (values.Count() == 0)
                return values;
            if (values.First().Count() == 0)
                return Transpose(values.Skip(1));

            var x = values.First().First();
            var xs = values.First().Skip(1);
            var xss = values.Skip(1);
            return
             new[] {new[] {x}
           .Concat(xss.Select(ht => ht.First()))}
               .Concat(new[] { xs }
               .Concat(xss.Select(ht => ht.Skip(1)))
               .Transpose());
        }
        /// <summary>
        /// Recursively create directory
        /// </summary>
        /// string path = @"C:\temp\one\two\three";
        /// var dir = new DirectoryInfo(path);
        /// dir.CreateDirectory();
        /// <param name="dirInfo">Folder path to create.</param>
        public static void CreateDirectory(this DirectoryInfo dirInfo)
        {
            if (dirInfo.Parent != null) CreateDirectory(dirInfo.Parent);
            if (!dirInfo.Exists) dirInfo.Create();
        }


        #endregion

        #region Validate methods
        /// <summary>
        /// Validate URL
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string text)
        {
            Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }
        /// <summary>
        /// Validate number
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string theValue)
        {
            long retNum;
            return long.TryParse(theValue, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }
        /// <summary>
        /// check if Null
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNull(this object source)
        {
            return source == null;
        }
        /// <summary>
        /// Check if datetime is valld or not
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string input)
        {
            return !string.IsNullOrEmpty(input) && DateTime.TryParse(input, out _);
        }
        /// <summary>
        /// validate Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Check if bool
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBoolean(this Type type)
        {
            return type.Equals(typeof(Boolean));
        }
        #endregion

    }
}
