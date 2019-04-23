using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class JoystickViewModel : BaseNotify
    {
        private float changeThrottle;
        private float changeRudder;
        private float changeAileron;
        private float changeElevator;

        // properties
        public float ChangeThrottle
        {
            get
            {
                return this.changeThrottle;
            }
            set
            {
                this.changeThrottle = value;
                sliderChanges("controls/engines/current-engine/throttle", changeThrottle);
                NotifyPropertyChanged("changeThrottle");
            }
        }

        public float ChangeRudder
        {
            get
            {
                return this.changeRudder;
            }
            set
            {
                this.changeRudder = value;
                sliderChanges("controls/flight/rudder", changeThrottle);
                NotifyPropertyChanged("changeRudder");
            }
        }

        private void sliderChanges(string pathName, float val)
        {
            string strVal = val.ToString("0.00");
            string command = "set " + pathName + " " + strVal;
            Command.Instance.JoystickSendToSimulator(command);
        }
    }
}
