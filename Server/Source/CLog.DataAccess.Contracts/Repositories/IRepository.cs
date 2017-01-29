using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CLog.DataAccess.Contracts.Repositories
{
    /// <summary>
    /// Represents the generic repository contract.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public interface IRepository<T> : IDisposable
        where T : class
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether changes will be committed manually using the <see cref="SaveChanges"/> method.
        /// </summary>
        /// <remarks>When set to <c>true</c>, all the changes will be committed within a transaction scope.</remarks>
        /// <value>
        ///   <c>true</c> for manual commits; otherwise, <c>false</c>.
        /// </value>
        bool ManualCommit { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the entities that satisfies the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The entities that satisfies the predicate.</returns>
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        
        /// <summary>
        /// Gets the entity that satisfies the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The entity.</returns>
        T Get(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);
        
        /// <summary>
        /// Counts the specified entities that satisfies the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The number of entities.</returns>
        long Count(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Save(T entity);

        /// <summary>
        /// Saves the changes that were made within a transaction scope.
        /// </summary>
        /// <remarks>
        /// The changes that were made will be committed within a transaction scope when this method is called.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">The respository has been configured for automatic commits.</exception>
        void SaveChanges();

        #endregion
    }
}
