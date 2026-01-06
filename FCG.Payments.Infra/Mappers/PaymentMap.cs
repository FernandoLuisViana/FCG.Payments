using FCG.Payments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Payments.Infra.Mappers;

public class PaymentMap : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
                .ValueGeneratedNever()
                .IsRequired();

        builder.Property(p => p.OrderId)
                .IsRequired();

        builder.Property(p => p.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PaymentMethod)
                .IsRequired();

        builder.Property(p => p.CreateDate)
                .IsRequired();
    }
}