using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class UsuarioBancoMapping : IEntityTypeConfiguration<UsuarioBanco>
    {
        public void Configure(EntityTypeBuilder<UsuarioBanco> builder)
        {
            builder.HasKey(e => e.IdUsuarioBanco)
                .HasName("PK__UsuarioB__DA9E9844CC5EC43F");

            builder.Property(e => e.IdUsuarioBanco)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Agencia)
                .IsUnicode(false)
                .HasMaxLength(10);
            builder.Property(e => e.Conta)
                .IsUnicode(false)
                .HasMaxLength(22);
            builder.Property(e => e.DigitoConta)
                .HasMaxLength(2);
            builder.Property(e => e.Cpfcnpj)
                .IsUnicode(false)
                .HasColumnName("CPFCNPJ")
                .HasMaxLength(22);
            builder.Property(e => e.DigitoAgencia)
                .IsUnicode(false)
                .HasMaxLength(22);

            builder.Property(e => e.DigitoConta).IsUnicode(false);

            builder.Property(e => e.NomeConta)
                .IsUnicode(false)
                .HasMaxLength(255);

            builder.HasOne(d => d.Banco)
                .WithMany(p => p.UsuarioBanco)
                .HasForeignKey(d => d.IdBanco)
                .HasPrincipalKey(d => d.IdBanco)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.UsuarioBanco)
                .HasForeignKey(d => d.IdUsuario)
                .HasPrincipalKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Tipo)
                .WithMany()
                .HasForeignKey(d => d.IdTipo)
                .HasPrincipalKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
