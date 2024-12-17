using GP.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Configrations
{
	public class ProcessedImageConfigration : IEntityTypeConfiguration<ProcessedImage>
	{
		public void Configure(EntityTypeBuilder<ProcessedImage> builder)
		{
			builder.HasOne(pi => pi.PatientPhoto)
				  .WithMany(pp => pp.ProcessedImages)
				  .HasForeignKey(pi => pi.PatientPhotoId);

			builder.HasMany(pi => pi.Measurements)
				 .WithOne(m => m.ProcessedImage)
				 .HasForeignKey(m => m.ProcessedImageId);
		}
	}
}
