using Microsoft.SPOT.Hardware;

namespace Sensors
{
    class AnalogSensor : Analog
    {
        /// <summary>
        /// To Simply Analog Reads
        /// </summary>
        /// <param name="_input">to use--> Pins.GPIO_Ax </param>
        /// <param name="_Resolution">Units / millivolt <br>
        /// </br> Example: LM35 = 0.1 C/mV (which is LM35 has 10 mv/ Celcius degree)</param>
        /// <param name="Index">How many Array Index you want to use as Moving Average? 
        /// default:4, Option 4,8,16,32</param>
        public AnalogSensor(Cpu.Pin _input, int Index,double _Resolution)
        :base(_input, Index)
        {
            Resolution = _Resolution;
        }
        protected override double Resolution
        { get; set; }
        
        public override double value
        {
            get { return base.value * Resolution; }
        }


        

    }
}
