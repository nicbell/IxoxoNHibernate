using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Ixoxo.Nhib
{
    public class NHibernateWebSessionModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            NHibernateSessionManager.ConnectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            context.EndRequest += (sender, e) => NHibernateSessionManager.CloseSession();
        }


        public void Dispose()
        {
        }
    }
}