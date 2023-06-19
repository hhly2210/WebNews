using Microsoft.EntityFrameworkCore;
using WebNews.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace WebNews.Models {
    public partial class DataContext : DbContext {
        public DataContext() {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }

        public virtual DbSet<Account> Accounts { set; get; }
        public virtual DbSet<AdminMenu> AdminMenus { set; get; }
        public virtual DbSet<Comment> Comments { set; get; }
        public virtual DbSet<Commercial> Commercials { set; get; }
        public virtual DbSet<Contact> Contacts { set; get; }
        public virtual DbSet<Menu> Menus { set; get; }
        public virtual DbSet<Post> Posts { set; get; }
        public virtual DbSet<Recommend> Recommends { set; get; }
        public virtual DbSet<Role> Roles { set; get; }
        public virtual DbSet<View_PostMenu> PostMenus { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if(!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=TinTuc;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountID).HasColumnName("AccountID");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Message).HasColumnType("ntext");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.RoleID).HasColumnName("RoleID");

                entity.Property(e => e.Salt).HasMaxLength(50);

                entity.Property(e => e.Story).HasColumnType("ntext");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleID)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<AdminMenu>(entity =>
            {
                entity.ToTable("AdminMenu");

                entity.Property(e => e.AdminMenuID).HasColumnName("AdminMenuID");

                entity.Property(e => e.ActionName).HasMaxLength(20);

                entity.Property(e => e.AreaName).HasMaxLength(20);

                entity.Property(e => e.ControllerName).HasMaxLength(20);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.IdName).HasMaxLength(50);

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.ItemTarget).HasMaxLength(20);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentID).HasColumnName("CommentID");

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.PostID).HasColumnName("PostID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostID)
                    .HasConstraintName("FK_Comment_Post");
            });

            modelBuilder.Entity<Commercial>(entity =>
            {
                entity.ToTable("Commercial");

                entity.Property(e => e.CommercialID).HasColumnName("CommercialID");

                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.Images).HasMaxLength(255);

                entity.Property(e => e.Link).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.ContactID).HasColumnName("ContactID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Message).HasColumnType("ntext");

                entity.Property(e => e.Subject).HasMaxLength(255);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuID).HasColumnName("MenuID");

                entity.Property(e => e.ActionName).HasMaxLength(50);

                entity.Property(e => e.ControllerName).HasMaxLength(50);

                entity.Property(e => e.Link).HasMaxLength(50);

                entity.Property(e => e.MenuName).HasMaxLength(50);

                entity.Property(e => e.ParentID).HasColumnName("ParentID");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.PostID).HasColumnName("PostID");

                entity.Property(e => e.Abstract).HasMaxLength(255);

                entity.Property(e => e.AccountID).HasColumnName("AccountID");

                entity.Property(e => e.Author).HasMaxLength(30);

                entity.Property(e => e.Contents).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Link).HasMaxLength(200);

                entity.Property(e => e.MenuID).HasColumnName("MenuID");

                entity.Property(e => e.SContents)
                    .HasMaxLength(255)
                    .HasColumnName("SContents");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AccountID)
                    .HasConstraintName("FK_Post_Account");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.MenuID)
                    .HasConstraintName("FK_Post_Menu");
            });

            modelBuilder.Entity<Recommend>(entity =>
            {
                entity.ToTable("Recommend");

                entity.Property(e => e.RecommendID).HasColumnName("RecommendID");

                entity.Property(e => e.Link).HasMaxLength(50);

                entity.Property(e => e.RecommendName).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleID).HasColumnName("RoleID");

                entity.Property(e => e.RoleDescription).HasMaxLength(255);

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<View_PostAccount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_PostAccount");

                entity.Property(e => e.Abstract).HasMaxLength(255);

                entity.Property(e => e.AccountID).HasColumnName("AccountID");

                entity.Property(e => e.Author).HasMaxLength(30);

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.Contents).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Link).HasMaxLength(200);

                entity.Property(e => e.MenuID).HasColumnName("MenuID");

                entity.Property(e => e.PostID).HasColumnName("PostID");

                entity.Property(e => e.SContents)
                    .HasMaxLength(255)
                    .HasColumnName("SContents");

                entity.Property(e => e.Title).HasMaxLength(255);
            });

            modelBuilder.Entity<View_PostMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_PostMenu");

                entity.Property(e => e.Abstract).HasMaxLength(255);

                entity.Property(e => e.Author).HasMaxLength(30);

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.Contents).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Link).HasMaxLength(200);

                entity.Property(e => e.MenuID).HasColumnName("MenuID");

                entity.Property(e => e.MenuName).HasMaxLength(50);

                entity.Property(e => e.ParentID).HasColumnName("ParentID");

                entity.Property(e => e.PostID).HasColumnName("PostID");

                entity.Property(e => e.Title).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}