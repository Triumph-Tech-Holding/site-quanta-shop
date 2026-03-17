using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class AlteracaoPerfilMapping : IEntityTypeConfiguration<AlteracaoPerfil>
    {
        public void Configure(EntityTypeBuilder<AlteracaoPerfil> builder)
        {
            builder.HasKey(e => e.IdAlteracaoPerfil);
            builder.Property(e => e.Observacao).IsRequired(false);
            builder.Property(e => e.Aceito).IsRequired();
            builder.Property(e => e.DataCadastro).IsRequired();
            builder.Property(e => e.DataAceite).IsRequired(false);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.AlteracaoPerfil)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.UsuarioAcao)
                .WithMany(p => p.AlteracaoPerfilAdmin)
                .HasForeignKey(d => d.IdUsuarioAcao)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
