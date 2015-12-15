using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Ixoxo.Nhib.Mapping;
using NHibernate;
using NHibernate.Caches.SysCache2;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Ixoxo.Nhib
{
    /// <summary>
    /// Manages the NHibernate session, please note SysCache2 has been added as the L2 cache provider.
    /// </summary>
    public class NHibernateSessionManager
    {
        private static ISessionFactory Factory { get; set; }

        /// <summary>
        /// Eg: Config = Fluently.Configure()
        ///    .Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(c => c.Is("xxx"))
        ///    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateSessionManager>());
        /// </summary>
        public static FluentConfiguration Config { get; set; }


        private static ISessionFactory GetFactory<T>() where T : ICurrentSessionContext
        {
            if (Config == null)
            {
                throw new Exception("Config not set. Please set NHibernateSessionManager.Config");
            }

            return Config
                .Cache(c => c.ProviderClass<SysCacheProvider>().UseQueryCache())
                .CurrentSessionContext<T>().BuildSessionFactory();
        }


        /// <summary>
        /// Gets the current session.
        /// </summary>
        public static ISession GetCurrentSession()
        {
            if (Factory == null)
                Factory = HttpContext.Current != null ? GetFactory<WebSessionContext>() : GetFactory<ThreadStaticSessionContext>();

            if (CurrentSessionContext.HasBind(Factory))
                return Factory.GetCurrentSession();

            var session = Factory.OpenSession();
            CurrentSessionContext.Bind(session);

            return session;
        }


        /// <summary>
        /// Closes the session.
        /// </summary>
        public static void CloseSession()
        {
            if (Factory != null && CurrentSessionContext.HasBind(Factory))
            {
                var session = CurrentSessionContext.Unbind(Factory);
                session.Close();
            }
        }


        /// <summary>
        /// Commits the session.
        /// </summary>
        /// <param name="session">The session.</param>
        public static void CommitSession(ISession session)
        {
            try
            {
                session.Transaction.Commit();
            }
            catch (Exception)
            {
                session.Transaction.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Creates the database from mapping in this assembly
        /// </summary>
        public static void CreateSchemaFromMappings()
        {
            if (Config == null)
            {
                throw new Exception("Config not set. Please set NHibernateSessionManager.Config");
            }

            new SchemaExport(Config.BuildConfiguration()).Create(false, true);
        }
    }
}