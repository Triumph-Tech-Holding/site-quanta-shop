using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    class CacheResumoBinarioMapping : IEntityTypeConfiguration<CacheResumoBinario>
    {
        public void Configure(EntityTypeBuilder<CacheResumoBinario> builder)
        {
            builder.HasKey(k => k.IdCache);


            builder.Property(p => p.IdCache)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.DataLimite)
                .IsFixedLength();
            builder.Property(p => p.DataLimite)
                .HasMaxLength(10);

            builder.HasOne(d => d.Produto)
                .WithMany(p => p.CacheResumoBinario)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.CacheResumoBinario)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
