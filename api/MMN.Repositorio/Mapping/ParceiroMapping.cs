using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class ParceiroMapping : IEntityTypeConfiguration<Parceiro>
    {
        public void Configure(EntityTypeBuilder<Parceiro> builder)
        {
            builder.HasKey(e => e.IdParceiro);

            builder.Property(p => p.IdParceiro)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.DataCriacao);
            builder.Property(e => e.DataAtualizacao)
                .IsRequired(false);
            builder.Property(e => e.Ativo);
            builder.Property(e => e.Descricao);
            builder.Property(e => e.Nome);
        }
    }
}
