using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class AutenticacaoExternaMapping : IEntityTypeConfiguration<AutenticacaoExterna>
    {
        public void Configure(EntityTypeBuilder<AutenticacaoExterna> builder)
        {
            builder.HasKey(h => h.IdAutenticacaoExterna);

            builder.Property(p => p.IdAutenticacaoExterna)
                .UseIdentityColumn();
            builder.Property(p => p.IdExterno)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasOne(o => o.Usuario)
                .WithMany(m => m.AutenticacaoExterna)
                .HasForeignKey(f => f.IdUsuario)
                .HasPrincipalKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.ProvedorAutenticacao)
                .WithMany(m => m.AutenticacaoExterna)
                .HasForeignKey(f => f.IdProvedorAutenticacao)
                .HasPrincipalKey(p => p.IdProvedorAutenticacao)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
