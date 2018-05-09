using System.Collections.Generic;
using System.Linq;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.Models;

namespace Chess_club_manager.Tests.Mock
{
    public class TournamentsMockRepository : IRepository<Tournament>
    {
        public List<Tournament> Tournaments { get; private set; }
        public TournamentsMockRepository()
        {
            Tournaments = new List<Tournament>();
        }

        public IQueryable<Tournament> All()
        {
            return Tournaments.AsQueryable();
        }

        public void Add(Tournament entity)
        {
            throw new System.NotImplementedException();
        }

        public void AddRange(IEnumerable<Tournament> entityList)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Tournament entity)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Tournament> entityList)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Tournament entity)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}