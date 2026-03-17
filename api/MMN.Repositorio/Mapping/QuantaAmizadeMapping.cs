using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    class QuantaAmizadeMapping : IEntityTypeConfiguration<QuantaAmizade>
    {
        public void Configure(EntityTypeBuilder<QuantaAmizade> builder)
        {
            builder.HasKey(k => k.IdQuantaAmizade);


            builder.Property(p => p.IdQuantaAmizade)
                .UseIdentityColumn()
                .IsRequired();
        }
    }
}
