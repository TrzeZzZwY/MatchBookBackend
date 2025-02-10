using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ReportingService.Domain.Models;

namespace ReportingService.Repository.Configurations;
public class BookConfiguration : IEntityTypeConfiguration<CaseEntity>
{
    public void Configure(EntityTypeBuilder<CaseEntity> builder)
    {
        builder.ToTable(nameof(CaseEntity));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.UserId);
        builder.Property(e => e.ReviewerId).IsRequired(false);
        builder.Property(e => e.CaseStatus);
        builder.Property(e => e.CaseItemType);
        builder.Property(e => e.SerializedCaseFields);
    }
}
