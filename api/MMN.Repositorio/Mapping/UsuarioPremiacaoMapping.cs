using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class UsuarioPremiacaoMapping : IEntityTypeConfiguration<UsuarioPremiacao>
    {
        public void Configure(EntityTypeBuilder<UsuarioPremiacao> builder)
        {
            builder.HasKey(e => e.IdUsuarioPremiacao);

            builder.Property(e => e.IdUsuarioPremiacao)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Premio)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.DataCadastro)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired(false);

            builder.Property(e => e.PremioEntregue)
                .IsRequired();

            builder.Property(e => e.IdUsuarioAcao)
                .IsRequired(false);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.UsuarioPremiacao)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Graduacao)
                .WithMany(p => p.UsuarioPremiacao)
                .HasForeignKey(d => d.IdGraduacao)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
