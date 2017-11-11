namespace Chess_club_manager.DataModel.Entity
{
    public class MailSettings : Entity
    {
        public string MailFrom { get; set; }
        public string MailPassword { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public bool EnableSsl { get; set; }
    }
}