namespace BanDoUong_User.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SAN_PHAM_SIZE
    {
        public int id { get; set; }

        public int? san_pham_id { get; set; }

        public int? size_id { get; set; }

        public decimal? gia { get; set; }

        public virtual SAN_PHAM SAN_PHAM { get; set; }

        public virtual SIZE SIZE { get; set; }
    }
}
