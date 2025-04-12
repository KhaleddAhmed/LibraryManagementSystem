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
    public class BookConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(b => b.Title).IsUnique();
            builder.Property(b => b.Title).HasMaxLength(100);
            builder.Property(b => b.Author).HasMaxLength(100);
            builder.Property(b => b.PublishedBy).HasMaxLength(100);
        }
    }
}
