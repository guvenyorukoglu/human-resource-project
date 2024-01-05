using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class PossessionConfig : IEntityTypeConfiguration<Possession>
    {
        public void Configure(EntityTypeBuilder<Possession> builder)
        {
            builder.Property(x => x.Barcode).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Brand).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PossessionModel).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Details).HasMaxLength(250);
            builder.Property(x => x.PossessionType).IsRequired();
            builder.Property(x => x.CompanyId).IsRequired();

            builder.HasOne(x => x.Company).WithMany(x => x.Possessions).HasForeignKey(x => x.CompanyId);
        }
    }

}
