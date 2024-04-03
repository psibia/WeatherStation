using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherStation.Services
{
    public class ArduinoService
    {
        private SerialPort serialPort;

        public ArduinoService(string portName, int baudRate = 9600)
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadLine();
            OnDataReceived?.Invoke(this, new DataReceivedEventArgs(data));
        }


        public event EventHandler<DataReceivedEventArgs> OnDataReceived;

        public class DataReceivedEventArgs : EventArgs
        {
            public string Data { get; }

            public DataReceivedEventArgs(string data)
            {
                Data = data;
            }
        }
    }
}
