using Go2Share.Entity.Entity;
using System.Collections.Generic;

namespace Go2Share.Entity.ViewModel
{
    public  class TUVExteriorChecklistModel
    {
        public List<ExteriorCategoryMasterEntity> ExteriorCategoryMaster { get; set; }
        public List<ExteriorSubCategoryMasterEntity> ExteriorSubCategoryMaster { get; set; }
        public List<DamagesMasterEntity> DamageMaster { get; set; }
        public List<TuvexteriorCheckListEntity> TuvexteriorCheckList { get; set; }
    }
}
