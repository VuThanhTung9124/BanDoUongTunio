namespace BanDoUongTunio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TAI_KHOAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAI_KHOAN()
        {
            DIA_CHI = new HashSet<DIA_CHI>();
            DON_HANG = new HashSet<DON_HANG>();
            GIO_HANG = new HashSet<GIO_HANG>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string ten_dang_nhap { get; set; }

        [StringLength(255)]
        public string mat_khau { get; set; }

        [StringLength(100)]
        public string ho_ten { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(20)]
        public string so_dien_thoai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIA_CHI> DIA_CHI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DON_HANG> DON_HANG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GIO_HANG> GIO_HANG { get; set; }
    }
}
