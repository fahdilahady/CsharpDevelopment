using System;
using Microsoft.SPOT;

namespace Sensors
{
    abstract class Digital:Sensors
    {
        public abstract void Count();

        public abstract void Reset();
    }
}
