using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ALG_LAB2
{
    class Road2
    {
        public int CityFromPtr { get; set; }
        public int CityToPtr { get; set; }
        public int Distance { get; set; }

        public int NextRoadIndex { get; set; }


        public void Write(BinaryWriter binaryWriter, int pointer)
        {
            binaryWriter.Seek(pointer, SeekOrigin.Begin);
            binaryWriter.Write(CityFromPtr);
            binaryWriter.Write(CityToPtr);
            binaryWriter.Write(Distance);
            binaryWriter.Write(NextRoadIndex);
        }

        public void Read(BinaryReader binaryReader, int pointer)
        {
            binaryReader.BaseStream.Seek(pointer, SeekOrigin.Begin);
            CityFromPtr = binaryReader.ReadInt32();
            CityToPtr = binaryReader.ReadInt32();
            Distance = binaryReader.ReadInt32();
            NextRoadIndex = binaryReader.ReadInt32();
        }
    }
}
