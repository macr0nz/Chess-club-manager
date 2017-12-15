using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Enum;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Helpers
{
    public static class ApplicationLogger
    {
        private static readonly IRepository<Log> _logRepository;

        static ApplicationLogger()
        {
            _logRepository = new ChessClubManagerRepository<Log>();
        }

        public static void Log(string message, LogType logType)
        {
            _logRepository.Add(new Log
            {
                Message = message,
                Type = logType
            });
        }
    }
}