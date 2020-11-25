using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebScrape.Entity;

namespace WebScrape.Dal.Configuration
{
    public class JobBoardConfiguration : IEntityTypeConfiguration<JobBoard>
    {
        public void Configure(EntityTypeBuilder<JobBoard> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AnnouncementDate).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.AnnouncementNo).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.City).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.District).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.EducationalStatus).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.Experience).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.IsHandicapped).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.JobInfo).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.MannerOfWork).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.Status).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
            builder.Property(x => x.WorkingArea).IsRequired(false).HasColumnType("varchar(MAX)").IsFixedLength();
        }
    }
}
