using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class SaqueMapping : IEntityTypeConfiguration<Saque>
    {
        public void Configure(EntityTypeBuilder<Saque> builder)
        {
            builder.HasKey(e => e.IdSaque);

            builder.Property(e => e.IdSaque)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.DataSolicitacao)
                .IsRequired();

            builder.Property(e => e.Valor)
                .IsRequired();

            builder.Property(e => e.DataProcessado)
                .IsRequired(false);

            builder.Property(e => e.Processado)
                .IsRequired();

            builder.Property(e => e.TaxaSaque)
                .IsRequired();

            builder.Property(e => e.Cotacao);

            builder.Property(e => e.DataAprovacao);

            builder.Property(e => e.Aprovador)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Historico)
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.IdUsuario)
               .IsRequired();
            builder.Property(e => e.IdStatus)
               .IsRequired();


            builder.HasOne(e => e.Status)
                .WithMany(e => e.Saque)
                .HasForeignKey(s => s.IdStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.Saque)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Tipo)
                .WithMany(d => d.Saque)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Transacao)
                .WithMany(d => d.Saque)
                .HasForeignKey(d => d.IdTransacao)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.UsuarioBanco)
                .WithMany(d => d.Saque)
                .HasForeignKey(d => d.IdUsuarioBanco)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
