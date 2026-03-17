using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;
using MMN.Util.Extensions;
using System;

namespace MMN.Repositorio.Mapping
{
    public class AuditoriaPremiacaoMapping : IEntityTypeConfiguration<AuditoriaPremiacao>
    {
        public void Configure(EntityTypeBuilder<AuditoriaPremiacao> builder)
        {
            builder.HasKey(e => e.IdAuditoriaPremiacao);

            builder.Property(p => p.IdAuditoriaPremiacao)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Login).IsRequired();
            builder.Property(e => e.Pontuacao).IsRequired();
            builder.Property(e => e.DataCadastro).IsRequired();
            builder.Property(e => e.IdUsuario).IsRequired();
            builder.Property(e => e.IdGraduacao).IsRequired();

            builder.HasOne(e => e.Usuario)
                .WithMany(e => e.AuditoriaPremiacao)
                .HasForeignKey(e => e.IdUsuarioDono)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Graduacao)
               .WithMany(e => e.AuditoriaPremiacao)
               .HasForeignKey(e => e.IdGraduacao)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
