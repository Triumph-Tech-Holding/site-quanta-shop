using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class OrdenacaoAnuncioMapping : IEntityTypeConfiguration<OrdenacaoAnuncio>
    {
        public void Configure(EntityTypeBuilder<OrdenacaoAnuncio> builder)
        {
            builder.HasKey(k => k.IdOrdenacaoAnuncio);

            builder.Property(p => p.IdOrdenacaoAnuncio)
                .IsRequired()
                .UseIdentityColumn();
            builder.Property(p => p.IdAnunciante);
            builder.Property(p => p.IdCredenciamento);
            builder.Property(p => p.Ordenacao);

            builder.HasOne(o => o.Anunciante)
                .WithOne(o => o.OrdenacaoAnuncio)
                .HasPrincipalKey<Anunciante>(f => f.IdAnunciante)
                .HasForeignKey<OrdenacaoAnuncio>(p => p.IdAnunciante);

            builder.HasOne(o => o.Credenciamento)
                .WithOne(o => o.OrdenacaoAnuncio)
                .HasPrincipalKey<Credenciamento>(f => f.IdCredenciamento)
                .HasForeignKey<OrdenacaoAnuncio>(p => p.IdCredenciamento);
        }
    }
}
