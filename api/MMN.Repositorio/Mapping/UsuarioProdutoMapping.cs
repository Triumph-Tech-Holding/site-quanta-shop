using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class UsuarioProdutoMapping : IEntityTypeConfiguration<UsuarioProduto>
    {
        public void Configure(EntityTypeBuilder<UsuarioProduto> builder)
        {
            builder.HasKey(e => e.IdUsuarioProduto);

            builder.Property(e => e.DataVinculo).HasColumnType("datetime");

            builder.HasOne(d => d.Pedido)
                .WithMany(p => p.UsuarioProduto)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Produto)
                .WithMany(p => p.UsuarioProduto)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.UsuarioProduto)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
