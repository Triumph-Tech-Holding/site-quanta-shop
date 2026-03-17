using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class MensagemGraduacaoMapping : IEntityTypeConfiguration<MensagemGraduacao>
    {

        public void Configure(EntityTypeBuilder<MensagemGraduacao> builder)
        {
            builder.HasKey(c => c.IdMensagemGraduacao);

            builder.Property(e => e.DataCadastro)
                .IsRequired();

            builder.HasOne(d => d.Mensagem)
                .WithMany(p => p.MensagemGraduacao)
                .HasForeignKey(d => d.IdMensagem)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Graduacao)
                .WithMany(p => p.MensagemGraduacao)
                .HasForeignKey(d => d.IdGraduacao)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}