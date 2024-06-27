using System;
using Microsoft.SPOT;

namespace AwsClasses
{
    public abstract class Digital:Sensors
    {
        public abstract void Count();

        public abstract void Reset();
    }
}
