namespace BanDoUong_User.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DON_HANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DON_HANG()
        {
            CHI_TIET_DON_HANG = new HashSet<CHI_TIET_DON_HANG>();
            THANH_TOAN = new HashSet<THANH_TOAN>();
        }

        public int id { get; set; }

        public int? tai_khoan_id { get; set; }

        public int? dia_chi_id { get; set; }

        public decimal? tong_tien { get; set; }

        [StringLength(255)]
        public string ghi_chu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_DON_HANG> CHI_TIET_DON_HANG { get; set; }

        public virtual DIA_CHI DIA_CHI { get; set; }

        public virtual TAI_KHOAN TAI_KHOAN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THANH_TOAN> THANH_TOAN { get; set; }
    }
}
