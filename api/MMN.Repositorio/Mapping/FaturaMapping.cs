using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class FaturaMapping : IEntityTypeConfiguration<Fatura>
    {
        public void Configure(EntityTypeBuilder<Fatura> builder)
        {
            builder.HasKey(k => k.IdFatura);


            builder.Property(p => p.IdFatura)
                .UseIdentityColumn()
                .IsRequired();


            builder.HasOne(f => f.Usuario)
                .WithMany(f => f.Faturas)
                .HasForeignKey(f => f.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
