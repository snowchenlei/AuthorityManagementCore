using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anc.Runtime.Session
{
    public interface IAncSession
    {
        int? UserId { get; }
    }
}