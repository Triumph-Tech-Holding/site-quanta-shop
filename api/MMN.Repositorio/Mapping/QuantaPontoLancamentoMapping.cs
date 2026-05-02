using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class QuantaPontoLancamentoMapping : IEntityTypeConfiguration<QuantaPontoLancamento>
    {
        public void Configure(EntityTypeBuilder<QuantaPontoLancamento> builder)
        {
            builder.ToTable("QuantaPontoLancamento");
            builder.HasKey(e => e.IdQuantaPontoLancamento);
            builder.Property(e => e.IdQuantaPontoLancamento).ValueGeneratedOnAdd();
            builder.Property(e => e.Tipo).IsRequired().HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.Origem).HasMaxLength(200);
            builder.Property(e => e.DataLancamento).IsRequired();
            builder.Property(e => e.Ativo).IsRequired();

            builder.HasIndex(e => new { e.IdUsuario, e.Ativo });
        }
    }
}
