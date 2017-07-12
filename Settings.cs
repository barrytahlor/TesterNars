using System;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Windows.Forms; // for Application.StartupPath

//namespace Termie
//{
    /// <summary>
    /// Persistent settings
    /// </summary>
    public class Settings
    {

    public string _prt;
    public string _path;
    public Port   port   = new Port();
    public Option option = new Option();

        public Settings(string port, string path)
        {
        this._prt = port;
        option.LogFileName = path + "\\" + port + ".log";
        this._path = path;
        }
        /// <summary> Port settings. </summary>
        public class Port
            {
            public string PortName = "COM1";
            public int    BaudRate = 19200;
            public int    DataBits = 8;
            public System.IO.Ports.Parity    Parity    = System.IO.Ports.Parity.Even;
            public System.IO.Ports.StopBits  StopBits  = System.IO.Ports.StopBits.One;
            public System.IO.Ports.Handshake Handshake = System.IO.Ports.Handshake.None;
            }

        /// <summary> Option settings. </summary>
        public class Option
        {
            public enum AppendType
            {
                AppendNothing,
                AppendCR,
                AppendLF,
                AppendCRLF
            }
		    public string LogFileName   = "";
            public bool Logging = false;
            public string ReadTimeOut = "60000";
    }

        /// <summary>
        ///   Read the settings from disk. </summary>
        public void Read(string inifile)
        {
            IniFile ini = new IniFile(_path + "\\" + _prt + ".ini");
            port.PortName       = ini.ReadValue("Port", "PortName", port.PortName);
            port.BaudRate       = ini.ReadValue("Port", "BaudRate", port.BaudRate);
            port.DataBits       = ini.ReadValue("Port", "DataBits", port.DataBits);
            port.Parity         = (Parity)Enum.Parse(typeof(Parity), ini.ReadValue("Port", "Parity", port.Parity.ToString()));
            port.StopBits       = (StopBits)Enum.Parse(typeof(StopBits), ini.ReadValue("Port", "StopBits", port.StopBits.ToString()));
            port.Handshake      = (Handshake)Enum.Parse(typeof(Handshake), ini.ReadValue("Port", "Handshake", port.Handshake.ToString()));
            option.LogFileName  = ini.ReadValue("Option", "LogFileName", option.LogFileName);
            option.ReadTimeOut  = ini.ReadValue("Option", "ReadTimeOut", option.ReadTimeOut);
            option.Logging = bool.Parse(ini.ReadValue("Option", "Logging", option.Logging.ToString()));
    }

        /// <summary>
        ///   Write the settings to disk. </summary>
        public void Write(string inifile)
        {
            IniFile ini = new IniFile(_path + "\\" + _prt + ".ini");
            ini.WriteValue("Port", "PortName",  port.PortName);
            ini.WriteValue("Port", "BaudRate",  port.BaudRate);
            ini.WriteValue("Port", "DataBits",  port.DataBits);
            ini.WriteValue("Port", "Parity",    port.Parity.ToString());
            ini.WriteValue("Port", "StopBits",  port.StopBits.ToString());
            ini.WriteValue("Port", "Handshake", port.Handshake.ToString());

            ini.WriteValue("Option", "LogFileName", option.LogFileName);
            ini.WriteValue("Option", "ReadTimeOut", option.ReadTimeOut);
            ini.WriteValue("Option", "Logging", option.Logging.ToString());
        }
	}
//}
