namespace FoodManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.ComponentModel;
    
    public partial class FOOD_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FOOD_TYPE()
        {
            this.ADDTOCARTs = new HashSet<ADDTOCART>();
        }

        public int TYPEID { get; set; }
        public Nullable<int> FOODID { get; set; }
        public string NAME { get; set; }
        public int PRICE { get; set; }
        public int QUANTITY { get; set; }
        [DisplayName("Upload FIle")]
        public string IMGPATH { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADDTOCART> ADDTOCARTs { get; set; }
        public virtual FOOD_ITEM FOOD_ITEM { get; set; }
    }
}