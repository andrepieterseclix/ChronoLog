using CLog.UI.Common.Modules;
using System;
using System.Linq;
using System.Reflection;

namespace CLog.UI.Main
{
    /// <summary>
    /// Represents the module that will suck up all other <see cref="IModuleInitialiser"/> implementations registered with names through the container.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Modules.IModuleInitialiser" />
    public sealed class CompositeModule : IModuleInitialiser
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeModule"/> class.
        /// </summary>
        /// <param name="compositeModules">The composite modules.</param>
        public CompositeModule(IModuleInitialiser[] compositeModules)
        {
            Modules = compositeModules
                .Select(x => new
                {
                    Initialiser = x,
                    Order = x.GetType().GetCustomAttribute<ModuleInitialiserOrderAttribute>()?.Order ?? 0
                })
                .OrderBy(x => x.Order)
                .Select(x => x.Initialiser)
                .ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>
        /// The modules.
        /// </value>
        public IModuleInitialiser[] Modules { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public Common.Modules.Module Initialise(IDependencyContainer container)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
