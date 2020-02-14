using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public NewsletterService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public void DeleteSubscribers(List<string> users)
        {
            List<Newsletter> newsletterUsers = _dbContext.Newsletter.Where(x => users.Contains(x.Email)).ToList();

            _dbContext.Newsletter.RemoveRange(newsletterUsers);

            _dbContext.SaveChanges();
        }

        public List<UserNewsletterViewModel> GetPaginated(int currentPage, int pageSize = 10)
        {
            List<UserNewsletterViewModel> newsletters = _dbContext.Newsletter
                .Select(x => new UserNewsletterViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    SubscribeDate = x.Date,
                    SubscribeDateString = x.Date.ToString("dd/MM/yyyy"),
                }).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return newsletters;
        }

        public int GetSubscribersCount()
        {
            return _dbContext.Newsletter.Count();
        }

        public bool IsSubscribed(string email)
        {
            return _dbContext.Newsletter.Any(x => x.Email == email && x.IsActive);
        }

        public void SendEmail(SendMailViewModel sendMail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(_configuration.GetValue<string>("Newsletter:SmtpClient"));

                mail.From = new MailAddress(_configuration.GetValue<string>("Newsletter:From"));

                foreach(var user in sendMail.Users)
                {
                    mail.To.Add(user);
                }

                mail.Subject = sendMail.Subject;
                mail.Body = sendMail.Body;
                mail.IsBodyHtml = true;

                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new System.Net.NetworkCredential(_configuration.GetValue<string>("Newsletter:Credentials:Username"),
                    _configuration.GetValue<string>("Newsletter:Credentials:Password"));
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch(Exception e)
            {
                //
            }
        }

        public bool Subscribe(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_configuration.GetValue<string>("Newsletter:SmtpClient"));

                mail.From = new MailAddress(_configuration.GetValue<string>("Newsletter:From"));
                mail.To.Add(email);
                mail.Subject = "You have subscribed to #AdminAdministration# Newsletter";
                mail.Body = "Hello!";
                mail.Body += "<br/><br/>";
                mail.Body += "Thank You for Your interest in our service.";
                mail.Body += "<br/>";
                mail.Body += "Several times a month, You will receive a newsletter with information about new app releases, major updates and other news form #AdminAdministration#";
                mail.Body += "<br/><br/>";
                mail.Body += "If You don't want to receive our occasional emails please unsubscribe <a href='https://localhost:44376/Blog/Unsubscribe?email=" + email + "'>here</a>.";
                mail.Body += "<br/><br/>";
                mail.Body += "Regards,";
                mail.Body += "<br/>";
                mail.Body += "#AdminAdministration# Team";
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_configuration.GetValue<string>("Newsletter:Credentials:Username"),
                    _configuration.GetValue<string>("Newsletter:Credentials:Password"));
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                _dbContext.Newsletter.Add(new Newsletter
                {
                    Email = email,
                    IsActive = true,
                    Date = DateTime.Now
                });

                return _dbContext.SaveChanges() == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Unsubscribe(string email)
        {
            if (IsSubscribed(email))
            {
                Newsletter newsletter = _dbContext.Newsletter.FirstOrDefault(x => x.Email == email);

                if (newsletter != null)
                {
                    _dbContext.Newsletter.Remove(newsletter);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}