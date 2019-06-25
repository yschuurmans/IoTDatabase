using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoT.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(e => new { e.Key, e.CollectionFK });

            builder.HasOne(d => d.Collection)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.CollectionFK)
                .HasConstraintName("FK_Collection_Item");
        }
    }
}
