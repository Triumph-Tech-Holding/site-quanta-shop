using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class LogGraduacaoMapping : IEntityTypeConfiguration<LogGraduacao>
    {
        public void Configure(EntityTypeBuilder<LogGraduacao> builder)
        {
            builder.HasKey(k => k.IdLogGraduacao);


            builder.Property(e => e.IdLogGraduacao)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(p => p.DataGraduacao)
                .HasColumnType("datetime");

            builder.HasOne(d => d.Graduacao)
                .WithMany(p => p.LogGraduacao)
                .HasForeignKey(d => d.IdGraduacao)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.LogGraduacao)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}