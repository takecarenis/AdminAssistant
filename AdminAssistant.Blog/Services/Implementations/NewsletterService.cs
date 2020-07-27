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

        private readonly ILogService _logService;

        public NewsletterService(ApplicationDbContext dbContext, IConfiguration configuration, IWebHostEnvironment env,
            ILogService logService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _env = env;

            _logService = logService;
        }

        public bool DeleteSubscribers(List<string> users)
        {
            foreach(var user in users)
            {
                string encryptedEmail = Encryption.Encrypt(user, _configuration.GetValue<string>("Newsletter:Credentials:Password"), _configuration.GetValue<string>("Newsletter:SecretKey"));

                Newsletter newsletter = _dbContext.Newsletter.Where(x => encryptedEmail == x.Email).FirstOrDefault();

                _dbContext.Newsletter.Remove(newsletter);
            }
            

            return _dbContext.SaveChanges() == 1;
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
            string encryptedEmail = Encryption.Encrypt(email, _configuration.GetValue<string>("Newsletter:Credentials:Password"), _configuration.GetValue<string>("Newsletter:SecretKey"));
            return _dbContext.Newsletter.Any(x => x.Email == encryptedEmail && x.IsActive);
        }

        public bool SendEmail(SendMailViewModel sendMail)
        {
            try
            {
                foreach (var user in sendMail.Users)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpServer = new SmtpClient(_configuration.GetValue<string>("Newsletter:SmtpClient"));
                    string path = Path.Combine(_env.WebRootPath, "img\\", "logo2.png");

                    mail.From = new MailAddress(_configuration.GetValue<string>("Newsletter:From"), "PrivacyOneStop News");
                    mail.Headers.Add("Message-Id", "<PrivacyOneStop News>");

                    mail.To.Add(user);

                    mail.Subject = sendMail.Subject;
                    mail.Body = "<div style='text-align: center'>";
                    mail.Body += @"<img src=""cid:YourPictureId""></div>";
                    mail.Body += "<div style='text-align: left'>";
                    mail.Body += "<br/><br/>";
                    mail.Body += sendMail.Body;
                    mail.Body += "<br/><br/>";
                    mail.Body += "<h5 style='text-align: center; margin: 0px;'>Click <a href='https://privacyonestop.com/Blog/Unsubscribe?email=" + user + "'>here</a> to unsubscribe.</h5>";
                    mail.Body += "<h6 style='text-align: center; margin: 0px;'>This email address is not monitored. Please do not reply to this email.</h6>";
                    mail.Body += "<h6 style='text-align: center; margin: 0px;'>You can read our Terms & Conditions <a href='https://privacyonestop.com/Home/Terms'>here</a> and our Privacy Policy <a href='https://privacyonestop.com/Home/Privacy'>here</a>.</h6>";
                    mail.Body += "</div>";

                    string html = mail.Body;
                    AlternateView altView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                    LinkedResource yourPictureRes = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                    yourPictureRes.ContentId = "YourPictureId";
                    altView.LinkedResources.Add(yourPictureRes);
                    mail.AlternateViews.Add(altView);

                    mail.IsBodyHtml = true;

                    smtpServer.Port = 25;
                    smtpServer.UseDefaultCredentials = false;
                    smtpServer.Credentials = new System.Net.NetworkCredential(_configuration.GetValue<string>("Newsletter:Credentials:Username"),
                        _configuration.GetValue<string>("Newsletter:Credentials:Password"));
                    smtpServer.EnableSsl = false;

                    smtpServer.Send(mail);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logService.Log(LogType.Error, "Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    _logService.Log(LogType.Error, "Error: " + ex.InnerException.Message);
                }

                return false;
            }
        }

        public bool Subscribe(string email)
        {
            try
            {
                if (IsSubscribed(email)) return true;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_configuration.GetValue<string>("Newsletter:SmtpClient"));

                string path = Path.Combine(_env.WebRootPath, "img\\", "logo2.png");

                mail.From = new MailAddress(_configuration.GetValue<string>("Newsletter:From"), "PrivacyOneStop News");
                //mail.Headers.Add("Message-Id", "<PrivacyOneStop News>");

                mail.To.Add(email);
                mail.Subject = "You have subscribed to PrivacyOneStop Newsletter";
                mail.Body = "<div style='text-align: center'>";
                mail.Body += @"<img src=""cid:YourPictureId""></div>";
                mail.Body += "<div style='text-align: left'>";
                mail.Body += "<br/><br/>";
                mail.Body += "Dear privacy colleague,";
                mail.Body += "<br/><br/>";
                mail.Body += "You are now subscribed to the PrivacyOneStop newsletter. ";
                mail.Body += "<br/><br/>";
                mail.Body += "We will keep you informed about our articles, products, services and news from the privacy world. In short, everything you need to know about privacy will be at your fingertips.";
                mail.Body += "<br/><br/>";
                mail.Body += "Useful tip:";
                mail.Body += "<br/><br/>";
                mail.Body += "Save PrivacyOneStop email address: newsletter@privacyonestop.com in your email address book. This way you will be certain your newsletter will not be inadvertently treated as ‘spam’.";
                mail.Body += "<br/><br/>";
                mail.Body += "Privacy";
                mail.Body += "<br/><br/>";
                mail.Body += "If you wish to unsubscribe easily, you can click on the unsubscribe link at the bottom of our marketing emails. Alternatively, you can choose to write to <a href='mailto:privacy@privacyonestop.com'>privacy@privacyonestop.com</a>.";
                mail.Body += "<br/><br/>";
                mail.Body += "<h5 style='text-align: center; margin: 0px;'>Click <a href='https://privacyonestop.com/Blog/Unsubscribe?email=" + email + "'>here</a> to unsubscribe.</h5>";
                mail.Body += "<h6 style='text-align: center; margin: 0px;'>This email address is not monitored. Please do not reply to this email.</h6>";
                mail.Body += "<h6 style='text-align: center; margin: 0px;'>You can read our Terms & Conditions <a href='https://privacyonestop.com/Home/Terms'>here</a> and our Privacy Policy <a href='https://privacyonestop.com/Home/Privacy'>here</a>.</h6>";
                mail.Body += "</div>";

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

                SmtpServer.Send(mail);

                _logService.Log(LogType.Information, "Email is sent!");

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
                _logService.Log(LogType.Error, "Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    _logService.Log(LogType.Error, "Error: " + ex.InnerException.Message);
                }
                
                return false;
            }
        }

        public void Unsubscribe(string email)
        {
            if (IsSubscribed(email))
            {
                string encryptedEmail = Encryption.Encrypt(email, _configuration.GetValue<string>("Newsletter:Credentials:Password"), _configuration.GetValue<string>("Newsletter:SecretKey"));
                Newsletter newsletter = _dbContext.Newsletter.FirstOrDefault(x => x.Email == encryptedEmail);

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
            catch (Exception e)
            {
                return false;
            }
        }
    }
}