using Riganti.Utils.Infrastructure.Core;
using System;

namespace BL.Facades
{
    public class BaseFacade
    {
        protected Func<IUnitOfWorkProvider> UowProviderFunc { get; }

        protected BaseFacade(Func<IUnitOfWorkProvider> uowFunc)
        {
            UowProviderFunc = uowFunc;
        }
    }
}
