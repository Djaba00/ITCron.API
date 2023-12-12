using System;
using ITCron.API.Models.InternetProtocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITCron.API.DataAccess.Configurations
{
	public class IPInformationConfiguration : IEntityTypeConfiguration<IPInformation>
    {
        public void Configure(EntityTypeBuilder<IPInformation> builder)
        {
            builder.HasKey(c => c.IP);
        }
    }
}

