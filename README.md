ixoxo NHibernate [![Build status](https://ci.appveyor.com/api/projects/status/gwx6n2keo4r4tw5x)](https://ci.appveyor.com/project/nicbell/ixoxonhibernate)
===============

This solution provides.

## Ixoxo.Domain
To be used with your domain model.
* Base domain enity.
* Domain repository interface.

## Ixoxo.Nhib
To be used with your NHibernate implementation.
* NHibernate Session Manager with SysCache.
* NHibernate domain repository implementation.
* Base fluent NHibernate mapping.

## Available to on Nuget
```
PM> Install-Package Ixoxo.Domain
```
```
PM> Install-Package Ixoxo.Nhib
```

## Usage
Before you get started you will need to configure the session manager.
```cs
NHibernateSessionManager.configure(Fluently.Configure()
    .Database(MsSqlConfiguration.MsSql2008
#if DEBUG
    .ShowSql()
#endif
    .ConnectionString(c => c.Is(ConnectionString)))
    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Example>()));
```
