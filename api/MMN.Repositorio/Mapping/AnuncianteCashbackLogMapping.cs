using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class AnuncianteCashbackLogMapping : IEntityTypeConfiguration<AnuncianteCashBackLog>
    {
        public void Configure(EntityTypeBuilder<AnuncianteCashBackLog> builder)
        {
            builder.HasKey(e => e.IdAnuncianteCashBackLog);

            builder.Property(e => e.IdAnuncianteCashBackLog)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.IdAnuncianteCashBack);
            builder.Property(e => e.Descricao).IsRequired().IsUnicode(false);
            builder.Property(e => e.Percentual).IsRequired(false);
            builder.Property(e => e.ValorFixo).IsRequired(false);
            builder.Property(e => e.IdProgramZanox);
            builder.Property(e => e.Ativo).IsRequired();
            builder.HasIndex(e => e.IdTrackingCategorie);
            builder.Property(e => e.DataCadastro).IsRequired();
            builder.Property(e => e.DataAtualizacao).IsRequired();
        }
    }
}
