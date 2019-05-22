
using Microsoft.EntityFrameworkCore;
using project.persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace project.persistence.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private members
        private CASContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private const bool Share_context = false;

        #endregion

        #region Constructors


        public RepositoryBase(CASContext context)
        {
            this._context = context;
            _dbSet = _context.Set<TEntity>();
        }

        #endregion

        #region Protected properties

        protected DbSet<TEntity> DbSet
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }

        #endregion

        #region Public methods


        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                return orderBy(query).AsEnumerable();
            }
            return query.AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetOrderBy(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, bool>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                return query.OrderBy(orderBy);
            }
            return query.AsEnumerable();
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return _dbSet.FromSql(query, parameters).ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual void Update(TEntity oldEntity, TEntity entityToUpdate)
        {
            _context.Entry(oldEntity).CurrentValues.SetValues(entityToUpdate);
        }


        public IQueryable<TEntity> All()
        {
            return DbSet.AsQueryable<TEntity>();
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public object SortDirection { get; private set; }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void Create(TEntity tEntity)
        {
            try
            {
                var newEntry = DbSet.Add(tEntity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);

            return 0;
        }

        public int ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(query, parameters);
        }
        //to read from stored procedure
        public IEnumerable<TEntity> ExecReadWithStoreProcedure(string query, params object[] parameters)
        {
            //return _context.Database.SqlQuery<TEntity>(query, parameters);
            return null;
        }

        public List<TEntity> ExecuteSP(string sql)//params object[] parameters
        {
            //return _context.Database.SqlQuery<TEntity>(sql).ToList();
            return null;
        }

        public TEntity ExecuteSPwithParameterForSingleRow(string sql, params object[] parameters)
        {
            //return _context.Database.SqlQuery<TEntity>(sql, parameters).FirstOrDefault();
            return null;
        }

        public List<TEntity> ExecuteSPwithParameterForList(string sql, params object[] parameters)
        {
            //return _context.Database.SqlQuery<TEntity>(sql, parameters).ToList();
            return null;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }


        #endregion
    }
}
