using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.News;
using Chess_club_manager.Filters;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    [Culture]
    [Authorize(Roles = "admin")]
    public class ManageNewsController : Controller
    {
        private readonly IRepository<News> _newsRepository;

        public ManageNewsController()
        {
            _newsRepository = new ChessClubManagerRepository<News>();
        }

        // GET: ManageNews
        public ActionResult Index()
        {
            var allNews = this._newsRepository.All().ToList();

            return View(allNews);
        }

        
        // GET: ManageNews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageNews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateNewsDto newsDto)
        {
            if (ModelState.IsValid)
            {
                this._newsRepository.Add(new News
                {
                    Content = newsDto.Content,
                    Title = newsDto.Title,
                });

                return RedirectToAction("Index");
            }

            return View(newsDto);
        }

        // GET: ManageNews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var news = this._newsRepository.All().Where(p => p.Id == id).Select(x => new EditNewsDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
            }).SingleOrDefault();

            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: ManageNews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditNewsDto editNewsDto)
        {
            if (ModelState.IsValid)
            {
                var news = this._newsRepository.All().SingleOrDefault(p => p.Id == editNewsDto.Id);

                if (news != null)
                {
                    news.Content = editNewsDto.Content;
                    news.Title = editNewsDto.Title;

                    this._newsRepository.Update(news);

                    return RedirectToAction("Index");
                }
            }
            return View(editNewsDto);
        }

        // GET: ManageNews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var news = this._newsRepository.All().SingleOrDefault(p => p.Id == id);

            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: ManageNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var news = this._newsRepository.All().SingleOrDefault(p => p.Id == id);

            if (news != null)
            {
                this._newsRepository.Delete(news);
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._newsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
