using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubelity.Domain.Entities;

namespace Nubelity.Infrastructure.Persistence.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.BirthDate)
                   .IsRequired();
        }
    }
}
