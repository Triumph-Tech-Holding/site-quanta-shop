using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Repositorio.Mapping
{
    public class ConfiguracaoMapping : IEntityTypeConfiguration<Configuracao>
    {
        public void Configure(EntityTypeBuilder<Configuracao> builder)
        {
            builder.HasKey(e => e.IdConfiguracao);

            builder.Property(e => e.Chave)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.Valor)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Descricao)
                .IsUnicode(false);

            builder.Property(e => e.Ativo)
                .IsRequired();

            builder.Property(e => e.Editavel)
                .IsRequired();

            builder.Property(e => e.Tipo)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
