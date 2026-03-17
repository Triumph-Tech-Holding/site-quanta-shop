using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(k => k.IdUsuario);


            builder.Property(e => e.IdUsuario)
                .HasDefaultValueSql("(newid())")
                .IsRequired();
            builder.Property(e => e.IdUsuarioPai)
                .IsRequired(false);
            builder.Property(e => e.AssinaturaEletronica)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Property(e => e.Celular)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(e => e.Cultura)
                .IsUnicode(false)
                .HasMaxLength(5);
            builder.Property(e => e.Documento)
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(e => e.Email)
                .IsUnicode(false)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(e => e.EmailConfirmado)
                .HasDefaultValueSql("((0))");
            builder.Property(e => e.Master)
                .HasDefaultValueSql("((0))");
            builder.Property(e => e.Login)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(e => e.Nome)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(e => e.Senha)
                .IsUnicode(false)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(e => e.SaltKey)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(e => e.EstadoRG)
                .HasMaxLength(2);
            builder.Property(e => e.OrgaoEmissorRG)
                .HasMaxLength(50);
            builder.Property(e => e.RG)
                .HasMaxLength(30);
            builder.Property(e => e.Empreendedor)
                .HasDefaultValue(true);
            builder.Property(e => e.Perfil)
                .HasMaxLength(1)
                .HasDefaultValue("E");
            builder.Property(e => e.UrlImg)
                .HasColumnName("URLIMG");
            builder.Property(p => p.DataBloqueio)
                .HasColumnType("datetime");
            builder.Property(p => p.DataReferencia)
                .HasColumnType("datetime");
            builder.Property(p => p.DataQualificacao)
                .HasColumnType("datetime");
            builder.Property(p => p.DataCadastro)
                .HasColumnType("datetime");


            builder.HasOne(d => d.Graduacao)
                .WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdGraduacao)
                .HasPrincipalKey(d => d.IdGraduacao)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Grupo)
                .WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdGrupo)
                .HasPrincipalKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.UsuarioPai)
                .WithMany(p => p.Filhos)
                .HasForeignKey(d => d.IdUsuarioPai)
                .HasPrincipalKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
