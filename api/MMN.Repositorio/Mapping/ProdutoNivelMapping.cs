using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class ProdutoNivelMapping : IEntityTypeConfiguration<ProdutoNivel>
    {
        public void Configure(EntityTypeBuilder<ProdutoNivel> builder)
        {
            builder.HasKey(e => e.IdProdutoNivel);


            builder.Property(p => p.IdProdutoNivel)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Nivel).IsRequired();

            builder.Property(e => e.PorcentagemCashback).IsRequired();

            builder.Property(e => e.PorcentagemAdesao).IsRequired(false);

            builder.HasOne(d => d.Produto)
                .WithMany(p => p.ProdutoNivel)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
