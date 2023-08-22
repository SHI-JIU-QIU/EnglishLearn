using FileService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.config
{
    public class UploadItemEntityConfig : IEntityTypeConfiguration<UploadItemEntity>
    {
        public void Configure(EntityTypeBuilder<UploadItemEntity> builder)
        {
            builder.ToTable("T_UploadItem");
            builder.Property(e=>e.FileName).IsUnicode(true).HasMaxLength(100);
            builder.Property(e=>e.FileSHA256Hash).IsUnicode(false).HasMaxLength(100);
        }
    }
}
