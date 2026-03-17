using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CidadeMapping : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.HasKey(e => e.IdCidade)
                .HasName("PK_tbCidades");

            builder.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(d => d.Estado)
                .WithMany(p => p.Cidade)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
