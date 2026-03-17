using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class ArquivosMapping : IEntityTypeConfiguration<Arquivos>
    {
        public void Configure(EntityTypeBuilder<Arquivos> builder)
        {
            builder.HasKey(e => e.IdArquivos);

            builder.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Url)
                .IsRequired()
                .HasColumnName("URL")
                .HasMaxLength(255)
                .IsFixedLength();
        }
    }
}
