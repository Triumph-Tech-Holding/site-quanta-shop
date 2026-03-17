using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class UsuarioEnderecoMapping : IEntityTypeConfiguration<UsuarioEndereco>
    {
        public void Configure(EntityTypeBuilder<UsuarioEndereco> builder)
        {
            builder.HasKey(e => e.IdEndereco);

            builder.Property(e => e.Bairro)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.Cep)
                .IsRequired()
                .HasColumnName("CEP")
                .HasMaxLength(9)
                .IsUnicode(false);

            builder.Property(e => e.Numero)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Rua)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.UsuarioEndereco)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(o => o.Cidade)
                .WithMany()
                .HasForeignKey(f => f.IdCidade)
                .HasPrincipalKey(p => p.IdCidade)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
