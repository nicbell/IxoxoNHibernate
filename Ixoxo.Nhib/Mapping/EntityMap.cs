using FluentNHibernate.Mapping;
using Ixoxo.Domain;

namespace Ixoxo.Nhib.Mapping
{
    public abstract class EntityMap<T> : ClassMap<T> where T : IEntity
    {
        protected EntityMap()
        {
            Version(x => x.Version)
            .CustomType("BinaryBlob")
            .UnsavedValue(null)
            .CustomSqlType("timestamp")
            .Nullable()
            .Generated.Always();
        }
    }
}
