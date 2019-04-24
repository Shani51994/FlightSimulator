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
        public Command commandClient;
        public bool isOkPressed = false;
        public string oldTxt = "";

        public AutoPilotViewModel()
        {
            commandClient = Command.Instance;
        }

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
                    if (isOkPressed && (TextUser == oldTxt))
                    {
                        return "White";
                    }
                    isOkPressed = false;
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
            commandClient.sendToSimulator(textUser);
            oldTxt = TextUser;
            isOkPressed = true;
            NotifyPropertyChanged("BackgroundColor");
        }

        private void ClearClick()
        {
            TextUser = ""; 
        }
     
    }
}
