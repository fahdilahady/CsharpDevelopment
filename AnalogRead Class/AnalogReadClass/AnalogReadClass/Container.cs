using System;
using Microsoft.SPOT;

namespace AnalogReadClass
{
    class Container
    {
        public Container(){     
        }
        public Container(float _nilai)
        {
            //Constructor
            for (int i = 0; i < 4; ++i)
            {
                Read[i] = _nilai;
            }
           
        }
        private float[] _read = new float[4];
        private float[] Read
        {
            get { return _read; }
            set { _read = value; }
        }


        public float MovingAverage(float _nilai)
        {
            try
            {
                
                for (int i = 0; i < 3; ++i)
                {
                    Read[i] = Read[i + 1];
                }
                Read[3] = _nilai;
                return (Read[0] + Read[1] + Read[2] + Read[3]) /4;

            }
            catch (NullReferenceException)
            {
                return _nilai;
            }

            
        }

    }
}
