using System.Web.Mvc;

namespace Chess_club_manager.DTO.News
{
    public class EditNewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}