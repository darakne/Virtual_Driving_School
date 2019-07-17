using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace HD
{
    [System.Serializable]
    public class MyMessage
    {
        /*
         int phase;
         float steeringInput;
         float motorInput;
         bool breakInput;
         */
        //variables with get/set
        public int Phase { get; }
        public float SteeringInput { get;  }
        public float MotorInput { get;  }
        public bool BreakInput { get;  }

        public MyMessage(int new_phase)
        {
            Phase = new_phase;
            SteeringInput = 0;
            MotorInput = 0;
            BreakInput = false;
        }

        public MyMessage(int new_phase, float new_steeringInput, float new_motorInput, bool new_breakInput)
        {
            Phase = new_phase;
            SteeringInput = new_steeringInput;
            MotorInput = new_motorInput;
            BreakInput = new_breakInput;
        }
        

    }

    public static class MyMessageSerializer
    {

        //https://stackoverflow.com/questions/27663714/convert-class-object-to-bytes-and-create-object-from-bytes
        //http://www.java2s.com/Code/CSharp/File-Stream/ObjecttobytearrayserializationDeserialization.htm
        public static byte[] SerializeToBytes<MyMessage>(MyMessage source)
        {
            
               using (MemoryStream stream = new MemoryStream())
               {
                   BinaryFormatter formatter = new BinaryFormatter();
                   formatter.Serialize(stream, source);
                   return stream.ToArray();
               }
               
            
            /*
            var size = Marshal.SizeOf(source);
            // Both managed and unmanaged buffers required.
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            // Copy object byte-to-byte to unmanaged memory.
            Marshal.StructureToPtr(source, ptr, false);
            // Copy data from unmanaged memory to managed buffer.
            Marshal.Copy(ptr, bytes, 0, size);
            // Release unmanaged memory.
            Marshal.FreeHGlobal(ptr);

            return bytes;*/
        }

        //https://stackoverflow.com/questions/27663714/convert-class-object-to-bytes-and-create-object-from-bytes

        public static MyMessage DeserializeFromBytes<MyMessage>(byte[] source)
        {
            
            using (MemoryStream stream = new MemoryStream(source))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Write(source, 0, source.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return (MyMessage)formatter.Deserialize(stream);
            }
            
           /* var size = Marshal.SizeOf(source);
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            var your_object = (MyMessage)Marshal.PtrToStructure(ptr, typeof(MyMessage));
            Marshal.FreeHGlobal(ptr);

            return your_object;
            */
        }
    }
}