using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class GrupoMapping : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.HasKey(e => e.IdGrupo);

            builder.Property(e => e.Descricao)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);
        }
    }
}
