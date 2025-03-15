//using GP.DAL.Data.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GP.DAL.Data.Configrations
//{
//	public class PatientPhotoConfigration : IEntityTypeConfiguration<PatientPhoto>
//	{
//		public void Configure(EntityTypeBuilder<PatientPhoto> builder)
//		{
//			builder.HasOne(pp => pp.Appointment)
//				  .WithOne(a => a.PatientPhoto)
//				  .HasForeignKey<PatientPhoto>(pp => pp.AppointmentId);

			
//		}
//	}
//}
