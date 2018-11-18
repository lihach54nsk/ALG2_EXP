using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALG_LAB2
{
    class City2
    {
        const int stringMaxLength = 40;

        public string Name { get; set; }

        public int NextCityIndex { get; set; }

        public void Write(BinaryWriter binaryWriter, int pointer)
        {
            binaryWriter.Seek(pointer, SeekOrigin.Begin);

            var tempStr = new char[stringMaxLength];
            for (int i = 0; i < Name.Length; i++)
            {
                tempStr[i] = Name[i];
            }

            binaryWriter.Write(tempStr);
            binaryWriter.Write(NextCityIndex);
        }

        public void Read(BinaryReader binaryReader, int pointer)
        {
            binaryReader.BaseStream.Seek(pointer, SeekOrigin.Begin);

            var tempString = binaryReader.ReadChars(stringMaxLength);

            Name = new string(tempString);
            NextCityIndex = binaryReader.ReadInt32();
        }

    }
}
