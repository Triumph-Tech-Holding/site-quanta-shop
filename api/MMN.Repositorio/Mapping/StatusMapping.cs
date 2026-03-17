using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class StatusMapping : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(e => e.IdStatus);

            builder.Property(e => e.IdStatus)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Ativo)
                .HasDefaultValueSql("((1))")
                .HasColumnType("bit");

            builder.Property(e => e.ChaveTraducao)
                .IsUnicode(false)
                .HasMaxLength(50);

            builder.Property(e => e.Nome)
                .IsUnicode(false)
                .HasMaxLength(50);
        }
    }
}
