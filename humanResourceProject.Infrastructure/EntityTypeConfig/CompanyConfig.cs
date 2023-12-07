using humanResourceProject.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class CompanyConfig : BaseEntityConfig<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(c => c.CompanyName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.NumberOfEmployees).IsRequired();
            builder.Property(c => c.TaxNumber).IsRequired().HasMaxLength(10);
            builder.Property(c => c.TaxOffice).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Address).IsRequired().HasMaxLength(200);
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(13);

            base.Configure(builder);
        }
    }
}
