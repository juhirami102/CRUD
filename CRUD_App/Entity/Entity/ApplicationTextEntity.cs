using System.ComponentModel.DataAnnotations;

namespace Go2Share.Entity.Entity
{
    public class ApplicationTextEntity
    {
        [Display(Name = "Application Text Id")]
        public int ApplicationTextId { get; set; }
        [Display(Name = "Text Title")]
        public string TextTitle { get; set; }
        [Display(Name = "Application Text")]
        public string ApplicationText1 { get; set; }
    }
}
