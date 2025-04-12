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
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(C => C.Id).UseIdentityColumn(100, 100);
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasMany(c => c.Books).WithOne(b => b.Category).HasForeignKey(b => b.CategoryId);
        }
    }
}
