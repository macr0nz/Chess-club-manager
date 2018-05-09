using System.Collections.Generic;
using System.Linq;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.Models;

namespace Chess_club_manager.Tests.Mock
{
    public class TourGamesMockRepository : IRepository<TourGame>
    {
        public List<TourGame> TourGames { get; private set; }
        public TourGamesMockRepository()
        {
            TourGames = new List<TourGame>();
        }

        public IQueryable<TourGame> All()
        {
            return TourGames.AsQueryable();
        }

        public void Add(TourGame entity)
        {
            throw new System.NotImplementedException();
        }

        public void AddRange(IEnumerable<TourGame> entityList)
        {
            throw new System.NotImplementedException();
        }

        public void Update(TourGame entity)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateRange(IEnumerable<TourGame> entityList)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(TourGame entity)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}