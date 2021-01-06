using Microsoft.Extensions.Configuration;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go2Share.Entity.Entity
{
    public class OTPModel
    {
        public string OTP { get; set; }
        public string MobileNo { get; set; }

        public string GetOTP(IConfiguration _configuration)
        {
            var timeStep = Convert.ToInt32(_configuration["OTP:TimeStep"]);
            var otpSize = Convert.ToInt16(_configuration["OTP:OTPLength"]);
            long longSeconds;
            long.TryParse(_configuration["OTP:Seconds"], out longSeconds);
            Random rnd = new Random();
            var otpNumber = (rnd.Next(1000, 9999)).ToString();

            //var otpNew = KeyGeneration.GenerateRandomKey(otpSize);
            //Totp otpCalc = new Totp(Encoding.UTF8.GetBytes(_configuration["jwt:secretKey"]), timeStep, OtpHashMode.Sha256, otpSize);
            //DateTime time = DateTimeOffset.FromUnixTimeSeconds(longSeconds).DateTime;
            //string otp = otpCalc.ComputeTotp(time);
            return otpNumber;
        }
        public bool IsValidOTP(string sessionOTP, string entityOTP, string sessionMobileNo, string entityMobileNo, IConfiguration _configuration = null)
        {
            var timeStep = Convert.ToInt32(_configuration["OTP:TimeStep"]);
            var otpSize = Convert.ToInt16(_configuration["OTP:OTPLength"]);
            long longSeconds;
            long.TryParse(_configuration["OTP:Seconds"], out longSeconds);

            Totp otpCalc = new Totp(Encoding.UTF8.GetBytes(_configuration["jwt:secretKey"]), timeStep, OtpHashMode.Sha256, otpSize);
            DateTime time = DateTimeOffset.FromUnixTimeSeconds(longSeconds).DateTime;
            //var verifyOTP = otpCalc.VerifyTotp(sessionOTP, out longSeconds);

            if (sessionOTP == entityOTP && sessionMobileNo == entityMobileNo)
            {
                return true;
            }
            else
                return false;
        }
    }
}
