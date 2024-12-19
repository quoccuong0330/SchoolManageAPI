using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Data;

public class ApplicationDbContext : DbContext{
  
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<TablePoint> Tables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(u => u.ClassId)
            .OnDelete(DeleteBehavior.SetNull); // Cho phép SET NULL khi Class bị xóa

        // Quan hệ Class -> Lead (User)
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Lead)
            .WithMany()
            .HasForeignKey(c => c.LeadId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasOne(u => u.TablePoint)
            .WithOne(tp => tp.Student)
            .HasForeignKey<TablePoint>(tp => tp.StudentId);

        modelBuilder.Entity<TablePoint>()
            .HasOne(tp => tp.Student)
            .WithOne(u => u.TablePoint)
            .HasForeignKey<User>(u => u.TableId);

        modelBuilder.Entity<TablePoint>()
            .HasOne(t => t.Editor)
            .WithMany()
            .HasForeignKey(t => t.EditorId)
            .OnDelete(DeleteBehavior.SetNull);
      
    }
}