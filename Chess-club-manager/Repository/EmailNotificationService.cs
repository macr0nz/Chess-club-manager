using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;

namespace Chess_club_manager.Repository
{
    public class EmailNotificationService:IDisposable
    {
        private readonly IRepository<MailSettings> _mailSettingsRepository;
        private readonly MailSettings _mailSettings;
        private readonly SmtpClient _smtpClient;

        public EmailNotificationService()
        {
            this._mailSettingsRepository = new ChessClubManagerRepository<MailSettings>();

            this._mailSettings = this._mailSettingsRepository.All().SingleOrDefault();

            if (_mailSettings != null)
            {
                
                this._smtpClient = new SmtpClient
                {
                    Host = _mailSettings.MailServer,
                    Port = _mailSettings.MailPort,
                    EnableSsl = _mailSettings.EnableSsl,
                    Timeout = 10000,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                this._smtpClient.Credentials = new NetworkCredential
                {
                    UserName = _mailSettings.MailFrom,
                    Password = _mailSettings.MailPassword
                };
            }
        }


        public void SendRegistrationMail(string mailTo, string login, string password)
        {
            try
            {
                var message = new MailMessage
                {
                    Body = "<div>" +
                           "<h3><b>Chess Club Manager Registration:</b></h3>" +
                           $"<p>Login:</p><p>{login}</p>" +
                           $"<p>Password:</p><p>{password}</p>" +
                           "<p><b>You can change temp.password via user account settings.</b></p>" +
                           $"<hr/><p>Chess Club Manager {DateTime.Now}</p>" +
                           "</div>",
                    From = new MailAddress(_mailSettings.MailFrom),
                    To = { new MailAddress(mailTo) },
                    Subject = "Chess Club Manager Registration",
                    IsBodyHtml = true
                };
                
                this._smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                //log error
            }

            //log success send

        }


        public void Dispose()
        {
            _mailSettingsRepository?.Dispose();
        }
    }
}