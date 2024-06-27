using Microsoft.SPOT.Hardware;
using System.Threading;
namespace AwsClasses
{
    public class RainRecorder : Digital
    {
        private int _counter = 0;
        private uint lastState = 0;
        protected override double Resolution
        {   get; set;  }
        
        private InterruptPort _input;
        /// <summary>
        /// Type of Digital Sensor
        /// to RainRecording
        /// </summary>
        /// <param name="InputPin">Digital Input ?</param>
        /// <param name="resolution">Please Provide Sensor Resolution, ex: 0.2 (mm)</param>
        public RainRecorder(Cpu.Pin InputPin, double resolution)
        {
            _input = new InterruptPort(InputPin, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeHigh);
            _input.OnInterrupt += new NativeEventHandler(_input_OnInterrupt);
            Resolution = resolution;
        }

        void _input_OnInterrupt(uint port, uint state, System.DateTime time)
        {
            if (state != lastState) 
            {
                if (state == 1)
                {
                    Count();
                }
            }
            lastState = state;
        }

    
        public override double value
        {
            get {  return _counter* Resolution; }
        }
        public override void Count()
        {
            _counter++; 
        }

        public override void Reset()
        {
            _counter = 0;
        }
    }
}
