using Domain;
using System.Collections.Generic;
using System.Linq;


namespace Core.Data
{
    public partial interface IRepository<T> where T : BaseEntity
    {
        AppDbContext DatabaseContext { get; }
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity, bool isCached = false);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities, bool isCached = false);

        int InsertGetId(T entity, bool isCached = false);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity, bool isCached = false);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities, bool isCached = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCached">是否删除缓存，默认true</param>
        /// <param name="disable">是否remove，默认false</param>
        void DeleteById(object id, bool isCached = true, bool disable = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isCached">是否删除缓存，默认true</param>
        /// <param name="disable">是否remove，默认false</param>
        void Delete(T entity, bool isCached = true, bool disable = false);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities, bool isCached = true);
        /// <summary>
        /// 多条记录一起插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int InsertRange(IEnumerable<T> list);
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        IQueryable<T> TableFromBuffer(int expire = 24);
    }
}
