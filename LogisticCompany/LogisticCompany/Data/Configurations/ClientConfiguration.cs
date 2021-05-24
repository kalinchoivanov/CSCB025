using LogisticCompany.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticCompany.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(s => s.Shipments)
                .WithOne(c => c.Recipient)
                .HasForeignKey(c => c.RecipientId);
        }
    }
}
