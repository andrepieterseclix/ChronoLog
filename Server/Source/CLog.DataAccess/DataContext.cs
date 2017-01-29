using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace CLog.DataAccess
{
    /// <summary>
    /// Represents the Entity Framework Data Context.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public sealed class DataContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <remarks>
        /// The configuration file of the host process should define a connection string named according to the name passed to the base constructor below.
        /// </remarks>
        public DataContext()
            : base("DbConnectionString")
        {
            // NOTE:  setup configuration here, e.g.
            //Configuration.LazyLoadingEnabled = true;
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // By not referencing anything from EntityFramework.SqlServer.dll, the file does not get copied to the output directory of the host process.
            Console.WriteLine("Forcing the dependency of '{0}'", typeof(SqlFunctions).Assembly);

            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
