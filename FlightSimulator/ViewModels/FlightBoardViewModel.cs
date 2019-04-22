using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Views;
using System.Windows.Input;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {
        private ICommand setCommand;

        // property
        public ICommand OpenSettingsWindow
        {
            get
            {
                return setCommand ?? (setCommand = new CommandHandler(() => OnClick()));
            }
        }

        private void OnClick()
        {
            Settings settings = new Settings();
            // shows the window
            settings.ShowDialog();
        }

        public double Lon
        {
            get;
        }

        public double Lat
        {
            get;
        }
    }
}
