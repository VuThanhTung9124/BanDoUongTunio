namespace BanDoUongTunio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CHI_TIET_DON_HANG_TOPPING
    {
        public int id { get; set; }

        public int? chi_tiet_don_hang_id { get; set; }

        public int? topping_id { get; set; }

        public int? so_luong { get; set; }

        public decimal? gia { get; set; }

        public virtual CHI_TIET_DON_HANG CHI_TIET_DON_HANG { get; set; }

        public virtual TOPPING TOPPING { get; set; }
    }
}
