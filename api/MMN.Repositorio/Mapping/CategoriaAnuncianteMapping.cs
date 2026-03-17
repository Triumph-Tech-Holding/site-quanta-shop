using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CategoriaAnuncianteMapping : IEntityTypeConfiguration<CategoriaAnunciante>
    {
        public void Configure(EntityTypeBuilder<CategoriaAnunciante> builder)
        {
            builder.HasKey(e => e.IdCategoriaAnunciante);

            builder.Property(p => p.IdCategoriaAnunciante)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Ativo).IsRequired();

            builder.HasOne(d => d.Categoria)
                .WithMany(p => p.CategoriaAnunciante)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Anunciante)
               .WithMany(p => p.CategoriaAnunciante)
               .HasForeignKey(d => d.IdAnunciante)
               .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
