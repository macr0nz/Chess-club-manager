using System;

namespace Chess_club_manager.DataModel.Entity
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}