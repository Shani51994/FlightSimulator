using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : BaseNotify
    {
        private ICommand sendCommand;
        private ICommand clearCommand;
        private String textUser = "";
    
    // properties
    public String TextUser
        {
            get
            {
                return this.textUser;
            }
            set
            {
                textUser = value;
                NotifyPropertyChanged("TextUser");
                NotifyPropertyChanged("BackgroundColor");
            }
        }

        public String BackgroundColor
        {
            get
            {
                if (textUser == "")
                {
                    return "White";
                }
                else
                {
                    return "Pink";
                }
            }
        }

        public ICommand SendCommand
        {
            get
            {
                return sendCommand ?? (sendCommand = new CommandHandler(() => SendClick()));
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return clearCommand ?? (clearCommand = new CommandHandler(() => ClearClick()));
            }
        }

        private void SendClick()
        {

        }

        private void ClearClick()
        {
            TextUser = "";
        }
     
    }
}
