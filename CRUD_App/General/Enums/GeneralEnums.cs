using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace CRUD_App.General.Enums
{
    public class GeneralEnums
    {
        public enum NotificationTypeConstants
        {
            Error = 0,
            Success = 1,
            Warning = 2
        }

        public enum RoleType
        {
            Admin = 1,
            TUOwner = 2,
            Customer = 3,
            Insurance = 4
        }

        public enum DocumentType
        {
            TUVPhotos = 1,
            TUVDocument = 2
        }
        public enum TUVBookingList
        {
            TUVBookingHisotry = 1,
            TUVBookingOnGoing = 2,
            TUVBookingUpComing = 3
        }
        public enum TUVBookingOwner
        {
            TUVBookingRecieved = 1,
            TUVBookingConfirm = 2,
            TUVBookingHistory = 3
        }

        public enum ApplicationText
        {
            CancellationPolicy = 1,
            AgreementText = 2,
            ParkingDeliveryDetails = 3,
            Guidelines = 4
        }

        public enum ClaimStatus
        {
            Pending = 0,
            Approved = 1,
            Rejected = 2
        }
        public enum UnlockingSystem
        {
            [Display(Name = "Automatic (with mobile application)")]
            Automatic = 1,
            Physical = 2
        }
        public enum Driver
        {
            [Display(Name = "With Driver")]
            WithDriver = 1,
            [Display(Name = "Without Driver")]
            WithoutDriver = 2,
            Any = 3
        }
        public static string DisplayAttributeName(Type enumType, int value)
        {
            string enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];
            string outString = string.Empty;
            object[] attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Length > 0)
            {
                outString = ((DisplayAttribute)attrs[0]).Name;

                if (((DisplayAttribute)attrs[0]).ResourceType != null)
                {
                    outString = ((DisplayAttribute)attrs[0]).GetName();
                }
            }
            else
            {
                outString = Enum.GetName(enumType, value).ToString();
            }
            return outString;
        }
    }
}
