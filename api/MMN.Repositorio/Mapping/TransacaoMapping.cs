using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.HasKey(k => k.IdTransacao);


            builder.Property(p => p.IdTransacao)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(p => p.ValorPrincipal)
                .HasColumnType("decimal(18, 8)");
            builder.Property(p => p.DataTransacao)
                .HasColumnType("datetime");

            builder.HasOne(d => d.Tipo)
                .WithMany(p => p.Transacao)
                .HasForeignKey(d => d.IdTipo)
                .HasPrincipalKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.Transacao)
                .HasForeignKey(d => d.IdUsuario)
                .HasPrincipalKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Status)
                .WithMany(p => p.Transacao)
                .HasForeignKey(d => d.IdStatus)
                .HasPrincipalKey(p => p.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Anunciante)
                .WithMany(p => p.Transacao)
                .HasForeignKey(d => d.IdAnunciante)
                .HasPrincipalKey(d => d.IdAnunciante)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
