using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class UsuarioConfiguracaoMapping : IEntityTypeConfiguration<UsuarioConfiguracao>
    {
        public void Configure(EntityTypeBuilder<UsuarioConfiguracao> builder)
        {
            builder.HasKey(e => e.IdUsuarioConfiguracao);

            builder.Property(e => e.TaxaSaque).IsRequired(false);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.UsuarioConfiguracao)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
