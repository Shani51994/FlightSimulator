using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel
    {
        private ICommand sendCommand;
        private ICommand clearCommand;
        //private String command;

        // properties
        //public String Command;

        public ICommand SendCommand
        {
            get
            {
                return sendCommand ?? (sendCommand = new CommandHandler(() => OnClick()));
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return clearCommand ?? (clearCommand = new CommandHandler(() => OnClick()));
            }
        }

        private void OnClick()
        {

        }

        private void OnClear()
        {

        }
    }
}
