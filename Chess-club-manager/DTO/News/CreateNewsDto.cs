using System.Web.Mvc;

namespace Chess_club_manager.DTO.News
{
    public class CreateNewsDto
    {
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}