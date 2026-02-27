namespace BanDoUongTunio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TOPPING")]
    public partial class TOPPING
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TOPPING()
        {
            CHI_TIET_DON_HANG_TOPPING = new HashSet<CHI_TIET_DON_HANG_TOPPING>();
        }

        public int id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        public string ten_topping { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        public decimal? gia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_DON_HANG_TOPPING> CHI_TIET_DON_HANG_TOPPING { get; set; }
    }
}
