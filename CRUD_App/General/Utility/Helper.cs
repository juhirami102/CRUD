using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Go2Share.General.Resources.Common
{
    public class Helper
    {

        public static string NoRecordFound = "No Records Found.";
        public static string OperationSuccess = "Operation success";
        public static string ErrorCodeRecevied = "Error Code Recevied.";
        public static string ValidationFailed = "Validation Failed.";
        public static string ClientIPAddress = "";
        public static HttpRequest httpRequest = null;
        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        public static long GetFileSizeInMB(string FilePath)
        {
            FileInfo info = new FileInfo(FilePath); // get latest file information.
            long length = info.Length; // get latest file size in byte.
            long kb = length / 1024; // convert file size from byte to kb.
            long mb = kb / 1024; // convert file size from kb to mb.
            return mb;
        }
        /// <summary>
        /// General function to Get Random Alpha Numeric 12 Digit Number
        /// </summary>
        /// <returns></returns>
        public static string GetBookingNo()
        {
            //return "YJ60OBISNP92";
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + numbers;

            int length = 12;
            string bookingNo = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (bookingNo.IndexOf(character) != -1);
                bookingNo += character;
            }
            return bookingNo;
        }
        public static string GetReferralCode()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + numbers;

            int length = 8;
            string bookingNo = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (bookingNo.IndexOf(character) != -1);
                bookingNo += character;
            }
            return bookingNo;
        }
        public static string GetZipCodeByLatLon(string lat, string lng)
        {
            string postalCode = "";
            try
            {
                var xml = new XmlDocument();
                xml.Load("http://maps.googleapis.com/maps/api/geocode/xml?latlng=" + lat + "," + lng + "&sensor=true");
                var components = xml["GeocodeResponse"]["result"].ChildNodes;
                foreach (XmlElement x in components)
                {
                    if (x.Name == "address_component" && x["type"].InnerText == "postal_code")
                    {
                        postalCode = x["long_name"].InnerText;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return postalCode;
        }
        public static string GetIPAddress(HttpRequest request)
        {
            string IPAddress = string.Empty;
            if (request != null)
            {
                IPAddress = request.HttpContext.Connection.RemoteIpAddress != null ? request.HttpContext.Connection.RemoteIpAddress.ToString() : "0.0.0.0";
                httpRequest = request;
            }
            if (IPAddress == "::1" || string.IsNullOrEmpty(IPAddress))
            {
                IPHostEntry Host = default(IPHostEntry);
                string Hostname = null;
                Hostname = System.Environment.MachineName;
                Host = Dns.GetHostEntry(Hostname);
                foreach (IPAddress IP in Host.AddressList)
                {
                    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        IPAddress = Convert.ToString(IP);
                    }
                }
            }
            return IPAddress;

        }
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
