using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class UsuarioCarteiraMapping : IEntityTypeConfiguration<UsuarioCarteira>
    {
        public void Configure(EntityTypeBuilder<UsuarioCarteira> builder)
        {
            builder.HasKey(e => e.IdUsuarioCarteira);

            builder.Property(e => e.IdUsuarioCarteira)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Endereco)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.DataCadastro)
                .IsRequired();

            builder.Property(e => e.Ativo)
                .IsRequired();

            builder.Property(e => e.Aprovado)
                .IsRequired();

            builder.Property(e => e.DataAprovacao)
                .IsRequired(false);
            builder.Property(e => e.IdUsuario)
                .IsRequired();

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.UsuarioCarteira)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
