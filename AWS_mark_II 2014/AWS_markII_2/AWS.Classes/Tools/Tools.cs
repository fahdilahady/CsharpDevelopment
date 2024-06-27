using System;
using Microsoft.SPOT;
using System.Text;

namespace AwsClasses.Tools
{
    public abstract class Tools
    {
        public abstract bool Ready
        { get; set; }

        public abstract string Report
        { get; set; }

        
    }
}
