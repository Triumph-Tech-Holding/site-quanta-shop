using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class TutorialMapping : IEntityTypeConfiguration<Tutorial>
    {

        public void Configure(EntityTypeBuilder<Tutorial> builder)
        {
            builder.HasKey(c => c.IdTutorial);

            
            builder.Property(c => c.IdTutorial)
                .UseIdentityColumn()
                .IsRequired();
        }
    }
}