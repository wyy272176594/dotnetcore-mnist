using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MnistWeb.DBModels
{
    public partial class mnistContext : DbContext
    {
        public virtual DbSet<Net> Net { get; set; }

        public mnistContext(DbContextOptions options) : base(options) { } 

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySql("Server=wangyueyang.mysql.rds.aliyuncs.com;User Id=wang;Password=Ws02055144;Database=mnist");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Net>(entity =>
            {
                entity.ToTable("net");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10) unsigned")
                    .ValueGeneratedNever();

                entity.Property(e => e.NetText).HasColumnType("mediumtext");
            });
        }
    }
}
