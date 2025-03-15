using GP.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Configrations
{
	internal class AppointmentConfigration : IEntityTypeConfiguration<Appointment>
	{
		public void Configure(EntityTypeBuilder<Appointment> builder)

		{

			builder
					.HasOne(a => a.Patient)
					.WithMany(p => p.Appointments)
					.HasForeignKey(a => a.PatientId);

            builder.Property(p => p.CobbAngle).HasColumnType("decimal(18,2)");


        }
    }
}
