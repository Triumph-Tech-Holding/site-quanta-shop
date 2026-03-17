using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(e => e.IdCategoria);

            builder.Property(e => e.IdCategoria)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Icone)
                .IsRequired(false)
                .HasMaxLength(256)
                .IsUnicode(false);

            builder.Property(e => e.ChaveTraducao)
                .IsRequired(false)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Chave)
                .IsRequired(false)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(d => d.CategoriaPai)
                .WithMany(p => p.Subcategorias)
                .HasForeignKey(d => d.IdCategoriaPai)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
