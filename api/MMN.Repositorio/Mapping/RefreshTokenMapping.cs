using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Token);
            builder.Property(r => r.Token).HasDefaultValueSql("(newid())");

            builder.HasOne(r => r.Usuario)
                .WithMany(r => r.RefreshToken)
                .HasForeignKey(r => r.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
