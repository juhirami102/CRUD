using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_App.Entity.ViewModel
{
    public abstract class TUVGeneral
    {
        public int TuvId { get; set; }
        public decimal StartLatitude { get; set; }
        public decimal EndLongitude { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class TUVListFilter : TUVGeneral
    {
        public int FrequencyId { get; set; }
        public int PromoCodeId { get; set; }
        public int CategoryId { get; set; }
        //public List<int> CategoryIds { get; set; }
    }
    public class TUVBooking : TUVGeneral
    {
        public string StartLoc { get; set; }
        public string DropLoc { get; set; }
        // public decimal StartLatitude { get; set; }
        public decimal EndLatitude { get; set; }
        public decimal StartLongitude { get; set; }
        //public decimal EndLongitude { get; set; }
        public string StartZipCode { get; set; }
        public string StartAddress { get; set; }
        public string EndZipCode { get; set; }
        public string EndAddress { get; set; }

        public int? LicenseId { get; set; }
        public int? TUVFareId { get; set; }
        public string IpAddress { get; set; }
        //public bool IsConfirm { get; set; }
        //public bool IsAttentionAgree { get; set; }
        //public DateTime? ConfirmRejectDateTime { get; set; }
        //public DateTime? AttentionDateTime { get; set; }
    }
}
