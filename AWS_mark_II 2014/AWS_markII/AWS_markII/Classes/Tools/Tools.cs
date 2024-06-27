using System;
using Microsoft.SPOT;
using System.Text;

namespace Tools
{
    abstract class Tools
    {
        public abstract bool Ready
        { get; set; }

        public abstract string Report
        { get; set; }

        
    }
}
