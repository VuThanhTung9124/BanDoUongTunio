namespace BanDoUongTunio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DIA_CHI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DIA_CHI()
        {
            DON_HANG = new HashSet<DON_HANG>();
        }

        public int id { get; set; }

        public int? tai_khoan_id { get; set; }

        [StringLength(100)]
        public string nguoi_nhan { get; set; }

        [StringLength(20)]
        public string so_dien_thoai { get; set; }

        [StringLength(255)]
        public string dia_chi_cu_the { get; set; }

        [StringLength(255)]
        public string ghi_chu { get; set; }

        public virtual TAI_KHOAN TAI_KHOAN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DON_HANG> DON_HANG { get; set; }
    }
}
