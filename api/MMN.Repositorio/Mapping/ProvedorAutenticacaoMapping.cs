using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class ProvedorAutenticacaoMapping : IEntityTypeConfiguration<ProvedorAutenticacao>
    {
        public void Configure(EntityTypeBuilder<ProvedorAutenticacao> builder)
        {
            builder.HasKey(h=>h.IdProvedorAutenticacao);

            builder.Property(p => p.IdProvedorAutenticacao)
                .UseIdentityColumn();
            builder.Property(p => p.EndpointCadastro)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(p => p.EndpointLogin)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(p => p.Login)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(p => p.ParametrosLogin)
                .HasMaxLength(2000)
                .IsRequired();
            builder.Property(p => p.Senha)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(p => p.UrlApi)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
