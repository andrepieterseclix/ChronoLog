using CLog.Framework.Models;
using System.Data.Entity.ModelConfiguration;

namespace CLog.DataAccess.Mapping
{
    /// <summary>
    /// Represents the base for entity model to storage model mappers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{T}" />
    internal abstract class EntityMap<T> : EntityTypeConfiguration<T>
        where T : BusinessModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap{T}"/> class.
        /// </summary>
        public EntityMap()
        {
            // Key
            HasKey(t => t.Id);

            MapColumns();
            MapTable();
            MapRelationships();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Maps the columns.
        /// </summary>
        protected abstract void MapColumns();

        /// <summary>
        /// Maps the table.
        /// </summary>
        protected virtual void MapTable()
        {
            ToTable(typeof(T).Name);
        }

        /// <summary>
        /// Maps the relationships.
        /// </summary>
        protected virtual void MapRelationships()
        {
        }

        #endregion
    }
}
