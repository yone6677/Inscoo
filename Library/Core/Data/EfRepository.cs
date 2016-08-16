using Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data
{
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields
        private static object locker = new object();
        private static object contextlocker = new object();
        private readonly IdentityDbContext<AppUser> _context;
        private IDbSet<T> _entities;
        private ICachingManager _cachingManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(ICachingManager cachingManager, IdentityDbContext<AppUser> context)
        {
            lock (locker)
            {
                _context = context;
                _entities = _context.Set<T>();
                _cachingManager = cachingManager;
            }
        }

        #endregion

        #region Methods
        public AppDbContext DatabaseContext
        {
            get
            {
                return this._context as AppDbContext;
            }
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
            //if (isCached)
            //{
            //    //var list = new List<T>();
            //    string key = typeof(T).Name;
            //    if (_cachingManager.IsSet(key))
            //    {
            //        var bufferTable = TableFromBuffer(expire);
            //        if (bufferTable.Any())
            //        {
            //            var item = bufferTable.Where(c => c.Id == int.Parse(id.ToString()));
            //            if (item.Any())
            //            {
            //                return item.FirstOrDefault();
            //            }
            //        }       
            //    }
            //    else
            //    {
            //        var item = Entities.Find(id);
            //        _cachingManager.Set(key, item, expire);
            //        return item;
            //    }
            //}
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return Entities.Find(id);
        }


        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity, bool isCached)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                _context.SaveChanges();
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<T> entities, bool isCached)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");
                _context.SaveChanges();
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        public virtual void DeleteById(object id, bool isCached, bool disable)
        {
            try
            {
                var entity = Entities.Find(id);
                Delete(entity, isCached, disable);
            }
            catch (Exception dbe)
            {
                throw dbe;
            }
        }
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity, bool isCached, bool disable)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (disable)
                {
                    Entities.Remove(entity);
                    _context.SaveChanges();
                }
                else
                {
                    entity.IsDeleted = true;
                    _context.SaveChanges();
                }
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities, bool isCached)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Entities.Remove(entity);

                _context.SaveChanges();
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public int InsertRange(IEnumerable<T> list)
        {
            try
            {
                foreach (T item in list)
                {
                    Entities.Add(item);
                }
                return _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(T entity, bool isCached)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);

                _context.SaveChanges();
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        public async Task InsertAsync(T entity, bool isCached)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);
                await _context.SaveChangesAsync();

                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        public virtual int InsertGetId(T entity, bool isCached)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);

                _context.SaveChanges();
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
                return entity.Id;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<T> entities, bool isCached)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Entities.Add(entity);
                _context.SaveChanges();
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        public async Task InsertAsync(IEnumerable<T> entities, bool isCached)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Entities.Add(entity);
                await _context.SaveChangesAsync();
                if (isCached)
                {
                    string key = typeof(T).Name;
                    if (_cachingManager.IsSet(key))
                    {
                        _cachingManager.Remove(key);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities.Where(s => !s.IsDeleted);
            }
        }
        public virtual IQueryable<T> TableFromBuffer(int expire)
        {
            string key = typeof(T).Name;
            if (_cachingManager.IsSet(key))
            {
                var cacheData = _cachingManager.Get<List<T>>(key);
                if (cacheData.Any())
                {
                    return cacheData.AsQueryable();
                }
            }
            var data = Entities;
            if (data.Any())
            {
                _cachingManager.Set(key, data.ToList(), expire);
                return data;
            }
            return null;
        }
        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.Where(s => !s.IsDeleted).AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        public virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }
        public AppDbContext DbContext
        {
            get
            {
                return _context as AppDbContext;
            }
        }
        #endregion
    }
}
