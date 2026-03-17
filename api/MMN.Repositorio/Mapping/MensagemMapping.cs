using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class MensagemMapping : IEntityTypeConfiguration<Mensagem>
    {
        public void Configure(EntityTypeBuilder<Mensagem> builder)
        {
            builder.HasKey(e => e.IdMensagem);

            builder.Property(p => p.IdMensagem)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.DataCadastro).HasColumnType("datetime");

            builder.Property(e => e.Texto)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.HasOne(o => o.UsuarioDestino)
                .WithMany()
                .HasForeignKey(f => f.IdUsuarioDestino)
                .HasPrincipalKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
