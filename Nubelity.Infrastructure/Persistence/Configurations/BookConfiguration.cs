using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubelity.Domain.Entities;

namespace Nubelity.Infrastructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.Isbn)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.PageNumber)
                   .IsRequired();

            builder.Property(b => b.CoverUrl)
                   .HasMaxLength(500);

            builder.HasOne(b => b.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
