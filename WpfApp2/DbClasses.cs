using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    public class Image
    {
        public int ImageId { get; set; }

        public int ImageHash { get; set; }

        public ImageBlob BLOB { get; set; }

        public List<Box> boxes { get; set; } = new List<Box>();

    }

    public class ImageBlob
    {
        public int ImageBlobId { get; set; }
        public byte[] Img { get; set; }
    }

    public class Box
    {
        public int BoxId { get; set; }

        public string Label { get; set; }
        public float x1 { get; set; }
        public float x2 { get; set; }
        public float x3 { get; set; }

        public float x4 { get; set; }
        public float Confidence { get; set; }

        public int ImageId { get; set; }

        public Image Image { get; set; }
    }

    public class ImageDB : DbContext
    {
        public DbSet<Image> images { get; set; }

        public DbSet<Box> boxes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder o) => o.UseSqlite("Data Source=images.db");
    }
}
