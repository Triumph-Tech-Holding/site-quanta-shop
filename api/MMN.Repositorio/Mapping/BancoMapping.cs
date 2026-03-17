using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class BancoMapping : IEntityTypeConfiguration<Banco>
    {
        public void Configure(EntityTypeBuilder<Banco> builder)
        {
            builder.HasKey(e => e.IdBanco)
                .HasName("PK__Banco__2D3F553EA4AC90ED");


            builder.Property(p => p.IdBanco)
                .UseIdentityColumn()
                .IsRequired();
            builder.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Property(p => p.Nome)
                .HasMaxLength(255);


            builder.Property(e => e.Nome).IsUnicode(false);
        }
    }
}
