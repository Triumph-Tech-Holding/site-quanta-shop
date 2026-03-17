using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class LancamentoRetidoMapping : IEntityTypeConfiguration<LancamentoRetido>
    {
        public void Configure(EntityTypeBuilder<LancamentoRetido> builder)
        {
            builder.HasKey(e => e.IdLancamentoRetido);


            builder.Property(e => e.IdLancamentoRetido)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(e => e.DataDesbloqueio).IsRequired(false);
            builder.Property(e => e.Ativo).IsRequired();

            builder.HasOne(d => d.Lancamento)
                .WithMany(p => p.LancamentoRetido)
                .HasForeignKey(d => d.IdLancamento)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Pedido)
                .WithMany(p => p.LancamentoRetido)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
