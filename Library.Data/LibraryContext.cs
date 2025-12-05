using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
namespace Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

       
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.BookId);
                entity.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.Description).HasMaxLength(2000);
                entity.Property(b => b.Edition).HasMaxLength(50);
                entity.Property(b => b.Language).HasMaxLength(50);
                entity.Property(b => b.ShelfLocation).HasMaxLength(100);
                entity.Property(b => b.Price).HasPrecision(18, 2);
                entity.Property(b => b.TotalCopies).HasDefaultValue(1);
                entity.Property(b => b.AvailableCopies).HasDefaultValue(1);
                entity.Property(b => b.AddedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(b => b.IsActive).HasDefaultValue(true);

                entity.HasIndex(b => b.ISBN).IsUnique();

                entity.HasOne(b => b.Category)
                    .WithMany(c => c.Books)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(b => b.PublisherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

          
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.AuthorId);
                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Biography).HasMaxLength(2000);
                entity.Property(a => a.Country).HasMaxLength(100);
                entity.Property(a => a.Email).HasMaxLength(100);
                entity.Property(a => a.Website).HasMaxLength(200);
            });

         
            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(ba => new { ba.BookId, ba.AuthorId });

                entity.HasOne(ba => ba.Book)
                    .WithMany(b => b.BookAuthors)
                    .HasForeignKey(ba => ba.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ba => ba.Author)
                    .WithMany(a => a.BookAuthors)
                    .HasForeignKey(ba => ba.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(ba => ba.AuthorOrder).IsRequired();
            });

           
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.MemberId);
                entity.Property(m => m.MembershipNumber).IsRequired().HasMaxLength(20);
                entity.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(m => m.LastName).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Email).HasMaxLength(100);
                entity.Property(m => m.Phone).HasMaxLength(20);
                entity.Property(m => m.Address).HasMaxLength(500);
                entity.Property(m => m.IdCardNumber).HasMaxLength(50);
                entity.Property(m => m.PhotoPath).HasMaxLength(500);
                entity.Property(m => m.JoinDate).HasDefaultValueSql("GETDATE()");
                entity.Property(m => m.IsActive).HasDefaultValue(true);

                entity.HasIndex(m => m.MembershipNumber).IsUnique();

                entity.HasOne(m => m.MembershipType)
                    .WithMany(mt => mt.Members)
                    .HasForeignKey(m => m.MembershipTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

          
            modelBuilder.Entity<BookLoan>(entity =>
            {
                entity.HasKey(bl => bl.BookLoanId);
                entity.Property(bl => bl.Status).IsRequired().HasMaxLength(20);
                entity.Property(bl => bl.Notes).HasMaxLength(1000);
                entity.Property(bl => bl.RenewalCount).HasDefaultValue(0);
                entity.Property(bl => bl.IssueDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(bl => bl.Member)
                    .WithMany(m => m.BookLoans)
                    .HasForeignKey(bl => bl.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bl => bl.Book)
                    .WithMany(b => b.BookLoans)
                    .HasForeignKey(bl => bl.BookId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bl => bl.IssuedByStaff)
                    .WithMany(s => s.IssuedLoans)
                    .HasForeignKey(bl => bl.IssuedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bl => bl.ReturnedByStaff)
                    .WithMany(s => s.ReturnedLoans)
                    .HasForeignKey(bl => bl.ReturnedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

           
            modelBuilder.Entity<Fine>(entity =>
            {
                entity.HasKey(f => f.FineId);
                entity.Property(f => f.Reason).HasMaxLength(100);
                entity.Property(f => f.Status).IsRequired().HasMaxLength(20);
                entity.Property(f => f.PaymentMethod).HasMaxLength(50);
                entity.Property(f => f.TransactionId).HasMaxLength(100);
                entity.Property(f => f.WaiverReason).HasMaxLength(500);
                entity.Property(f => f.Amount).HasPrecision(18, 2);
                entity.Property(f => f.PaidAmount).HasPrecision(18, 2);
                entity.Property(f => f.IssueDate).HasDefaultValueSql("GETDATE()");
                entity.Property(f => f.Status).HasDefaultValue("Pending");

                entity.HasOne(f => f.Member)
                    .WithMany(m => m.Fines)
                    .HasForeignKey(f => f.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.BookLoan)
                    .WithOne(bl => bl.Fine)
                    .HasForeignKey<Fine>(f => f.BookLoanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

         
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(u => u.IsActive).HasDefaultValue(true);

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Staff)
                    .WithMany()
                    .HasForeignKey(u => u.StaffId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Member)
                    .WithMany()
                    .HasForeignKey(u => u.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

         
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(s => s.StaffId);
                entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Email).HasMaxLength(100);
                entity.Property(s => s.Phone).HasMaxLength(20);
                entity.Property(s => s.Position).HasMaxLength(100);
                entity.Property(s => s.Department).HasMaxLength(100);
                entity.Property(s => s.Salary).HasPrecision(18, 2);
                entity.Property(s => s.IsActive).HasDefaultValue(true);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}