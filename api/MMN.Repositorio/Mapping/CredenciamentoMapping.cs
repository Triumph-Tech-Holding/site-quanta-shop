using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CredenciamentoMapping : IEntityTypeConfiguration<Credenciamento>
    {
        public void Configure(EntityTypeBuilder<Credenciamento> builder)
        {
            builder.HasKey(e => e.IdCredenciamento).HasName("PK_dbo.Credenciamento");
            
            builder.Property(e => e.Estabelecimento).IsRequired();

            builder.HasOne(e => e.Cidade)
                .WithMany(e => e.Credenciamento)
                .HasForeignKey(e => e.IdCidade)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UsuarioPai)
                .WithMany(e => e.Credenciamentos)
                .HasForeignKey(e => e.IdUsuarioPai)
                .HasPrincipalKey(p=>p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Categoria)
                .WithMany(e => e.Credenciamentos)
                .HasForeignKey(e => e.IdCategoria)
                .HasPrincipalKey(p=>p.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Usuario)
                .WithOne(e => e.Credenciamento)
                .HasForeignKey<Credenciamento>(e => e.IdUsuario)
                .HasPrincipalKey<Usuario>(p=>p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
