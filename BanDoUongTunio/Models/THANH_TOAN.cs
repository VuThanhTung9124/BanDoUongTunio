namespace BanDoUongTunio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class THANH_TOAN
    {
        public int id { get; set; }

        public int? don_hang_id { get; set; }

        [StringLength(50)]
        public string phuong_thuc { get; set; }

        [StringLength(50)]
        public string trang_thai { get; set; }

        public virtual DON_HANG DON_HANG { get; set; }
    }
}
