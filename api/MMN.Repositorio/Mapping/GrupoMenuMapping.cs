using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class GrupoMenuMapping : IEntityTypeConfiguration<GrupoMenu>
    {
        public void Configure(EntityTypeBuilder<GrupoMenu> builder)
        {
            builder.HasKey(e => e.IdGrupoMenu);

            //builder.Property(e => e.IdGrupoMenu).ValueGeneratedNever();

            builder.Property(e => e.DataCadastro).HasColumnType("datetime");

            builder.HasOne(d => d.Grupo)
                .WithMany(p => p.GrupoMenu)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Menu)
                .WithMany(p => p.GrupoMenu)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
