namespace BanDoUong_User.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QL_BAN_DO_UONGDb : DbContext
    {
        public QL_BAN_DO_UONGDb()
            : base("name=QL_BAN_DO_UONGDb")
        {
        }

        public virtual DbSet<CHI_TIET_DON_HANG> CHI_TIET_DON_HANG { get; set; }
        public virtual DbSet<CHI_TIET_DON_HANG_TOPPING> CHI_TIET_DON_HANG_TOPPING { get; set; }
        public virtual DbSet<CHI_TIET_GIO_HANG> CHI_TIET_GIO_HANG { get; set; }
        public virtual DbSet<DANH_MUC> DANH_MUC { get; set; }
        public virtual DbSet<DIA_CHI> DIA_CHI { get; set; }
        public virtual DbSet<DON_HANG> DON_HANG { get; set; }
        public virtual DbSet<GIO_HANG> GIO_HANG { get; set; }
        public virtual DbSet<SAN_PHAM> SAN_PHAM { get; set; }
        public virtual DbSet<SAN_PHAM_SIZE> SAN_PHAM_SIZE { get; set; }
        public virtual DbSet<SIZE> SIZEs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAI_KHOAN> TAI_KHOAN { get; set; }
        public virtual DbSet<THANH_TOAN> THANH_TOAN { get; set; }
        public virtual DbSet<TOPPING> TOPPINGs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CHI_TIET_DON_HANG>()
                .Property(e => e.don_gia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CHI_TIET_DON_HANG>()
                .HasMany(e => e.CHI_TIET_DON_HANG_TOPPING)
                .WithOptional(e => e.CHI_TIET_DON_HANG)
                .HasForeignKey(e => e.chi_tiet_don_hang_id);

            modelBuilder.Entity<CHI_TIET_DON_HANG_TOPPING>()
                .Property(e => e.gia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DANH_MUC>()
                .HasMany(e => e.SAN_PHAM)
                .WithOptional(e => e.DANH_MUC)
                .HasForeignKey(e => e.danh_muc_id);

            modelBuilder.Entity<DIA_CHI>()
                .HasMany(e => e.DON_HANG)
                .WithOptional(e => e.DIA_CHI)
                .HasForeignKey(e => e.dia_chi_id);

            modelBuilder.Entity<DON_HANG>()
                .Property(e => e.tong_tien)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DON_HANG>()
                .HasMany(e => e.CHI_TIET_DON_HANG)
                .WithOptional(e => e.DON_HANG)
                .HasForeignKey(e => e.don_hang_id);

            modelBuilder.Entity<DON_HANG>()
                .HasMany(e => e.THANH_TOAN)
                .WithOptional(e => e.DON_HANG)
                .HasForeignKey(e => e.don_hang_id);

            modelBuilder.Entity<GIO_HANG>()
                .HasMany(e => e.CHI_TIET_GIO_HANG)
                .WithOptional(e => e.GIO_HANG)
                .HasForeignKey(e => e.gio_hang_id);

            modelBuilder.Entity<SAN_PHAM>()
                .Property(e => e.gia_co_ban)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SAN_PHAM>()
                .HasMany(e => e.CHI_TIET_DON_HANG)
                .WithOptional(e => e.SAN_PHAM)
                .HasForeignKey(e => e.san_pham_id);

            modelBuilder.Entity<SAN_PHAM>()
                .HasMany(e => e.CHI_TIET_GIO_HANG)
                .WithOptional(e => e.SAN_PHAM)
                .HasForeignKey(e => e.san_pham_id);

            modelBuilder.Entity<SAN_PHAM>()
                .HasMany(e => e.SAN_PHAM_SIZE)
                .WithOptional(e => e.SAN_PHAM)
                .HasForeignKey(e => e.san_pham_id);

            modelBuilder.Entity<SAN_PHAM_SIZE>()
                .Property(e => e.gia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SIZE>()
                .HasMany(e => e.CHI_TIET_DON_HANG)
                .WithOptional(e => e.SIZE)
                .HasForeignKey(e => e.size_id);

            modelBuilder.Entity<SIZE>()
                .HasMany(e => e.CHI_TIET_GIO_HANG)
                .WithOptional(e => e.SIZE)
                .HasForeignKey(e => e.size_id);

            modelBuilder.Entity<SIZE>()
                .HasMany(e => e.SAN_PHAM_SIZE)
                .WithOptional(e => e.SIZE)
                .HasForeignKey(e => e.size_id);

            modelBuilder.Entity<TAI_KHOAN>()
                .HasMany(e => e.DIA_CHI)
                .WithOptional(e => e.TAI_KHOAN)
                .HasForeignKey(e => e.tai_khoan_id);

            modelBuilder.Entity<TAI_KHOAN>()
                .HasMany(e => e.DON_HANG)
                .WithOptional(e => e.TAI_KHOAN)
                .HasForeignKey(e => e.tai_khoan_id);

            modelBuilder.Entity<TAI_KHOAN>()
                .HasMany(e => e.GIO_HANG)
                .WithOptional(e => e.TAI_KHOAN)
                .HasForeignKey(e => e.tai_khoan_id);

            modelBuilder.Entity<TOPPING>()
                .Property(e => e.gia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<TOPPING>()
                .HasMany(e => e.CHI_TIET_DON_HANG_TOPPING)
                .WithOptional(e => e.TOPPING)
                .HasForeignKey(e => e.topping_id);
        }
    }
}
