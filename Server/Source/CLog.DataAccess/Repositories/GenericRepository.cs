using CLog.DataAccess.Contracts.Repositories;
using CLog.Framework.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace CLog.DataAccess.Repositories
{
    /// <summary>
    /// Represents the generic repository as a base class for entity repositories to derive from.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.IRepository{T}" />
    public abstract class GenericRepository<T> : IRepository<T>
        where T : BusinessModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
        /// </summary>
        protected GenericRepository()
            : this(new DataContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected GenericRepository(DataContext dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));

            DataContext = dataContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether each operation will be committed manually or automatically.
        /// </summary>
        /// <value>
        /// <c>true</c> if this repository instance takes part in the Unit Of Work pattern; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, all the changes will be committed within a transaction scope.
        /// </remarks>
        public bool ManualCommit { get; set; }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        protected DataContext DataContext { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Add(T entity)
        {
            EnsureNotDisposed();

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            DataContext.Set<T>().Add(entity);

            if (!ManualCommit)
                DataContext.SaveChanges();
        }

        /// <summary>
        /// Counts the specified entities that satisfies the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// The number of entities.
        /// </returns>
        public long Count(Expression<Func<T, bool>> predicate = null)
        {
            EnsureNotDisposed();

            IQueryable<T> set = DataContext.Set<T>();

            return predicate == null
                ? set.Count()
                : set.Count(predicate);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Delete(T entity)
        {
            EnsureNotDisposed();

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            DataContext.Set<T>().Remove(entity);

            if (!ManualCommit)
                DataContext.SaveChanges();
        }

        /// <summary>
        /// Gets the entity that satisfies the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            EnsureNotDisposed();

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return DataContext.Set<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets all the entities that satisfies the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// The entities that satisfies the predicate.
        /// </returns>
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            EnsureNotDisposed();

            IQueryable<T> query = DataContext.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);

            return query.AsEnumerable();
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(T entity)
        {
            EnsureNotDisposed();

            DataContext.Set<T>().Attach(entity);
            ((IObjectContextAdapter)DataContext)
                .ObjectContext
                .ObjectStateManager
                .ChangeObjectState(entity, EntityState.Modified);

            if (!ManualCommit)
                DataContext.SaveChanges();
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Save(T entity)
        {
            if (entity.Id > 0)
                Update(entity);
            else
                Add(entity);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The respository has been configured for automatic commits.</exception>
        public void SaveChanges()
        {
            EnsureNotDisposed();

            if (!ManualCommit)
                throw new InvalidOperationException("The respository has been configured for automatic commits.");

            DataContext.SaveChanges();
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Keep this private, and create and maintain one for every derived class.
        /// </summary>
        private bool _disposed;

        protected void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Release managed resources, and set to null
                if (DataContext != null)
                {
                    DataContext.Dispose();
                    DataContext = null;
                }
            }

            // Release native resources
            // NOTE:  call Dispose(false); in finalizer if this class contains unmanaged resources.

            _disposed = true;
        }

        #endregion
    }
}
