using IC.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace IC.Data.EntityConfig
{
    public class EquipmentConfiguration : EntityTypeConfiguration<Equipment>
    {
        public EquipmentConfiguration()
        {
            HasKey(e => e.EquipmentId);

            Property(e => e.TypeEquipment)
                .IsRequired();

            Property(e => e.ModelEquipment)
                .IsRequired();

            Property(e => e.DateAcquisition)
                .IsRequired();

            Property(e => e.ValueAcquisition)
                .IsRequired();

            Property(e => e.PathImage)
                .IsOptional();

            Property(e => e.Code)
                .IsRequired();

            HasIndex(e => e.Code)
                .IsUnique();

            Ignore(e => e.Photo);

            Ignore(e => e.EstimatedValueActs);
        }
    }
}