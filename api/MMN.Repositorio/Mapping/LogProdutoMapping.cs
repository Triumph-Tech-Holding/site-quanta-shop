using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class LogProdutoMapping : IEntityTypeConfiguration<LogProduto>
    {
        public void Configure(EntityTypeBuilder<LogProduto> builder)
        {
            builder.HasKey(k => k.IdLogProduto);


            builder.Property(p => p.IdLogProduto)
                .UseIdentityColumn()
                .IsRequired();


            builder.HasOne(o => o.Usuario)
                .WithMany()
                .HasForeignKey(f => f.IdUsuario)
                .HasPrincipalKey(f => f.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Produto)
                .WithMany()
                .HasForeignKey(f => f.IdProduto)
                .HasPrincipalKey(f => f.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}