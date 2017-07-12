using System;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.Threading;


// merge all SM to 1 shared memory


namespace Termie
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Class to keep track of string and color for lines in output window.
        /// </summary>
        private class Line
        {
            public string Str;
            public Color ForeColor;

            public Line(string str, Color color)
            {
                Str = str;
                ForeColor = color;
            }
        };

       // public static string systemPath = "C:\\" + "EMSim\\EMS5"; // your code goes here


        public static string home_dir = "EMSim";
        public static string app_dir = "NARSTester";
        public static string path_dir = home_dir + "\\" + app_dir;
        public static string systemPath = "C:\\" + home_dir + "\\" + app_dir; // your code goes here





        public static List<SharedMemory> sh1 = new List<SharedMemory>
                {
                new SharedMemory("Port1",path_dir)
                };

        public static List<SharedMemory> sh2 = new List<SharedMemory>
                {
                new SharedMemory("Port2",path_dir)
                };


        public static List<SharedMemory> sh7 = new List<SharedMemory>
                {
                new SharedMemory("Port71",path_dir),
                new SharedMemory("Port73",path_dir),
                new SharedMemory("Port75",path_dir),
                new SharedMemory("Port77",path_dir),
                new SharedMemory("Port79",path_dir),
                new SharedMemory("Port711",path_dir),
                new SharedMemory("Port713",path_dir),
                new SharedMemory("Port715",path_dir)
                };
        public static List<SharedMemory> sh8 = new List<SharedMemory>
                {
                new SharedMemory("Port82",path_dir),
                new SharedMemory("Port84",path_dir),
                new SharedMemory("Port86",path_dir),
                new SharedMemory("Port88",path_dir),
                new SharedMemory("Port810",path_dir),
                new SharedMemory("Port812",path_dir),
                new SharedMemory("Port814",path_dir),
                new SharedMemory("Port816",path_dir)
                };

        public Settings settings1 = new Settings("Port1", systemPath);
        public Settings settings2 = new Settings("Port2", systemPath);
      
        public Settings settings7 = new Settings("Port7", systemPath);
        public Settings settings8 = new Settings("Port8", systemPath);



        public CommPort com1 = new CommPort("Port1", sh1);
        public CommPort com2 = new CommPort("Port2", sh2);
     
        public CommPort com7 = new CommPort("Port7", sh7);
        public CommPort com8 = new CommPort("Port8", sh8);

        int Frame;
        int rate = 100;

        ArrayList lines = new ArrayList();

        Font origFont;
        Font monoFont;

        Stream myStream;
        const string FILENAME = "LargeFile.dat";
        SM2 memCommand2 = new SM2("shCommand2", path_dir);


        public Form1()
        {
            InitializeComponent();  // Initialize all widgets...
            // Create Shardem Memory Folder (systemPath) if does not exist
            System.IO.Directory.CreateDirectory(systemPath);
            //Create Shared Memory
            InitializeSharedMemory();

            //splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer2.FixedPanel = FixedPanel.Panel2;

            AcceptButton = button5; //Send
            CancelButton = button4; //Close
            // let form use multiple fonts
            origFont = Font;
            FontFamily ff = new FontFamily("Courier New");
            monoFont = new Font(ff, 8, FontStyle.Regular);

            // Product Directory

            bool exists = System.IO.Directory.Exists(systemPath);
            if (!exists) System.IO.Directory.CreateDirectory(systemPath);

            byte[] init = new byte[2];
            init[0] = 0;
            init[0] = 0;

            //Port 1
            settings1.Read("Port1");
            com1.StatusChanged += OnStatusChanged;
            com1.DataReceived += OnDataReceived;
            com1.DataWrite += OnDataWrite1;
            com1.MasterStatus += OnMasterStatus;
            port1Label.Text = settings1.port.PortName;
            com1.setup(memCommand2);



        }


        public void InitializeSharedMemory()
        {
            //MyDataStructure data = new MyDataStructure();
            //data = memCommand.Get();

            MyDataStructure2 data2 = new MyDataStructure2();
            data2 = memCommand2.Get("InitializeSharedMemory");

           //

            checkBoxBadCRC1.Checked = true;
           
        }

        /// <summary>
        /// output string to log file
        /// </summary>
        /// <param name="stringOut">string to output</param>
        public void logFile_writeLine1(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }
        public void logFile_writeLine2(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }
        public void logFile_writeLine3(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }
        public void logFile_writeLine4(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }
        public void logFile_writeLine5(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }
        public void logFile_writeLine6(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }
        public void logFile_writeLine7(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }
        public void logFile_writeLine8(string stringOut, string prt)
        {

            if (prt != "")
            {

                try
                {
                    myStream = File.Open(prt, FileMode.Append, FileAccess.Write, FileShare.Read);

                }
                catch (Exception err)
                {
                    Console.Write("Exception Thread:File.Open\n");
                    Console.WriteLine(err);
                }
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);

                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }

        #region Output window

        Color receivedColor = Color.Green;
        Color sentColor = Color.Blue;

        #endregion

        #region Event handling - OnDataReceived, OnStatusChanged, OnMasterStatus

        // delegate used for Invoke
        internal delegate void StringDelegate(string data);

        /// <summary>
        /// Handle data received event from serial port.
        /// </summary>
        /// <param name="data">incoming data</param>
        public void OnDataReceived(string dataIn)
        {
            //Handle multi-threading
            if (InvokeRequired)
            {
                Invoke(new StringDelegate(OnDataReceived), new object[] { dataIn });
                return;
            }
            int len1 = dataIn.Length;
            int len2 = dataIn.Length / 2;
            int tmp = dataIn.IndexOf("#") + 1;
            textAreaPort1write.Text = dataIn.Substring(tmp, len1 - tmp);
            textRx1.Text = dataIn.Substring(len1-tmp,len2-tmp);

            if (settings1.option.Logging) logFile_writeLine1(dataIn, settings1.option.LogFileName);
        }

      
 

        public void OnDataWrite1(string dataIn)
        {
            //Handle multi-threading
            if (InvokeRequired)
            {
                Invoke(new StringDelegate(OnDataWrite1), new object[] { dataIn });
                return;
            }
           
            int tmp = dataIn.IndexOf("#") + 1;
            int tmp2 = dataIn.IndexOf(")") + 1;

            Frame++;

            int end1 = dataIn.Length -tmp;
            int end2 = dataIn.Length / 2 ;

            if (Frame % rate == 0)
            {
                //string strN = dataIn.Substring(tmp, tmp2-tmp);
                textRx1.Text = dataIn.Substring(tmp, end2);
                textAreaPort1write.Text = dataIn.Substring(end2 + tmp, end2 - tmp);
                if (settings1.option.Logging) logFile_writeLine1(dataIn, settings1.option.LogFileName);
            }
        }



        /// <summary>
        /// Update the connection status
        /// </summary>  OnMasterStatus
        public void OnStatusChanged(string status)
        {
            //Handle multi-threading
            if (InvokeRequired)
            {
                Invoke(new StringDelegate(OnStatusChanged), new object[] { status });
                return;
            }
            string tmp = status.IndexOf("System.TimeoutException").ToString();
            if (tmp == "0") this.closePort1.BackColor = System.Drawing.Color.Red;

            textStatusPort1.Text = status;
            closePort1.Enabled = true;
        }


        public void OnMasterStatus(string status)
        {
            //Handle multi-threading
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new StringDelegate(OnMasterStatus), new object[] { status });
                    return;
                }
                catch (Exception err)
                {
                    Console.Write("OnMasterStatus: InvokeRequired\n");
                    Console.WriteLine(err);
                }


            }

            this.masterLED1.BackgroundImage = global::NARSTester.Properties.Resources.greenLED;
            if (status == "Bad") this.masterLED1.BackgroundImage = global::NARSTester.Properties.Resources.led_red_black;
        }

 
        #endregion 

        #region Port   settingPort1_Click,closePort1_Click
        // Close Port
        private void closePort1_Click(object sender, EventArgs e)
        {
            //updateTemp1_click_Click(sender, e);
            Frame = 0;

            if (com1.IsOpen)
            {
                

                closePort1.Enabled = false;
                this.closePort1.BackColor = System.Drawing.Color.LightGray;
                this.masterLED1.BackgroundImage = global::NARSTester.Properties.Resources.amberLED;

                if (settings1.option.Logging) logFile_writeLine1("Close Port1", settings1.option.LogFileName);
                //byte[] cmd1 = command1.Get();
                //cmd1[1] = 1;
                //command1.Set(cmd1);
                com1.Close();
            }
            else
            {
                this.closePort1.BackColor = System.Drawing.Color.Green;
                string status = com1.Open(settings1);
                if (!com1.IsOpen) this.closePort1.BackColor = System.Drawing.Color.Yellow;
                if (settings1.option.Logging) logFile_writeLine1("Start Port1", settings1.option.LogFileName);
            }


        }



        // Settings
        private void settingPort1_Click(object sender, EventArgs e)
        {



            TopMost = false;
            Form2 form1 = new Form2("Port1", settings1, com1);
            form1.ShowDialog();

            string p = form1._settings.port.Parity.ToString().Substring(0, 1);   //First char
            string h = form1._settings.port.Handshake.ToString();
            if (form1._settings.port.Handshake == Handshake.None)
                h = "no handshake"; // more descriptive than "None"

            textStatusPort1.Text = String.Format("{0}: {1} bps, {2}{3}{4}, {5}",
                    form1._settings.port.PortName, form1._settings.port.BaudRate,
                    form1._settings.port.DataBits, p, (int)form1._settings.port.StopBits, h);
            port1Label.Text = settings1.port.PortName;
        }


        #endregion

        #region Check Box Callbacks


        // No response
     
        // Bad CRC
        private void checkBoxBadCRC1_CheckedChanged(object sender, EventArgs e)
        {

            rate = 1;
            if (checkBoxBadCRC1.Checked) rate = 100;

        }



        #endregion

        #region About, Exit, Close X

        /// <summary>
        /// Show about dialog
        /// </summary>
        private void button_about_Click(object sender, EventArgs e)
        {
            TopMost = false;

            AboutBox about = new AboutBox();
            about.ShowDialog();


        }

        /// <summary>
        /// Close the application - EXIT
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {

            try {
                if (com1.IsOpen) com1.Close();
                if (com2.IsOpen) com2.Close();
               
                if (com7.IsOpen) com7.Close();
                if (com8.IsOpen) com8.Close();

                //Application.ExitThread();
                //base.OnClosed(e);
            }
            catch (Exception err)
            {
                Console.Write("Exception button4_Click  - Exit\n");
                Console.WriteLine(err);
            }

            Close();

        }
        // shutdown the worker thread when the form closes
        protected override void OnClosed(EventArgs e)
        {
            try
            {

                if (com1.IsOpen) com1.Close();
                if (com2.IsOpen) com2.Close();
        
                if (com7.IsOpen) com7.Close();
                if (com8.IsOpen) com8.Close();

                Application.ExitThread();
                base.OnClosed(e);
            }
            catch (Exception err)
            {
                Console.Write("Exception OnClosed\n");
                Console.WriteLine(err);
            }

        }




        #endregion

        #region Packets
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct bt
        {
            public byte addr;
            public byte fc;
            public byte len;
            public byte data1;
            public byte data2;
            public byte data3;
            public byte data4;
            public byte crc1;
            public byte crc2;
        }
        #endregion

        #region Data Inject Callbacks

       // bt msg;
        Boolean inject_failure1 = false;

        byte[] buffer_out = new byte[256];
        byte[] _in_temp_fl = new byte[4];
        byte[] _in_flow_fl = new byte[4];
        byte[] _inject_data2 = new byte[4];

        byte[] _in_temp_fl1 = new byte[4];
        byte[] _in_temp_fl2 = new byte[4];
        byte[] _in_temp_fl3 = new byte[4];
        byte[] _in_temp_fl4 = new byte[4];
        byte[] _in_temp_fl5 = new byte[4];
        byte[] _in_temp_fl6 = new byte[4];
        byte[] _in_temp_fl7 = new byte[4];
        byte[] _in_temp_fl8 = new byte[4];
        byte[] _in_temp_fl9 = new byte[4];
        byte[] _in_temp_fl10 = new byte[4];
        byte[] _in_temp_fl11 = new byte[4];
        byte[] _in_temp_fl12 = new byte[4];
        byte[] _in_temp_fl13 = new byte[4];
        byte[] _in_temp_fl14 = new byte[4];
        byte[] _in_temp_fl15 = new byte[4];
        byte[] _in_temp_fl16 = new byte[4];

        float[] f1 = new float[1];
        byte[] StructureToByteArray(bt obj)
        {
            int len = Marshal.SizeOf(obj);

            byte[] arr = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);

            Marshal.Copy(ptr, arr, 0, len);

            Marshal.FreeHGlobal(ptr);

            return arr;
        }
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

        private byte[] CRC(bt msg, bool badcrc)
        {
            byte data_s;
            data_s = 4;
            int len;
            ushort crc;

            ////////////////////////////////////
            //CRC
            len = 3 + data_s;
            crc = 0xFFFF;
            buffer_out = StructureToByteArray(msg);

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
            if (badcrc) inject_failure1 = true;
            if (!badcrc) inject_failure1 = false;




            if (!inject_failure1)
            {
                buffer_out[len + 0] = (byte)(crc & 0xFF);
                buffer_out[len + 1] = (byte)((crc >> 8) & 0xFF);
                len = len + 2;
            }
            else
            {
                buffer_out[len + 0] = 0;
                buffer_out[len + 1] = 0;
                len = len + 2;
            }
            ///////////   end of CRC  ///////////////
            return buffer_out;
        }



     

        /// <summary>
        /// 
        /// Table 28 - Data Message:  HPIB Strip Heater Response to CAMP Data Request 										
        //  8 Byte Message
        //  Lowest 2 Bytes:  Various Strip Heater Status…Bits 3 and 4 combined for a MODE, the rest are just bits for 7 statuses…7 Spare(Reg 0001)
        //  Next 2 Bytes:  Individual Bits of Strip Heater Failures  - 10 Used, 6 Spare(Reg 0002)
        //  Next 2 Bytes:  Fault Detection within Strip Heaters for Individual Sensors.  16 Used (Reg 0003)
        //  Last 2 Bytes:   Missile Tube Surface Overtemp.  1 = Overtemp, 0 = Nope.Tube 1 is Bit 1, Tube 16 is Bit 16.  (Reg 0004)

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //  Lowest 2 Bytes:  Various Strip Heater Status…Bits 3 and 4 combined for a MODE, the rest are just bits for 7 statuses…7 Spare(Reg 0001)
 
        




      

    
        #endregion

        #region Mouse Events  Up,Down
        private void updateTemp1_click_MouseDown(object sender, MouseEventArgs e)
        {
            //Console.Write("updateTemp1_click_MouseDown  DOWN\n");
            //this.updateTemp1_click.BackgroundImage = global::Termie.Properties.Resources.backArrow;
        }
        private void updateTemp1_click_MouseUp(object sender, MouseEventArgs e)
        {
            //Console.Write("updateTemp1_click_MouseDown  UP\n");
           // this.updateTemp1_click.BackgroundImage = global::Termie.Properties.Resources.update;
 
        }





        #endregion


        private void port3Label_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void textStatusPort4_TextChanged(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TempDataPort3_TextChanged(object sender, EventArgs e)
        {

        }

        private void TempDataPort6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void FlowDataPort3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textRx5_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TempDataPort1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textAreaPort1write_TextChanged(object sender, EventArgs e)
        {

        }

        private void TempDataHeater1_TextChanged(object sender, EventArgs e)
        {

        }
    }
   
}
