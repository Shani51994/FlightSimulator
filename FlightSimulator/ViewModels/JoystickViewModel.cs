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
        private float aileron;
        private float elevator;

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
                setChanges("controls/engines/current-engine/throttle", changeThrottle);
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
                setChanges("controls/flight/rudder", changeThrottle);
                NotifyPropertyChanged("changeRudder");
            }
        }

        public float Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                setChanges("controls/flight/aileron", aileron);
                NotifyPropertyChanged("aileron");
            }
        }

        public float Elevator
        {
            get
            {
                return elevator;
            }
            set
            {
                this.elevator = value;
                setChanges("controls/flight/elevator", elevator);
                NotifyPropertyChanged("elevator");
            }
        }

        private void setChanges(string pathName, float val)
        {
            string strVal = val.ToString("0.00");
            string command = "set " + pathName + " " + strVal;
            Command.Instance.JoystickSendToSimulator(command);
        }
    }
}
