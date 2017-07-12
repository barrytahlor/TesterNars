using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Termie
{


    public class SharedMemory : IDisposable
    {

        //public readonly Mutex mget  = new Mutex();

        String _port;
        Mutex _mset;
        String _path;
        string _fileloc;
        MemoryMappedFile _memMapFile;
        MemoryMappedViewStream _stream_reader, _stream_writer;
        BinaryWriter _writer;
        BinaryReader _reader;

        static readonly object locker = new object();


        public SharedMemory(String port,String path)
        {
            _port    = port;
            _mset    = new Mutex(false, _port);
            _path    = path;
            _fileloc = @"C:\\" + _path + "\\SharedMemory" + _port;
            //access memory mapped file (need persistence)
            _memMapFile = MemoryMappedFile.CreateFromFile(
                            _fileloc, //file location
                            FileMode.OpenOrCreate,  //create new file if not exist, open if exist
                            "shared5_0" + _port, //map name
                            1024); //size
            _stream_reader = _memMapFile.CreateViewStream();
            _stream_writer = _memMapFile.CreateViewStream();
            _writer = new BinaryWriter(_stream_writer);
            _reader = new BinaryReader(_stream_reader);
        }
      
        public void Set(byte[] comByte)
        {
            //string str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
            //Console.Write(trace + ") before Set Mutex " + str + "\n");
            _mset.WaitOne();
            try
            {

                            _writer.Write(comByte);
                

            }
            finally
            {
                _mset.ReleaseMutex();
                //str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
                //Console.Write(trace + ") after Set Mutex " + str + "\n");
            }
        }


        public byte[] Get()
        {

            byte[] comByte;
            //MyDataStructure2 data;
            //Mutex mget = new Mutex();

            //string str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
            //Console.Write(trace + ") before Get Mutex " + str + "\n");
            _mset.WaitOne();
            try
            {

                            comByte = _reader.ReadBytes(9);

            }

            finally
            {
                _mset.ReleaseMutex();
                //str = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fff");
                //Console.Write(trace + ") after Get Mutex " + str + "\n");
            }
            return comByte;
        }

        public void Dispose()
        {

        }
    }
}
