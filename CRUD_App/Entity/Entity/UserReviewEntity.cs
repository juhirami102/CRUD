using System;
using System.Collections.Generic;

namespace Go2Share.Entity.Entity
{
    public partial class UserReviewEntity
    {
        public long UserReviewId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public decimal Rating { get; set; }
        public string ReviewComment { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Ipaddress { get; set; }

        public UserEntity FromUser { get; set; }
        public UserEntity ToUser { get; set; }
    }
    public class CustomerToOwnerReview
    {
        public int TUVId { get; set; }

        public decimal Rating { get; set; }

        public string ReviewComment { get; set; }
    }
    public class OwnerToCustomerReview
    {
        public int CustomerId { get; set; }

        public decimal Rating { get; set; }

        public string ReviewComment { get; set; }
    }
}
