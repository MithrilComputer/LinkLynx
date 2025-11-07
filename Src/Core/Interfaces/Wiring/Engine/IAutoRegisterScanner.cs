using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Interfaces.Wiring.Engine
{
    public interface IAutoRegisterScanner
    {
        void Run();
        void AutoWireJoins(Type pageType, ushort pageId);
        void TryRegisterEnumSigType(Type type);
    }
}
