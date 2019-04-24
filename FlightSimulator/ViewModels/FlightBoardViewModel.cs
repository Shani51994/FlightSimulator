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

        bool isConnect = false;
        private ICommand connectCommand;

        // property
        public ICommand ConnectCommand
        {
            get
            {
                return connectCommand ?? (connectCommand = new CommandHandler(() => ConnectClick()));
            }
            set
            {

            }
        }

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

        // property

       public ICommand DisconnectCommand
        {
            get
            {
                return disconnectCommand ?? (disconnectCommand = new CommandHandler(() => DisconnectClick()));
            }
            set
            {

            }
        }

        private void DisconnectClick()
        {
            // only if not connected- connect.
            if (this.isConnect)
            {
                this.close();
                this.isConnect = false;
            }
            else
            {
                return;
            }
        }

        private void close()
        {
            Info.Instance.closeServer();
            //Command.Instance.startClient();
        }

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
