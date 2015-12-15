using FluentNHibernate.Cfg.Db;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Ixoxo.Nhib
{
    /// <summary>
    /// NHibernate web session module, this is abstract because it needs implementing 
    /// with database configuration.
    /// </summary>
    public abstract class NHibernateWebSessionModule : IHttpModule
    {
        public abstract IPersistenceConfigurer DatabaseConfig { get; }

        public void Init(HttpApplication context)
        {
            NHibernateSessionManager.DatabaseConfig = DatabaseConfig;
            context.EndRequest += (sender, e) => NHibernateSessionManager.CloseSession();
        }


        public void Dispose()
        {
        }
    }
}