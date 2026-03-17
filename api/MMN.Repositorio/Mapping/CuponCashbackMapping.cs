using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CuponCashbackMapping : IEntityTypeConfiguration<CupomCashback>
    {
        public void Configure(EntityTypeBuilder<CupomCashback> builder)
        {
            builder.HasKey(e => e.IdCuponCashback);
            builder.HasAlternateKey(h => h.Token);
            
            builder
                .Property(e => e.IdCuponCashback)
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder.Property(e => e.Descricao);
            builder.Property(e => e.IdComerciante);//.IsRequired();
            builder.Property(e => e.IdCuponCashback);
            builder.Property(e => e.MeioPagamento);
            builder.Property(e => e.Token).IsRequired();
            builder.Property(e => e.Valor).IsRequired();

            builder.HasOne(d => d.Comerciante)
                .WithMany()
                .HasForeignKey(d => d.IdComerciante)
                .HasPrincipalKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
