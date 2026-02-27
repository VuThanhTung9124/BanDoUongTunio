namespace BanDoUong_User.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GIO_HANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GIO_HANG()
        {
            CHI_TIET_GIO_HANG = new HashSet<CHI_TIET_GIO_HANG>();
        }

        public int id { get; set; }

        public int? tai_khoan_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_GIO_HANG> CHI_TIET_GIO_HANG { get; set; }

        public virtual TAI_KHOAN TAI_KHOAN { get; set; }
    }
}
