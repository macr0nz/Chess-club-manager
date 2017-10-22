using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.Models;

namespace Chess_club_manager.Repository
{
    public class ChessClubManagerRepository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public ChessClubManagerRepository()
        {
            this._context = new ApplicationDbContext();
            this._dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
        {
            return _dbSet.AsQueryable();
        }

        public void Add(TEntity entity)
        {
            this._dbSet.Add(entity);
            this._context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entityList)
        {
            this._dbSet.AddRange(entityList);
            this._context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            this._context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<TEntity> entityList)
        {
            foreach (var entity in entityList)
            {
                this._dbSet.Attach(entity);
                this._context.Entry(entity).State = EntityState.Modified;
            }

            this._context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            this._dbSet.Remove(entity);
            this._context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}