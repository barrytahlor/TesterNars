using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public struct MyDataStructure2
{
    public bool Port_serial_status { get; set; }  //Serial port open/close status
    public float Port1_temp { get; set; }  //Water Heater temp Setpoint
    public float Port2_temp { get; set; }  //Water Heater temp Setpoint


    public bool Port1_badCRC { get; set; }
    public bool Port2_badCRC { get; set; }


    public bool Port1_noResp { get; set; }
    public bool Port2_noResp { get; set; }


    public byte Port1_b01 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b02 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b03 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b04 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b05 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b06 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b07 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b08 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b09 { get; set; }  //Water Heater temp Setpoint
    public byte Port1_b10 { get; set; }  //Water Heater temp Setpoint


    public byte Port2_b01 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b02 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b03 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b04 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b05 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b06 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b07 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b08 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b09 { get; set; }  //Water Heater temp Setpoint
    public byte Port2_b10 { get; set; }  //Water Heater temp Setpoint

}


public class SM2 : IDisposable
{
    //public readonly Mutex mget  = new Mutex();

    String _port;
    Mutex _mset;
    String _path;
    string _fileloc;
    MemoryMappedFile _memMapFile;
    MemoryMappedViewAccessor _accessor;
    static readonly object locker = new object();

    public SM2(String port,String path)
    {
        _port = port;
        _mset = new Mutex(false,_port);
        _path = path;
        _fileloc = @"C:\\" + _path + "\\SM2" + _port;
        _memMapFile = MemoryMappedFile.CreateFromFile(
            _fileloc, //file location
            FileMode.OpenOrCreate,  //create new file if not exist, open if exist
            "shared5_0-2"+port, //map name
            1024, MemoryMappedFileAccess.ReadWrite); //size
        _accessor = _memMapFile.CreateViewAccessor();
    }

    public void Set(MyDataStructure2 data, string trace)
    {
        //string str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
        //Console.Write(trace + ") before Set Mutex " + str + "\n");
        _mset.WaitOne();
        try
        {

                _accessor.Write<MyDataStructure2>(0, ref data);

        }
        finally
        {
            _mset.ReleaseMutex();
            //str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
            //Console.Write(trace + ") after Set Mutex " + str + "\n");
        }

    }


    public MyDataStructure2 Get(string trace)
    {
        MyDataStructure2 data;
        //Mutex mget = new Mutex();

        //string str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
        //Console.Write(trace + ") before Get Mutex " + str + "\n");
        _mset.WaitOne();
        try
        {

                _accessor.Read<MyDataStructure2>(0, out data);
                    //accessor.Dispose();

        }

        finally
        {
            _mset.ReleaseMutex();
            //str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
            //Console.Write(trace + ") after Get Mutex " + str + "\n");
        }


        return data;
    }


    public void Dispose()
    {

    }
}




