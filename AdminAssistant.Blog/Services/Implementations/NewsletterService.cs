using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Implementations
{
    public class NewsletterService : INewsletterService
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsletterService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsSubscribed(string email)
        {
            return _dbContext.Newsletter.Any(x => x.Email == email && x.IsActive);
        }

        public void Subscribe(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("stefan.djokic1995@gmail.com");
                mail.To.Add("take.care.nis@gmail.com");
                mail.Subject = "De ste pederi tejkerovci";
                mail.Body = "Ovde 'direktor' TakeCare-a. Ili duhovni vodja. Pravi neku aplikaciju, pa testira SMTP Server i slanje email-ova preko aplikacije, hehe ";

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("stefan.djokic1995@gmail.com", "VolimPivo04071995");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                _dbContext.Newsletter.Add(new Newsletter
                {
                     Email = email,
                     IsActive = true,
                     Date = DateTime.Now
                });
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
