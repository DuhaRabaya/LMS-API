using LMS.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Security.Claims;

namespace LMS.PL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTranslation> CourseTranslations { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskTranslation> TaskTranslations { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
        { 
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<Course>()
           .Property(c => c.Price)
           .HasPrecision(18, 2);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseModel>();
            if (_httpContextAccessor.HttpContext != null)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(x => x.CreatedBy).CurrentValue = userId;
                        entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property(x => x.UpdatedBy).CurrentValue = userId;
                        entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseModel>();
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedBy).CurrentValue = userId;
                    entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedBy).CurrentValue = userId;
                    entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }
    }
}
