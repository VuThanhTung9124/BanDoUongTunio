namespace BanDoUong_User.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SAN_PHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SAN_PHAM()
        {
            CHI_TIET_DON_HANG = new HashSet<CHI_TIET_DON_HANG>();
            CHI_TIET_GIO_HANG = new HashSet<CHI_TIET_GIO_HANG>();
            SAN_PHAM_SIZE = new HashSet<SAN_PHAM_SIZE>();
        }

        public int id { get; set; }

        public int? danh_muc_id { get; set; }

        [StringLength(100)]
        public string ten_san_pham { get; set; }

        [StringLength(255)]
        public string mo_ta { get; set; }

        [StringLength(255)]
        public string hinh_anh { get; set; }

        public decimal? gia_co_ban { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_DON_HANG> CHI_TIET_DON_HANG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_GIO_HANG> CHI_TIET_GIO_HANG { get; set; }

        public virtual DANH_MUC DANH_MUC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SAN_PHAM_SIZE> SAN_PHAM_SIZE { get; set; }
    }
}
