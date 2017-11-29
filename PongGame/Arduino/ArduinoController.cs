using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Arduino
{
    public class ArduinoController
    {
        public SerialPort serialport;
        string message;

        public ArduinoController(string port)
        {
            try
            {
                serialport = new SerialPort(port, 115200);
                serialport.Open();
                serialport.DataReceived += new SerialDataReceivedEventHandler(serialport_DataReceived);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void closePort()
        {
            this.serialport.Close();
        }

        private void serialport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            message = serialport.ReadLine();
        }

        public string returnValue()
        {
            return message;
        }
    }
}
