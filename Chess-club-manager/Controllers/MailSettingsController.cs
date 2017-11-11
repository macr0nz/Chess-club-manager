using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO;
using Chess_club_manager.DTO.News;
using Chess_club_manager.Filters;
using Chess_club_manager.Repository;


namespace Chess_club_manager.Controllers
{
    [Culture]
    [Authorize(Roles = "admin, moderator")]
    public class MailSettingsController : Controller
    {
        private readonly IRepository<MailSettings> _mailSettingsRepository;

        public MailSettingsController()
        {
            this._mailSettingsRepository = new ChessClubManagerRepository<MailSettings>();
        }

        // GET: MailSettings
        public ActionResult Index()
        {
            var mailSettings = this._mailSettingsRepository.All().SingleOrDefault();

            if (mailSettings != null)
            {
                return View(mailSettings);
            }

            return RedirectToAction("Create");

        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit()
        {

            var mailSettings = this._mailSettingsRepository.All().Select(x => new MailSettingsDto
            {
                EnableSsl = x.EnableSsl,
                MailFrom = x.MailFrom,
                MailPassword = x.MailPassword,
                MailServer = x.MailServer,
                MailPort = x.MailPort,
            }).SingleOrDefault();

            if (mailSettings == null)
            {
                return HttpNotFound();
            }

            return View(mailSettings);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(MailSettingsDto mailSettingsDto)
        {
            if (ModelState.IsValid)
            {
                var mailSettings = this._mailSettingsRepository.All().SingleOrDefault();

                if (mailSettings != null)
                {
                    mailSettings.EnableSsl = mailSettingsDto.EnableSsl;
                    mailSettings.MailFrom = mailSettingsDto.MailFrom;
                    mailSettings.MailPassword = mailSettingsDto.MailPassword;
                    mailSettings.MailPort = mailSettingsDto.MailPort;
                    mailSettings.MailServer = mailSettingsDto.MailServer;
                    

                    this._mailSettingsRepository.Update(mailSettings);

                    return RedirectToAction("Index");
                }
            }
            return View(mailSettingsDto);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MailSettingsDto mailSettingsDto)
        {
            if (ModelState.IsValid)
            {
                this._mailSettingsRepository.Add(new MailSettings
                {
                    EnableSsl = mailSettingsDto.EnableSsl,
                    MailFrom = mailSettingsDto.MailFrom,
                    MailPassword = mailSettingsDto.MailPassword,
                    MailServer = mailSettingsDto.MailServer,
                    MailPort = mailSettingsDto.MailPort,
                });

                return RedirectToAction("Index");
            }

            return View(mailSettingsDto);
        }

    }
}