using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class MaterialApoioMapping : IEntityTypeConfiguration<MaterialApoio>
    {
        public void Configure(EntityTypeBuilder<MaterialApoio> builder)
        {
            builder.HasKey(e => e.IdMaterial);

            builder.ToTable("MaterialApoio");

            builder.Property(e => e.IdMaterial)
                .HasColumnName("IdMaterial")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(e => e.Nome)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Descricao)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.URLMaterial).HasColumnName("URLMaterial");

            builder.Property(e => e.DataCadastro).HasColumnType("datetime");
            
            builder.Property(e => e.UltimaAtualizacao).HasColumnType("datetime");

            builder.Property(e => e.Ativo);
        }
    }
}
