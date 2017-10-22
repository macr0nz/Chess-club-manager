using System.Collections.Generic;
using System.Linq;

namespace Chess_club_manager.DataModel.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All();

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entityList);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entityList);

        void Delete(TEntity entity);

    }
}