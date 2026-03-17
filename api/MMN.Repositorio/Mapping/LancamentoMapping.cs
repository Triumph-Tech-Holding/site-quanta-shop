using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class LancamentoMapping : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            builder.HasKey(k => k.IdLancamento);

            builder.Property(p => p.IdLancamento)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(e => e.Bloqueado)
                .IsRequired().HasDefaultValue(false);
            builder.Property(p => p.Valor)
                .HasColumnType("decimal(18, 8)");
            builder.Property(p => p.DataLancamento)
                .HasColumnType("datetime");

            builder.HasOne(d => d.Tipo)
                .WithMany(p => p.Lancamento)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.Lancamento)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Status)
                .WithMany(p => p.Lancamento)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Status)
                .WithMany(m => m.Lancamento)
                .HasForeignKey(f => f.IdStatus)
                .HasPrincipalKey(p => p.IdStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Transacao)
                .WithMany(m => m.Lancamento)
                .HasForeignKey(f => f.IdTransacao)
                .HasPrincipalKey(p => p.IdTransacao)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
