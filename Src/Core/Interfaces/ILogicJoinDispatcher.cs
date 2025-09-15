using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using System;

namespace LinkLynx.Core.Interfaces
{
    public interface ILogicJoinDispatcher
    {  
        int Count { get; }
        
        bool TryAddToDispatcher(uint joinId, Action<PageLogicBase, SigEventArgs> action);
        
        bool Contains(uint joinId);
        
        Action<PageLogicBase, SigEventArgs> Get(uint joinId);

        void Clear();
    }
}
