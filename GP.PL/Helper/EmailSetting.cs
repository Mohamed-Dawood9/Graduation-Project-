using GP.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace GP.PL.Helper
{
	public static class EmailSetting
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("mboa990@gmail.com", "hgny xotp svvl mqjb");
			client.Send("mboa990@gmail.com", email.Recipients, email.Subject, email.Body);
		}
	}
}
