using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ALG_LAB2
{
    class WorkFiles
    {
        string _CitiesFile;
        string _RoadsFile;
        CitiesList _CitiesList;

        public WorkFiles(string filename)
        {
            _CitiesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename + ".Cities.dat");
            _RoadsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename + ".Roads.dat");
            _CitiesList = new CitiesList(_CitiesFile);

            using (var bw = new BinaryWriter(File.Create(_CitiesFile, 4096, FileOptions.RandomAccess)))
            {
                bw.Write(0);
                bw.Write(1);
                bw.Close();
            }

            using (var bw = new BinaryWriter(File.Create(_RoadsFile, 4096, FileOptions.RandomAccess)))
            {
                bw.Write(0);
                bw.Close();
            }
        }

        public string AddCity(string NameCity)
        {
            var newIDCity = _CitiesList.Add(NameCity);
            
            return "City with was successfuly added \n";
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

        public int CitysCount
        {
            get
            {
                using (var br = new BinaryReader(File.OpenRead(_CitiesFile)))
                    return br.ReadInt32();
            }
        }

        public int RoadsCount
        {
            get
            {
                using (var br = new BinaryReader(File.OpenRead(_RoadsFile)))
                    return br.ReadInt32();
            }
        }

        public string AddRoad(int c1, int c2, int d)
        {
            Road road = new Road();
            road.City1 =c1;
            road.City2 =c2;
            road.Distance =d;


            var sp1 = _CitiesList.CityRoads(road.City1);
            var sp2 = _CitiesList.CityRoads(road.City2);

         

            var rl1 = new RoadsList(_RoadsFile, road.City1, sp1);
            var rl2 = new RoadsList(_RoadsFile, road.City2, sp2);

            var p1 = rl1.AddRoad(road.City2, road.Distance, out sp1);

         

            rl2.UpdateLastRoadLink(p1);

            _CitiesList.UpdateRoadListPointer(road.City1, sp1);
            _CitiesList.UpdateRoadListPointer(road.City2, sp2 == RoadsList.EndOfList ? p1 : sp2);

            
            return "Road  was successfuly added!\n";
        }

        public string DeleteRoad(int c1, int c2)
        {
            Road road = new Road();
            road.City1 = c1;
            road.City2 = c2;

            int offset1 = _CitiesList.CityRoads(road.City1);
            int offset2 = _CitiesList.CityRoads(road.City2);


            RoadsList rl1 = new RoadsList(_RoadsFile, road.City1, offset1);
            RoadsList rl2 = new RoadsList(_RoadsFile, road.City2, offset2);

            if (rl1.Delete(road.City2, out offset1) && rl2.Delete(road.City1, out offset2))
            {
                _CitiesList.UpdateRoadListPointer(road.City1, offset1);
                _CitiesList.UpdateRoadListPointer(road.City2, offset2);
                
                return "Road Deleted!\n";
            }


            return ":(\n";
        }


        public string DeleteCity(int CityId)
        {
            var offset = _CitiesList.CityRoads(CityId);


            DeleteCityRoads(CityId);
            _CitiesList.Delete(CityId);
            return "City  was successfuly Deleted!\n";
        }

        private void DeleteCityRoads(int CityId)
        {
            var startPosition = _CitiesList.CityRoads(CityId);

            if (startPosition == RoadsList.EndOfList)
                return;

            while (startPosition != RoadsList.EndOfList)
            {
                int tX, tY;

                using (var br = new BinaryReader(File.OpenRead(_RoadsFile)))
                {
                    br.BaseStream.Seek(startPosition + 1, SeekOrigin.Begin);

                    tX = br.ReadInt32();
                    tY = br.ReadInt32();
                }

                DeleteRoad(tX, tY);
                startPosition = _CitiesList.CityRoads(CityId);
            }

            return;
        }

    }
}
