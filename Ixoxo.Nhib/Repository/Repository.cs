using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ixoxo.Domain.Repository;
using NHibernate;

namespace Ixoxo.Nhib.Repository
{
    // <summary>
    /// Base NHibernate repository.
    /// </summary>
    /// <typeparam name="T">Type of entity the repository is for.</typeparam>
    /// <typeparam name="TID">Type of the ID for the entity.</typeparam>
    public abstract class Repository<T, TID> : IRepository<T, TID> where T : class
    {
        protected Repository()
        {

        }


        protected ISession Session
        {
            get { return NHibernateSessionManager.GetCurrentSession(); }
        }


        #region IRepository<T,ID> Members

        public virtual T Get(TID id)
        {
            return Session.Get<T>(id);
        }


        public virtual T GetByIdAndLock(TID id)
        {
            return Session.Get<T>(id, LockMode.Upgrade);
        }


        public virtual IList<T> GetAll()
        {
            return Session.CreateCriteria(typeof(T)).List<T>();
        }


        public virtual IList<T> GetAll(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = Convert.ToInt32(Math.Ceiling(Session.QueryOver<T>().RowCount() / (double)pageSize));

            return Session.CreateCriteria(typeof(T))
                .SetFirstResult((pageNumber - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List<T>();
        }


        public virtual T Save(T entity)
        {
            Session.SaveOrUpdate(entity);
            Session.Flush();
            return entity;
        }


        public virtual void Delete(T entity)
        {
            Session.Delete(entity);
            Session.Flush();
        }


        public void Refresh(T entity)
        {
            var id = Session.GetIdentifier(entity);
            Session.Evict(entity);
            Session.Load(entity, id);
        }


        public void FlushSession()
        {
            if (Session != null)
                Session.Flush();
        }

        #endregion
    }
}