using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    class CarrosselMapping : IEntityTypeConfiguration<Carrossel>
    {
        public void Configure(EntityTypeBuilder<Carrossel> builder)
        {
            builder.HasKey(k => k.IdCarrossel);


            builder.Property(p => p.IdCarrossel)
                .UseIdentityColumn()
                .IsRequired();
        }
    }
}
