using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Arbitrators;
using Chess_club_manager.DTO.News;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    public class NewsController : Controller
    {

        private IRepository<News> newsRepository;

        public NewsController()
        {
            newsRepository = new ChessClubManagerRepository<News>();
        }

        
        public ActionResult Index()
        {
            var allNews = this.newsRepository.All().OrderByDescending(p => p.CreatedDate).ToList();

            return View(allNews);
        }

        public ActionResult Details(int? id)
        {
            var otherNewsCount = 3;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var news = this.newsRepository.All().SingleOrDefault(p => p.Id == id);

            if (news == null)
            {
                return HttpNotFound();
            }

            var newsCreatedDateRange1 = news.CreatedDate.AddDays(-3);
            var newsCreatedDateRange2 = news.CreatedDate.AddDays(3);

            var otherNews = this.newsRepository.All()
                .Where(p => p.Id != news.Id)
                .OrderBy(p => p.CreatedDate < newsCreatedDateRange1 || p.CreatedDate > newsCreatedDateRange2)
                .Take(otherNewsCount)
                .ToList();

            if (otherNews.Count == 0)
            {
                otherNews = this.newsRepository.All().Take(otherNewsCount).ToList();
            }

            var newsView = new ViewNewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                CreatedDate = news.CreatedDate,
                OtherNews = otherNews
            };
            
            return View(newsView);
        }
    }
}