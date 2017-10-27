using System;
using System.Collections.Generic;


namespace Chess_club_manager.DTO.News
{
    public class ViewNewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedDate { get; set; }

        public IEnumerable<DataModel.Entity.News> OtherNews { get; set; }

    }
}