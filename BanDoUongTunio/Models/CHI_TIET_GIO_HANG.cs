namespace BanDoUongTunio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CHI_TIET_GIO_HANG
    {
        public int id { get; set; }

        public int? gio_hang_id { get; set; }

        public int? san_pham_id { get; set; }

        public int? size_id { get; set; }

        public int? so_luong { get; set; }

        public virtual GIO_HANG GIO_HANG { get; set; }

        public virtual SAN_PHAM SAN_PHAM { get; set; }

        public virtual SIZE SIZE { get; set; }
    }
}
