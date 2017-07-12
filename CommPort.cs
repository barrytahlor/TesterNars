using System;
using System.IO;
using System.Text;
using System.IO.Ports;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using Termie;
using System.Collections.Generic;

//namespace Termie
//{
/// <summary> CommPort class creates a singleton instance
/// of SerialPort (System.IO.Ports) </summary>
/// <remarks> When ready, you open the port.
///   <code>
///   CommPort com = CommPort.Instance;
///   com.StatusChanged += OnStatusChanged;
///   com.DataReceived += OnDataReceived;
///   com.Open();
///   </code>
///   Notice that delegates are used to handle status and data events.
///   When settings are changed, you close and reopen the port.
///   <code>
///   CommPort com = CommPort.Instance;
///   com.Close();
///   com.PortName = "COM4";
///   com.Open();
///   </code>
/// </remarks>
public class CommPort
    {

        string _PortName;
        string master_flag;
        SerialPort _serialPort;
		Thread _readThread;
		volatile bool _keepReading;
        volatile int start;
        int x_start, x_end;

        int test1 = 0;


       

        byte[] _in_temp_command = new byte[4];


        byte[] buffer_out = new byte[256];
        byte[] _in_temp_fl = new byte[4];
        byte[] _in_flow_fl = new byte[4];
        byte[] _inject_data2 = new byte[4];

        byte[] _in_temp_fl1  = new byte[4];
        byte[] _in_temp_fl2  = new byte[4];
        byte[] _in_temp_fl3  = new byte[4];
        byte[] _in_temp_fl4  = new byte[4];
        byte[] _in_temp_fl5  = new byte[4];
        byte[] _in_temp_fl6  = new byte[4];
        byte[] _in_temp_fl7  = new byte[4];
        byte[] _in_temp_fl8  = new byte[4];
        byte[] _in_temp_fl9  = new byte[4];
        byte[] _in_temp_fl10 = new byte[4];
        byte[] _in_temp_fl11 = new byte[4];
        byte[] _in_temp_fl12 = new byte[4];
        byte[] _in_temp_fl13 = new byte[4];
        byte[] _in_temp_fl14 = new byte[4];
        byte[] _in_temp_fl15 = new byte[4];
        byte[] _in_temp_fl16 = new byte[4];

        float[] f1 = new float[1];
        List<Termie.SharedMemory> _mem;
    string str2, strE;
    SM2 _memCommand2;

    byte[] comByte_inx;

    byte[] readBuffer;

    public  CommPort(string portName, List<Termie.SharedMemory> mem)
        {
			_serialPort = new SerialPort();
			_readThread = null;
			_keepReading = false;

            _PortName = portName;
            _mem = mem;
         
        }

    public CommPort getCommPort()
        {
            return this;
        }
        
    public void setup(SM2 memCommand2)
    {
        _memCommand2 = memCommand2;
    }  
    //begin Observer pattern

        public delegate void EventHandler(string param);
        public EventHandler StatusChanged;
        public EventHandler DataReceived;
        public EventHandler DataWrite;
        public EventHandler MasterStatus;


        string str;

        public ushort frameR = 0;
        public ushort frameW = 0;
        //end Observer pattern
        private void StartReading()
		{
			if (!_keepReading)
			{
            try
                {

                    _keepReading = true;
                    _readThread = new Thread(ReadPort);
                    _readThread.Start();
  
                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread: StartReading\n");
                    Console.WriteLine(err);
                }

            }
		}
		private void StopReading()
		{
			if (_keepReading)
			{

                frameR = 0;
                frameW = 0;
                _keepReading = false;

            _serialPort.Close();
            _serialPort.Open();

            Array.Clear(readBuffer, 0, readBuffer.Length);

            try {
                if (!_readThread.Join(3000))
                    _readThread.Abort();
                
            }
            catch (Exception err)
            {
                Console.Write("Exception Thread: StopReading\n");
                Console.WriteLine(err);
            }

            _readThread = null;
			}
		}
   
        #region HexToByte
        /// <summary>
        /// method to convert hex string into a byte array
        /// </summary>
        /// <param name="msg">string to convert</param>
        /// <returns>a byte array</returns>
        private byte[] HexToByte(string msg)
        {
            //remove any spaces from the string
            msg = msg.Replace(" ", "");
            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            byte[] comBuffer = new byte[msg.Length / 2];
            //loop through the length of the provided string
            for (int i = 0; i < msg.Length; i += 2)
                //convert each set of 2 characters to a byte
                //and add to the array
                //comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
                comBuffer[0] = (byte)0xad;
            //return the array
            return comBuffer;
        }
        #endregion

        #region ByteToHex
        /// <summary>
        /// method to convert a byte array into a hex string
        /// </summary>
        /// <param name="comByte">byte array to convert</param>
        /// <returns>a hex string</returns>
        private string ByteToHex(byte[] comByte)
        {
            //create a new StringBuilder object
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            //loop through each byte in the array
            foreach (byte data in comByte)
                //convert the byte to a string and add to the stringbuilder
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            //return the converted value
            return builder.ToString().ToUpper();
        }
        #endregion
        /// <summary> Open the serial port with current settings. </summary>
        public String Open(Settings settings)
        {
			Close();

            try
            {
                _serialPort.PortName  = settings.port.PortName;
                _serialPort.BaudRate  = settings.port.BaudRate;
                _serialPort.Parity    = settings.port.Parity;
                _serialPort.DataBits  = settings.port.DataBits;
                _serialPort.StopBits  = settings.port.StopBits;
                _serialPort.Handshake = settings.port.Handshake;
               

				// Set the read/write timeouts
				_serialPort.ReadTimeout = Int32.Parse(settings.option.ReadTimeOut);
				_serialPort.WriteTimeout = 50;

				_serialPort.Open();
				StartReading();
			}
            catch (IOException)
            {
                StatusChanged(String.Format("{0} does not exist", settings.port.PortName));
                return String.Format("{0} already in use", settings.port.PortName);
        }
            catch (UnauthorizedAccessException)
            {
                StatusChanged(String.Format("{0} already in use", settings.port.PortName));
                return String.Format("{0} already in use", settings.port.PortName);
            }
            catch (Exception ex)
            {
                StatusChanged(String.Format("{0}", ex.ToString()));
                return String.Format("{0} error", settings.port.PortName);
        }

            // Update the status
            if (_serialPort.IsOpen)
            {
                string p = _serialPort.Parity.ToString().Substring(0, 1);   //First char
                string h = _serialPort.Handshake.ToString();
                if (_serialPort.Handshake == Handshake.None)
                    h = "no handshake"; // more descriptive than "None"

                StatusChanged(String.Format("{0}: {1} bps, {2}{3}{4}, {5}",
                    _serialPort.PortName, _serialPort.BaudRate,
                    _serialPort.DataBits, p, (int)_serialPort.StopBits, h));
            }
            else
            {
                StatusChanged(String.Format("{0} already in use", settings.port.PortName));
                return String.Format("{0} already in use", settings.port.PortName);
            }
        return "OK";
        }

        /// <summary> Close the serial port. </summary>
        public void Close()
        {
            StopReading();
			if (IsOpen) _serialPort.Close();
            StatusChanged("Connection Closed");
        }

        /// <summary> Get the status of the serial port. </summary>
        public bool IsOpen
        {
            get
            {
                return _serialPort.IsOpen;
            }
        }

        /// <summary> Get a list of the available ports. Already opened ports
        /// are not returend. </summary>
        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ack_msg
        {
            public byte addr;
            public byte fc;
            public ushort staddr;
            public ushort Qor;
            public byte crc1;
            public byte crc2;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct data_resp
        {
            public byte addr;
            public byte fc;
            public byte staddr;
            public byte data1;
            public byte data2;
            public byte data3;
            public byte data4;
            public byte data5;
            public byte data6;
            public byte data7;
            public byte data8;
            public byte crc1;
            public byte crc2;
        }
        void ByteArrayToStructure(byte[] bytearray, ref object obj)
            {
                int len = Marshal.SizeOf(obj);

                IntPtr i = Marshal.AllocHGlobal(len);

                Marshal.Copy(bytearray, 0, i, len);

                obj = Marshal.PtrToStructure(i, obj.GetType());

                Marshal.FreeHGlobal(i);
            }
        byte[] StructureToByteArray_ack_msg(ack_msg obj)
        {
            int len = Marshal.SizeOf(obj);

            byte[] arr = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);

            Marshal.Copy(ptr, arr, 0, len);

            Marshal.FreeHGlobal(ptr);

            return arr;
        }
        byte[] StructureToByteArray_data_resp(data_resp obj)
        {
            int len = Marshal.SizeOf(obj);

            byte[] arr = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);

            Marshal.Copy(ptr, arr, 0, len);

            Marshal.FreeHGlobal(ptr);

            return arr;
        }
        byte[] CRC_data_resp_msg(data_resp msg, bool bad)
    {
        byte data_s;
        data_s = 8;
        int len;
        ushort crc;

        ////////////////////////////////////
        //CRC
        len = 3 + data_s;
        crc = 0xFFFF;
        buffer_out = StructureToByteArray_data_resp(msg);

        for (int pos = 0; pos < len; pos++)
        {
            crc ^= buffer_out[pos];    // XOR byte into least sig. byte of crc

            for (int i = 8; i != 0; i--)
            {    // Loop over each bit
                if ((crc & 0x0001) != 0)
                {      // If the LSB is set
                    crc >>= 1;                    // Shift right and XOR 0xA001
                    crc ^= 0xA001;
                }
                else                            // Else LSB is not set  checkBoxBadCRC1
                    crc >>= 1;                    // Just shift right
            }
        }

            buffer_out[len + 0] = (byte)(crc & 0xFF);
            buffer_out[len + 1] = (byte)((crc >> 8) & 0xFF);
           
            if (bad)
            {
                buffer_out[len + 0] = 0;
                buffer_out[len + 1] = 0;
            }
        len = len + 2;

        ///////////   end of CRC  ///////////////
        return buffer_out;
    }
        byte[] CRC_msg_ack_msg(ack_msg msg, bool bad)
    {
        byte data_s;
        data_s = 3;
        int len;
        ushort crc;

        ////////////////////////////////////
        //CRC
        len = 3 + data_s;
        crc = 0xFFFF;
        buffer_out = StructureToByteArray_ack_msg(msg);

        for (int pos = 0; pos < len; pos++)
        {
            crc ^= buffer_out[pos];    // XOR byte into least sig. byte of crc

            for (int i = 8; i != 0; i--)
            {    // Loop over each bit
                if ((crc & 0x0001) != 0)
                {      // If the LSB is set
                    crc >>= 1;                    // Shift right and XOR 0xA001
                    crc ^= 0xA001;
                }
                else                            // Else LSB is not set  checkBoxBadCRC1
                    crc >>= 1;                    // Just shift right
            }
        }

            buffer_out[len + 0] = (byte)(crc & 0xFF);
            buffer_out[len + 1] = (byte)((crc >> 8) & 0xFF);
            

            if (bad) {
            buffer_out[len + 0] = 0;
            buffer_out[len + 1] = 0;
            }
        len = len + 2;
        ///////////   end of CRC  ///////////////
        return buffer_out;
    }

    DateTime startime;
    DateTime startime2;
    private void ReadPort()
    {
        int flag = 0;
        int flag2 = 0;
        int retry = 0;
        int tmp = 0;
        start = 0;
        byte[] newBuffer = new byte[32];
        int newbuffer_size = 32;
        int Gnewbuffer_size = 32;
        

        TimeSpan timeDiff;
       

        while (_keepReading)
        {
            if (_serialPort.IsOpen)
            {


                readBuffer = new byte[newbuffer_size + 1];

                try
                {
                    if (flag2 == 0)
                    {
                        startime2 = DateTime.Now;
                        str = startime2.ToString("dd-MMM-yy hh:mm:ss.fff");
                        flag2 = 1;
                    }


                    // If there are bytes available on the serial port,
                    // Read returns up to "count" bytes, but will not block (wait)
                    // for the remaining bytes. If there are no bytes available
                    // on the serial port, Read will block until at least one byte
                    // is available on the port, up until the ReadTimeout milliseconds
                    // have elapsed, at which time a TimeoutException will be thrown.

                    int count = _serialPort.Read(readBuffer, 0, newbuffer_size);
                    //Console.Write(" _serialPort.Read (" + count + " )\n");

                    //Since the data comes in smaller packets - 
                    // dont display until you fill the expected packet
                    if (flag == 0)
                    {
                        startime = DateTime.Now;
                        str = startime.ToString("dd-MMM-yy hh:mm:ss.fff");
                        flag = 1;

                        if (readBuffer[0] != 0x01 && readBuffer[0] != 0x5A ) {
                            _serialPort.Close();
                            _serialPort.Open();
                            continue;
                        }

                    }


                    timeDiff = DateTime.Now - startime;



                    int j = 0;
                    tmp = tmp + count;
                    newbuffer_size = Gnewbuffer_size - tmp;
                    //Console.Write("newbuffer_size = " + newbuffer_size + ", Gnewbuffer_size = " + Gnewbuffer_size + ", tmp = " + tmp + "\n");

                    byte[] tmpBuffer = new byte[count];
                    for (int ic = 0; ic < count; ic++)
                    {
                        tmpBuffer[ic] = readBuffer[ic];
                    }


                    MasterStatus(DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff") + "> " + "Read cnt " + count + " Total " + tmp + " < " + ByteToHex(tmpBuffer) + "> " + timeDiff.TotalMilliseconds + " ms");


                    // Do some work




                    if (tmp <= Gnewbuffer_size)
                    {


                        x_start = tmp - (count - 1) - 1;
                        x_end = tmp;

                        //Console.Write(" newBuffer start "+ x_start + " loop " + x_end + "\n");
                        for (int ii = x_start; ii < x_end; ii++)
                        {
                            //Console.Write(" newBuffer[" + ii + "] =  readBuffer[" + j + "] \n");
                            newBuffer[ii] = readBuffer[j];
                            j++;
                        }

                        ///////////////////////////////
                        // Packet is ready
                        if (tmp == Gnewbuffer_size || retry == 4)
                        {
                            flag = 0;
                            flag2 = 0;
                            tmp = 0;
                            frameR++;
                            retry = 0;
                            newbuffer_size = 32;
                            //package buffer
                            String SerialIn = System.Text.Encoding.ASCII.GetString(newBuffer, 0, newbuffer_size);

                            DataWrite(str + "# " + frameR + ") Rx:   " + ByteToHex(newBuffer));  //  Send buffer as String to GUI

                        } // if (tmp == newbuffer_size)
                    } //if (tmp <= newbuffer_size)


                    else
                    {
                        tmp = 0;
                        _serialPort.Close();
                        _serialPort.Open();
                        retry++;
                    }

                }  //Try
                catch (Exception err)
                {
                    strE = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
                    MasterStatus("Exception Thread: ReadPort:" + err.ToString() + " " + strE);
                    Console.WriteLine(err);
                    //StatusChanged(String.Format(err.ToString() + " " + strE));
                    //StopReading();
                    //if (IsOpen) _serialPort.Close();
                    //return String.Format(err.ToString());
                }
                finally
                {
                    //Console.Write("Exception finally,Thread: ReadPort\n");                 
                }

            }  // if serial port is open
            else
            {
                MasterStatus(DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff") + "> " + "Sleep 5000ms \n");
                TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 5000);
                Thread.Sleep(waitTime);
            }
        } // while keepreading
      
    }

    public void Send(byte[] comByte, int siz)
            {     

            // Test Master
            master_flag = "";
            switch (_PortName)
                {
                case "Port1":

                if (siz == 17)
                {
                    // port 1    0x01, 0x04, 0x14D8, 0x0002, CRC1, CRC2   // Temp
                    if (comByte[0] != 0x01) master_flag = "Bad";
                    if (comByte[1] != 0x10) master_flag = "Bad";
                    if (comByte[2] != 0x10) master_flag = "Bad";
                    if (comByte[3] != 0x01) master_flag = "Bad";
                    if (comByte[4] != 0x00) master_flag = "Bad";
                    if (comByte[5] != 0x04) master_flag = "Bad";
                    if (comByte[6] != 0x08) master_flag = "Bad";
                }
                if (siz == 8)
                {
                    // port 1    0x01, 0x04, 0x14D8, 0x0002, CRC1, CRC2   // Temp
                    if (comByte[0] != 0x01) master_flag = "Bad";
                    if (comByte[1] != 0x04) master_flag = "Bad";
                    if (comByte[2] != 0x00) master_flag = "Bad";
                    if (comByte[3] != 0x01) master_flag = "Bad";
                    if (comByte[4] != 0x00) master_flag = "Bad";
                    if (comByte[5] != 0x04) master_flag = "Bad";
                }

                break;

                case "Port2": 
                    // port 2    0x01, 0x04, 0x14D8, 0x0002, CRC1, CRC2   // Temp
                    if (comByte[0] != 0x01) master_flag = "Bad";
                    if (comByte[1] != 0x04) master_flag = "Bad";
                    if (comByte[2] != 0x14) master_flag = "Bad";
                    if (comByte[3] != 0xD8) master_flag = "Bad";
                    if (comByte[4] != 0x00) master_flag = "Bad";
                    if (comByte[5] != 0x02) master_flag = "Bad";
                    break;



                }


            MyDataStructure2 data2 = new MyDataStructure2();
            data2 = _memCommand2.Get("Send");
            bool noResp = false;

            ack_msg     msg_ack_msg;
            data_resp   data_resp_msg;


            frameW++;
            //Output packet
            if (IsOpen)
                {
                if (master_flag == "Bad") comByte_inx = _mem[0].Get();
                test1++;
            switch (_PortName)
            {
                case "Port1":
                    comByte_inx = _mem[0].Get();


                    if (siz == 17) {
                        // Pack Packet
                        msg_ack_msg.addr    = 0x01;     // Address
                        msg_ack_msg.fc      = 0x10;     // Function Code
                        msg_ack_msg.staddr  = 0x0110;   // Data Length in Bytes
                        msg_ack_msg.Qor     = 0x0400;   // Data Length in Bytes
                        msg_ack_msg.crc1 = 0;           // CRC - Hi
                        msg_ack_msg.crc2 = 0;           // CRC - Low
                        buffer_out = CRC_msg_ack_msg(msg_ack_msg, data2.Port1_badCRC);

                    }
                    if (siz == 8)
                    {
                        // Pack Packet
                        data_resp_msg.addr  = 0x01;     // Address
                        data_resp_msg.fc    = 0x04;     // Function Code
                        data_resp_msg.staddr = 0x08;    // Data Length in Bytes
                        data_resp_msg.data1 = data2.Port1_b01;
                        data_resp_msg.data2 = data2.Port1_b02;
                        data_resp_msg.data3 = data2.Port1_b03;
                        data_resp_msg.data4 = data2.Port1_b04;
                        data_resp_msg.data5 = data2.Port1_b05;
                        data_resp_msg.data6 = data2.Port1_b06;
                        data_resp_msg.data7 = data2.Port1_b07;
                        data_resp_msg.data8 = data2.Port1_b08;
                        //data_resp_msg.data9 = data2.Port1_b09;
                        //data_resp_msg.data10= data2.Port1_b10;



                        data_resp_msg.crc1 = 0;      // CRC - Hi
                        data_resp_msg.crc2 = 0;      // CRC - Low
                        buffer_out = CRC_data_resp_msg(data_resp_msg, data2.Port1_badCRC);
                       

                    }
                    // Add CRC to Packet
                    

                    noResp = data2.Port1_noResp;
                        break;            
                    case "Port2":
                        comByte_inx = _mem[0].Get();
                        noResp = data2.Port2_noResp;
                        break;
                 
                    }   




            // Get NoResponse from SharedMemory
            str2 = ByteToHex(buffer_out);
            if (!noResp)
            {
                try
                {
                    //Console.Write("Send: 1(" + test1 + ") " + str2 + "\n");
                    if (siz == 17)_serialPort.Write(buffer_out, 0, 8);  // serial port write
                    if (siz == 8) _serialPort.Write(buffer_out, 0, 13);  // serial port write
                    str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread: _serialPort.Write\n");
                    Console.WriteLine(err);
                }
                if (master_flag == "Bad")
                    DataReceived(str + "# " + frameW + ") Tx: X " + str2);   // Write to GUI 
                else
                    DataReceived(str + "# " + frameW + ") Tx:   " + str2);   // Write to GUI 
            }
            MasterStatus(master_flag);  // Control LSD on GUI
            }
        }
 
}