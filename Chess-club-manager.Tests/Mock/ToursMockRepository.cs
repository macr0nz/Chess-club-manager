using System.Collections.Generic;
using System.Linq;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.Models;

namespace Chess_club_manager.Tests.Mock
{
    public class ToursMockRepository : IRepository<TournamentTour>
    {
        public List<TournamentTour> Tours { get; private set; }
        public ToursMockRepository()
        {
            Tours = new List<TournamentTour>();
        }

        public IQueryable<TournamentTour> All()
        {
            return Tours.AsQueryable();
        }

        public void Add(TournamentTour entity)
        {
            throw new System.NotImplementedException();
        }

        public void AddRange(IEnumerable<TournamentTour> entityList)
        {
            throw new System.NotImplementedException();
        }

        public void Update(TournamentTour entity)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateRange(IEnumerable<TournamentTour> entityList)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(TournamentTour entity)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}