using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class AuditoriaLgpdMapping : IEntityTypeConfiguration<AuditoriaLgpd>
    {
        public void Configure(EntityTypeBuilder<AuditoriaLgpd> builder)
        {
            builder.ToTable("AuditoriaLgpd");
            builder.HasKey(e => e.IdAuditoriaLgpd);
            builder.Property(e => e.IdAuditoriaLgpd).ValueGeneratedOnAdd();
            builder.Property(e => e.Campo).IsRequired().HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Motivo).HasMaxLength(500);
            builder.Property(e => e.IpOrigem).HasMaxLength(64);
            builder.Property(e => e.UserAgent).HasMaxLength(500);
            builder.Property(e => e.DataAcesso).IsRequired();

            builder.HasIndex(e => new { e.IdUsuarioMaster, e.DataAcesso });
            builder.HasIndex(e => new { e.IdUsuarioAlvo, e.DataAcesso });
        }
    }
}
