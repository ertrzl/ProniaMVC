using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProniaMVC.Models;

namespace ProniaMVC.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable(tb => tb.HasCheckConstraint(
                "CK_ProductImage_Image_RequiredUnlessAdditional",
                "[IsPrimary] IS NULL OR [Image] IS NOT NULL"));
        }
    }
}
