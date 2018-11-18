using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ALG_LAB2
{
    class CitiesList
    {
        public const int MaxNameCityLength = 32;
        public const int CityNotFound = -100500;
        public const int InvalidName = -100;

        private string _CitiesFile;

        public CitiesList(string file)
        {
            _CitiesFile = file;
        }

        public int Add(string NameCity)
        {
            if (NameCity.Length > MaxNameCityLength || NameCity.Length < 1)
                return InvalidName;

            return Write(NameCity);
        }


        private int Write(string NameCity)
        {
            int nextId = NextId;
            int position;
            int CitiesNumber;

            using (var br = new BinaryReader(File.OpenRead(_CitiesFile))){
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                CitiesNumber = br.ReadInt32();
            }

            var NameInBytes = UTF8Encoding.UTF8.GetBytes(NameCity);
            Array.Resize(ref NameInBytes, MaxNameCityLength);

            using (var br = new BinaryReader(File.OpenRead(_CitiesFile)))
            {
                br.BaseStream.Seek(2 * sizeof(int), SeekOrigin.Begin);
                while (br.BaseStream.Position < br.BaseStream.Length)
                    if (br.ReadBoolean())
                        return (int)br.BaseStream.Position - 1;
                    else
                        br.BaseStream.Seek(City.Size - 1, SeekOrigin.Current);

                 position = (int)br.BaseStream.Position;
            }
          
            using (var bw = new BinaryWriter(File.OpenWrite(_CitiesFile)))
            {
                bw.Seek(position, SeekOrigin.Begin);
                bw.Write(false);
                bw.Write(NameInBytes);
                bw.Write(nextId);
                bw.Write(RoadsList.EndOfList);

                bw.Seek(0, SeekOrigin.Begin);
                bw.Write(CitiesNumber + 1);
                bw.Write(++nextId);
            }

            return --nextId;
        }

        private int FindPosition()
        {
            using (var br = new BinaryReader(File.OpenRead(_CitiesFile)))
            {
                br.BaseStream.Seek(2 * sizeof(int), SeekOrigin.Begin);
                while (br.BaseStream.Position < br.BaseStream.Length)
                    if (br.ReadBoolean())
                        return (int)br.BaseStream.Position - 1;
                    else
                        br.BaseStream.Seek(City.Size - 1, SeekOrigin.Current);

                return (int)br.BaseStream.Position;
            }
        }

        private int NextId
        {
            get
            {
                using (var br = new BinaryReader(File.OpenRead(_CitiesFile)))
                {
                    br.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
                    return br.ReadInt32();
                }
            }
        }

        public int Find(int townId)
        {          

            using (var br = new BinaryReader(File.OpenRead(_CitiesFile)))
            {
                br.BaseStream.Seek(2 * sizeof(int), SeekOrigin.Begin);
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    if (br.ReadBoolean())
                    {
                        br.BaseStream.Seek(City.Size - 1, SeekOrigin.Current);
                        continue;
                    }

                    br.BaseStream.Seek(MaxNameCityLength, SeekOrigin.Current);

                    var id = br.ReadInt32();
                    if (id == townId)
                        return (int)br.BaseStream.Position - City.Size + 4;

                    br.BaseStream.Seek(4, SeekOrigin.Current);
                }

                return -5;
            }
        }

        public int CityRoads(int townId)
        {
            var town = Find(townId);

            

            using (var br = new BinaryReader(File.OpenRead(_CitiesFile)))
            {
                br.BaseStream.Seek(town + City.Size - 4, SeekOrigin.Begin);
                return br.ReadInt32();
            }
        }

        public void Delete(int townId)
        {
            var town = Find(townId);
            var townsCount = CitiesCount;

            using (var bw = new BinaryWriter(File.OpenWrite(_CitiesFile)))
            {
                bw.Seek(town, SeekOrigin.Begin);
                bw.Write(true);

                bw.Seek(0, SeekOrigin.Begin);
                bw.Write(--townsCount);
            }
        }

        public void UpdateRoadListPointer(int townId, int newPosition)
        {
            var positionInFile = Find(townId);
            using (var br = new BinaryWriter(File.OpenWrite(_CitiesFile)))
            {
                br.BaseStream.Seek(positionInFile + City.Size - sizeof(int), SeekOrigin.Begin);
                br.Write(newPosition);
            }
        }

        private int CitiesCount
        {
            get
            {
                using (var br = new BinaryReader(File.OpenRead(_CitiesFile)))
                {
                    br.BaseStream.Seek(0, SeekOrigin.Begin);
                    return br.ReadInt32();
                }
            }
        }
    }
}
