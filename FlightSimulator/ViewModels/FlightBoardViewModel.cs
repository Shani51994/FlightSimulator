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
        private double lat;
        private double lon;
        private static FlightBoardViewModel instance = null;

        /*
         * singleton to create only one instance of the FlightBoardViewModel
        */
        public static FlightBoardViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FlightBoardViewModel();
                }
                return instance;
            }
        }

        // Property of OpenSettingsWindow
        public ICommand OpenSettingsWindow
        {
            get
            {
                // activate OnClick
                return setCommand ?? (setCommand = new CommandHandler(() => OnClick()));
            }
        }

        /*
         * open the settings window
         */ 
        private void OnClick()
        {
            Settings settings = new Settings();
            // shows the window
            settings.ShowDialog();
        }


        bool isConnect = false;
        private ICommand connectCommand;

        // Property of ConnectCommand
        public ICommand ConnectCommand
        {
            get
            {
                // activate ConnectClick
                return connectCommand ?? (connectCommand = new CommandHandler(() => ConnectClick()));
            }
        }

        /*
          * if not connected- connect the client and server
        */
        private void ConnectClick()
        {
            // only if not connected- connect.
            if (!this.isConnect)
            {
                this.open();
                this.isConnect = true;
            }
            else
            {
                return;
            }
        }

        private void open()
        {
            Info.Instance.openServer();
            Command.Instance.startClient();
        }

        private ICommand disconnectCommand;

        // property of DisconnectCommand
        public ICommand DisconnectCommand
        {
            get
            {
                // activate DisconnectClick
                return disconnectCommand ?? (disconnectCommand = new CommandHandler(() => DisconnectClick()));
            }
        }

        /*
         * Disconnect the connection between the client and server
         */
        private void DisconnectClick()
        {
            if (this.isConnect)
            {
                Info.Instance.closeServer();
                Command.Instance.closeClient();
                this.isConnect = false;
            }
            else
            {
                return;
            }
        }
      
        // Property of Lon
        public double Lon
        {
            get
            {
                return this.lon;
            }
            set
            {
                this.lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        // Property of Lat
        public double Lat
        {
            get
            {
                return this.lat;
            }
            set
            {
                this.lat = value;
                NotifyPropertyChanged("Lat");
            }
        }
    }
}
