using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Repository.Data.Configurations
{
    public class UserBorrowingConfigurations : IEntityTypeConfiguration<UserBorrowing>
    {
        public void Configure(EntityTypeBuilder<UserBorrowing> builder)
        {
            builder.HasKey(UB => new
            {
                UB.BookId,
                UB.AppUserId,
                UB.RequestForBorrowDate,
            });
            builder
                .HasOne(ub => ub.Book)
                .WithMany(b => b.UserBorrowings)
                .HasForeignKey(ub => ub.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(UB => UB.AppUser)
                .WithMany(U => U.UserBorrowings)
                .HasForeignKey(UB => UB.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
