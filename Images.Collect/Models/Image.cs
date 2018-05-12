using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Images.Collect.Models
{
    public class Image
    {
        [Key]
        public int id { get; set; }
        public string FullName { get; set; }
        public string BaseName { get; set; }
        public string Path { get; set; }
        [MaxLength(10)]
        public string Extension { get; set; }
        [MaxLength(3)]
        public string Drive { get; set; }
        public string Hash { get; set; }
        public bool Unreadable { get; set; }
        public long FileSize { get; set; }
        public DateTime? FileDate { get; set; }
    }


    public class ImageDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ImageDbContext>());
            //base.OnModelCreating(modelBuilder);
        }
    }
}
