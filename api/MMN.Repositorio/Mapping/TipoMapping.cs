using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MMN.Repositorio.Mapping
{
    public class TipoMapping : IEntityTypeConfiguration<Tipo>
    {
        public void Configure(EntityTypeBuilder<Tipo> builder)
        {
            builder.HasKey(k => k.IdTipo);

            builder.Property(k => k.IdTipo)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Chave)
                .IsUnicode(false)
                .HasMaxLength(10)
                .IsRequired();
            builder.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
