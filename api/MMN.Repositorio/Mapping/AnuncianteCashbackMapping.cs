using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class AnuncianteCashbackMapping : IEntityTypeConfiguration<AnuncianteCashBack>
    {
        public void Configure(EntityTypeBuilder<AnuncianteCashBack> builder)
        {
            builder.HasKey(e => e.IdAnuncianteCashBack);

            builder.Property(p => p.IdAnuncianteCashBack)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Descricao)
                .IsRequired()
                .IsUnicode(false);
            builder.Property(e => e.Percentual)
                .IsRequired(false);
            builder.Property(e => e.ValorFixo)
                .IsRequired(false);
            builder.Property(e => e.IdProgramZanox)
                .HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Ativo)
                .IsRequired();
            builder.HasIndex(e => e.IdTrackingCategorie)
                .HasDatabaseName("Index_AnuncianteCashBack");
            builder.Property(e => e.DataCadastro)
                .IsRequired();
            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasOne(d => d.Anunciante)
                .WithMany(p => p.AnuncianteCashBack)
                .HasForeignKey(d => d.IdAnunciante)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
