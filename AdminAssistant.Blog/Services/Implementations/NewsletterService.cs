using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Helpers;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Implementations
{
    public class NewsletterService : INewsletterService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
<<<<<<< HEAD
        private readonly ILogService _logService;

        public NewsletterService(ApplicationDbContext dbContext, IConfiguration configuration, IWebHostEnvironment env, ILogService logService)
=======
        private readonly ILogger<NewsletterService> _logger;

        public NewsletterService(ApplicationDbContext dbContext, IConfiguration configuration, IWebHostEnvironment env, ILogger<NewsletterService> logger)
>>>>>>> 0a8387eb810fcad8f262981bc5ca1104882e79d6
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _env = env;
<<<<<<< HEAD
            _logService = logService;
=======
            _logger = logger;
>>>>>>> 0a8387eb810fcad8f262981bc5ca1104882e79d6
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
                    Email = Encryption.Decrypt(x.Email, _configuration.GetValue<string>("Newsletter:Credentials:Password"),
                    _configuration.GetValue<string>("Newsletter:SecretKey")),
                    IsActive = x.IsActive,
                    SubscribeDate = x.Date,
                    SubscribeDateString = x.Date.ToString("dd/MM/yyyy"),
                    Category = x.Category
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

                string path = Path.Combine(_env.WebRootPath, "img\\", "logo2.png");              

                mail.From = new MailAddress(_configuration.GetValue<string>("Newsletter:From"));
                mail.To.Add(email);
                mail.Subject = "You have subscribed to PrivacyOneStop Newsletter";
                mail.Body = @"<img style=""margin-left: 20%"" src=""cid:YourPictureId""></body></html>";
                mail.Body += "<br/><br/>";
                mail.Body += "Hello dear reader,";
                mail.Body += "<br/><br/>";
                mail.Body += "You have now subscribed to the PrivacyOneStop newsletter. You will get updates about our articles, products & services, and you will benefit from the most up-to-date highlights from the privacy world. You will have everything you need to know at the tip of your fingers.";
                mail.Body += "<br/><br/>";
                mail.Body += "A useful tip";
                mail.Body += "<br/><br/>";
                mail.Body += "You can save the PrivacyOneStop newsletter address newsletter@privacyonestop.com in your e-mail address book. This way you will be certain your newsletter will not be inadvertently treated as ‘spam’.";
                mail.Body += "<br/><br/>";
                mail.Body += "Privacy";
                mail.Body += "<br/><br/>";
                mail.Body += "If you wish to unsubscribe easily, you can click on the unsubscribe link at the bottom of our marketing emails. Alternatively, you can choose to write to <a href='mailto:privacy@privacyonestop.com'>privacy@privacyonestop.com</a>.";
                mail.Body += "<br/><br/>";
                mail.Body += "<h4 style='text-align: center; margin: 0px;'>Click <a href='http://privacyonestop.com/Blog/Unsubscribe?email=" + email + "'>here</a>. to unsubscribe.</h4>";
                mail.Body += "<h5 style='text-align: center; margin: 0px;'>This email address is not monitored. Please do not reply to this email.</h5>";
                mail.Body += "<h5 style='text-align: center; margin: 0px;'>You can read our Terms & Conditions here and our Privacy Policy here.</h5>";

                string html = mail.Body;
                AlternateView altView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                LinkedResource yourPictureRes = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                yourPictureRes.ContentId = "YourPictureId";
                altView.LinkedResources.Add(yourPictureRes);
                mail.AlternateViews.Add(altView);

                mail.IsBodyHtml = true;
                SmtpServer.Port = 25;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_configuration.GetValue<string>("Newsletter:Credentials:Username"),
                    _configuration.GetValue<string>("Newsletter:Credentials:Password"));
                SmtpServer.EnableSsl = false;

                _logger.LogInformation("Before sending");
                SmtpServer.Send(mail);

                _logger.LogInformation("Email is sent!");

                string encryptedEmail = Encryption.Encrypt(email, _configuration.GetValue<string>("Newsletter:Credentials:Password"), _configuration.GetValue<string>("Newsletter:SecretKey"));
                _dbContext.Newsletter.Add(new Newsletter
                {
                    Email = encryptedEmail,
                    IsActive = true,
                    Date = DateTime.Now,
                    Category = string.Empty
                });

                _logService.Log(LogType.Information, "Everything is okay");
                return _dbContext.SaveChanges() == 1;
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                _logService.Log(LogType.Error, "Error: " + ex.Message);
                if(ex.InnerException != null)
                {
                    _logService.Log(LogType.Error, "Error: " + ex.InnerException.Message);
                }

=======
                _logger.LogError("Error: " + ex.Message);
>>>>>>> 0a8387eb810fcad8f262981bc5ca1104882e79d6
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

        public bool UpdateUserCategory(SendMailViewModel updateCategory)
        {
            string categoryName = updateCategory.Subject;

            try
            {
                foreach (var user in updateCategory.Users)
                {
                    Newsletter newsletter = _dbContext.Newsletter
                        .FirstOrDefault(x => x.Email == Encryption.Encrypt(user,
                        _configuration.GetValue<string>("Newsletter:Credentials:Password"),
                        _configuration.GetValue<string>("Newsletter:SecretKey")));

                    if (newsletter != null)
                    {
                        newsletter.Category = categoryName;

                        if (_dbContext.SaveChanges() != 1) return false;
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}