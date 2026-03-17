using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class MenuMapping : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(e => e.IdMenu);

            builder.Property(e => e.Texto)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Url)
                .IsRequired()
                .HasColumnName("URL")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.HasOne(d => d.MenuPai)
               .WithMany(p => p.SubMenus)
               .HasForeignKey(d => d.IdMenuPai);

            builder.Property(e => e.Ativo).IsRequired();

            builder.Property(e => e.Posicao).IsRequired();
            builder.Property(e => e.ChaveTraducao).IsRequired();

        }
    }
}
