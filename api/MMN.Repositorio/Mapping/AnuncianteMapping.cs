using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class AnuncianteMapping : IEntityTypeConfiguration<Anunciante>
    {
        public void Configure(EntityTypeBuilder<Anunciante> builder)
        {
            builder.HasKey(e => e.IdAnunciante);


            builder.Property(p => p.IdAnunciante)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Nome)
                .IsRequired()
                .IsUnicode(false);
            builder.Property(e => e.ImagemUrl)
                .IsRequired()
                .IsUnicode(false);
            builder.Property(e => e.Cashback)
                .IsRequired();
            builder.Property(e => e.Ativo)
                .IsRequired();
            builder.Property(e => e.DataCadastro)
                .IsRequired();
            builder.Property(e => e.DataAtualizacao)
                .IsRequired();


            builder.HasIndex(h => new { h.IdAfilio, h.AccountId });
            builder.HasIndex(h => new { h.IdAwin, h.AccountId });
            builder.HasIndex(h => new { h.IdProgramZanox, h.AccountId });
        }
    }
}
