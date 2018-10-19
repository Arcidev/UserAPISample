using Riganti.Utils.Infrastructure.Core;
using System;

namespace BL.Facades
{
    /// <summary>
    /// Base facade providing shared logic
    /// </summary>
    public abstract class BaseFacade
    {
        protected Func<IUnitOfWorkProvider> UowProviderFunc { get; }

        /// <summary>
        /// Protected ctor with required parameters
        /// </summary>
        /// <param name="uowFunc">Functor for instantiating <see cref="IUnitOfWorkProvider"/></param>
        protected BaseFacade(Func<IUnitOfWorkProvider> uowFunc)
        {
            UowProviderFunc = uowFunc;
        }
    }
}
