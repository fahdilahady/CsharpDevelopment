using System;
using Microsoft.SPOT;

namespace Sensors
{
    abstract class Sensors
    {       
        public abstract double value
        {
            get;
        }


        public string valueToString
        {
            get { return value.ToString("F1"); }
        }

        protected abstract double Resolution
        {
            get;
            set;
        }
    }
}
