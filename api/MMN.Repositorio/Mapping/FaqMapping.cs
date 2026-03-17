using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class FaqMapping : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.HasKey(e => e.IdFaq);

            builder.ToTable("FAQ");

            builder.Property(e => e.IdFaq).HasColumnName("IdFAQ");

            builder.Property(e => e.DataCadastro).HasColumnType("datetime");

            builder.Property(e => e.Pergunta)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Resposta)
                .IsRequired()
                .IsUnicode(false);
        }
    }
}
