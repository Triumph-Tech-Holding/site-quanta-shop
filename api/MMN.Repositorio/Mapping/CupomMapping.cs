using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CupomMapping : IEntityTypeConfiguration<Cupom>
    {
        public void Configure(EntityTypeBuilder<Cupom> builder)
        {
            builder.ToTable("Cupom");
            builder.HasKey(e => e.IdCupom);

            builder.Property(e => e.IdCupom).ValueGeneratedOnAdd();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(60).IsUnicode(false);
            builder.HasIndex(e => e.Codigo).IsUnique();
            builder.Property(e => e.Tipo).IsRequired().HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.Valor).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(e => e.MinimoPedido).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Descricao).HasMaxLength(500);
            builder.Property(e => e.Ativo).IsRequired();
            builder.Property(e => e.DataCadastro).IsRequired();
        }
    }
}
