using CompassDirection;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace Sensors
{
    class WindVane: Encoder
    {
        Direction windDirection = new Direction();
        private Encoder MywindVane;

        /// <summary>
        /// Constructor WindVane Class
        /// </summary>
        /// <param name="_pinA">The pin used for output A</param>
        /// <param name="_pinB">The pin used for output A</param>
        /// <param name="_isLimitedRotation">Bool Is Set as Limited Rotation?</param>

        public WindVane(Cpu.Pin _pinA, Cpu.Pin _pinB, bool _isLimitedRotation)
            :this (_pinA, _pinB, _isLimitedRotation, 40, Cpu.Pin.GPIO_NONE)
        {}
        ///<param name="_maxVal">If true Insert Custom MaxVal of Limited Rotation :(default 40)</param>
        public WindVane(Cpu.Pin _pinA, Cpu.Pin _pinB, bool _isLimitedRotation, int _maxVal)
            : this(_pinA, _pinB, _isLimitedRotation, 40, Cpu.Pin.GPIO_NONE)
        { }
        ///<param name="_button">Which Pin Using As Button?</param>
        public WindVane(Cpu.Pin _pinA, Cpu.Pin _pinB, bool _isLimitedRotation, int _maxVal, Cpu.Pin _button = Cpu.Pin.GPIO_NONE)
        : base(_pinA, _pinB, _isLimitedRotation, _maxVal, _button)
        {         
        }
        public string pointTo16
        {
            get
            {
                return windDirection.Degreeto16Direction(base.windDegree);
            }
        }

        public string pointTo8
        {
            get
            {
                return windDirection.Degreeto8Direction(base.windDegree);
            }
        }

        public override double value
        {
            get
            {
                return base.value;
            }
        }
        public override void Reset()
        {
            base.Reset();
        }
    }
}
