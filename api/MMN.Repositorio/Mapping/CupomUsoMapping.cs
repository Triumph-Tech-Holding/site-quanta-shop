using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CupomUsoMapping : IEntityTypeConfiguration<CupomUso>
    {
        public void Configure(EntityTypeBuilder<CupomUso> builder)
        {
            builder.ToTable("CupomUso");
            builder.HasKey(e => e.IdCupomUso);
            builder.Property(e => e.IdCupomUso).ValueGeneratedOnAdd();
            builder.Property(e => e.ValorAplicado).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(e => e.DataUso).IsRequired();

            builder.HasOne(d => d.Cupom)
                .WithMany(p => p.Usos)
                .HasForeignKey(d => d.IdCupom)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.IdCupom, e.IdUsuario });
        }
    }
}
