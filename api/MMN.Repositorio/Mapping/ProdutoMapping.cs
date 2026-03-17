using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(e => e.IdProduto);

            builder.Property(e => e.IdProduto)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Descricao)
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.ImagemUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.Valor).HasColumnType("decimal(18, 8)");
            builder.Property(e => e.TetoBinario).HasColumnType("decimal(18, 8)");
            builder.Property(e => e.Pontos).IsRequired();

            builder.HasOne(d => d.Categoria)
                .WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Produto_Categoria");
        }
    }
}
