using System;
using Microsoft.SPOT;

namespace SutronListener
{
    class aMniDataDesc
    {
        private string myRootPath;
        private string myFileName;
        private string myFullAddress;
        public string MyRootPath
        {
            get { return '@' + myRootPath; }
            set { myRootPath = value; }
        }

    }
}
