namespace BanDoUongTunio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CHI_TIET_DON_HANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHI_TIET_DON_HANG()
        {
            CHI_TIET_DON_HANG_TOPPING = new HashSet<CHI_TIET_DON_HANG_TOPPING>();
        }

        public int id { get; set; }

        public int? don_hang_id { get; set; }

        public int? san_pham_id { get; set; }

        public int? size_id { get; set; }

        public int? so_luong { get; set; }

        public decimal? don_gia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHI_TIET_DON_HANG_TOPPING> CHI_TIET_DON_HANG_TOPPING { get; set; }

        public virtual DON_HANG DON_HANG { get; set; }

        public virtual SAN_PHAM SAN_PHAM { get; set; }

        public virtual SIZE SIZE { get; set; }
    }
}
