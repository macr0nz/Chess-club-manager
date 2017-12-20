using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.Models;

namespace Chess_club_manager.Repository
{
    public class ChessClubManagerUnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;

        public IRepository<Tournament> TournamentsRepository { get; }
        public IRepository<TournamentTour> ToursRepository { get; }
        public IRepository<TourGame> TourGamesRepository { get; }
        public IRepository<ApplicationUser> UsersRepository { get; }
        public IRepository<News> NewsRepository { get; }
        public IRepository<MailSettings> MailSettingsRepository { get; }
        public IRepository<Log> LogsRepository { get; }

        public ChessClubManagerUnitOfWork()
        {
            _context = new ApplicationDbContext();

            TournamentsRepository = new ChessClubManagerRepository<Tournament>(_context);
            UsersRepository = new ChessClubManagerRepository<ApplicationUser>(_context);
            NewsRepository = new ChessClubManagerRepository<News>(_context);
            MailSettingsRepository = new ChessClubManagerRepository<MailSettings>(_context);
            LogsRepository = new ChessClubManagerRepository<Log>(_context);
            ToursRepository = new ChessClubManagerRepository<TournamentTour>(_context);
            TourGamesRepository = new ChessClubManagerRepository<TourGame>(_context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}