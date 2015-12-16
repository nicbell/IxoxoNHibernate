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
        private static ISessionFactory SessionFactory { get; set; }

        private static FluentConfiguration FluentConfig { get; set; }


        private static ISessionFactory GetFactory<T>() where T : ICurrentSessionContext
        {
            return FluentConfig
                .Cache(c => c.ProviderClass<SysCacheProvider>().UseQueryCache())
                .CurrentSessionContext<T>().BuildSessionFactory();
        }


        /// <summary>
        /// Configure NHibernate session manager
        /// </summary>
        /// <param name="config"></param>
        /// <param name="app"></param>
        public static void Configure(FluentConfiguration config)
        {
            FluentConfig = config;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.ApplicationInstance.EndRequest += delegate
                {
                    CloseSession();
                };
            }
        }


        /// <summary>
        /// Gets the current session or opens a new session
        /// </summary>        
        public static ISession GetCurrentSession()
        {
            // Check that we have config
            if (FluentConfig == null)
            {
                throw new Exception("Please configure NHibernateSessionManager");
            }

            // Create a SessionFactory if needed
            if (SessionFactory == null)
            {
                SessionFactory = HttpContext.Current != null ? GetFactory<WebSessionContext>() : GetFactory<ThreadStaticSessionContext>();
            }

            // Open a session if needed
            if (!CurrentSessionContext.HasBind(SessionFactory))
            {
                var session = SessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

            return SessionFactory.GetCurrentSession();
        }


        /// <summary>
        /// Closes the current session.
        /// </summary>
        public static void CloseSession()
        {
            if (SessionFactory != null && CurrentSessionContext.HasBind(SessionFactory))
            {
                var session = CurrentSessionContext.Unbind(SessionFactory);
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
            if (FluentConfig == null)
            {
                throw new Exception("Please configure NHibernateSessionManager");
            }

            new SchemaExport(FluentConfig.BuildConfiguration()).Create(false, true);
        }
    }
}