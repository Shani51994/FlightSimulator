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

        // Property of TextUser
        public String TextUser
        {
            get
            {
                return this.textUser;
            }
            set
            {
                textUser = value;
                // notify that the text and background has change
                NotifyPropertyChanged("TextUser");
                NotifyPropertyChanged("BackgroundColor");
            }
        }

        // Property of BackgroundColor
        public String BackgroundColor
        {
            get
            {
                // if no text the background is white
                if (textUser == "")
                {
                    return "White";
                }
                else
                {
                    // if ok pressed and there is no change in text the background is white 
                    if (isOkPressed && (TextUser == oldTxt))
                    {
                        return "White";
                    }
                    isOkPressed = false;
                    return "Pink";
                }
            }
        }

        // Property of SendCommand
        public ICommand SendCommand
        {
            get
            {
                // activate SendClick
                return sendCommand ?? (sendCommand = new CommandHandler(() => SendClick()));
            }
        }

        // Property of ClearCommand
        public ICommand ClearCommand
        {
            get
            {
                // activate ClearClick
                return clearCommand ?? (clearCommand = new CommandHandler(() => ClearClick()));
            }
        }

        /*
         * sent the command to the simulator, and change the back ground to white
         */
        private void SendClick()
        {
            commandClient.sendToSimulator(textUser);
            oldTxt = TextUser;
            isOkPressed = true;
            NotifyPropertyChanged("BackgroundColor");
        }

        /*
         * delete the text in the auto pilot
         */
        private void ClearClick()
        {
            TextUser = ""; 
        }
     
    }
}
