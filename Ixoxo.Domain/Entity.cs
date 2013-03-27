using System;

namespace Ixoxo.Domain
{
    /// <summary>
    /// Base domain Entity
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TID">Id type</typeparam>
    [Serializable]
    public abstract class Entity<T, TID> : IEntity, ICloneable where T : Entity<T, TID>
    {
        public virtual TID Id { get; set; }


        /// <summary>
        /// This returns equality based on IDs.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (this is T && obj is T)

                return Id.Equals(((T)obj).Id);

            return false;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool operator ==(Entity<T, TID> c1, object c2)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(c1, c2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)c1 == null) || (c2 == null))
            {
                return false;
            }

            return c1.Equals(c2);
        }


        /// <summary>
        /// Not equals
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool operator !=(Entity<T, TID> c1, object c2)
        {
            return !(c1 == c2);
        }

        /// <summary>
        /// Version
        /// </summary>
        public virtual byte[] Version { get; set; }


        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }


    /// <summary>
    /// Base domain Entity interface
    /// </summary>
    public interface IEntity
    {
        byte[] Version { get; set; }
    }
}
