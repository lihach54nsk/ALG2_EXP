using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ALG_LAB2
{
    class RoadsList
    {
        public const int EndOfList = -100;
        public const int RoadNotFound = -2;
        public const int RoadConflict = -3;

        private string _roadsFile;
        private int _startPosition;
        private int IDCity;

        private int RoadsCount
        {
            get
            {
                using (var br = new BinaryReader(File.OpenRead(_roadsFile)))
                {
                    return br.ReadInt32();
                }
            }
        }

        public RoadsList(string filename, int townId, int startPosition)
        {
            _roadsFile = filename;
            IDCity = townId;
            _startPosition = startPosition;
        }

        public int FindRoad(int neighborTownId)
        {
            return FindRoad(neighborTownId, out var p, out var n);
        }

        public int FindRoad(int neighborTownId, out int previousRoad)
        {
            return FindRoad(neighborTownId, out previousRoad, out var n);
        }

        public int FindRoad(
            int neighborTownId,
            out int previousRoad,
            out int nextRoad
        )
        {
            previousRoad = RoadNotFound;
            nextRoad = EndOfList;

            if (_startPosition == EndOfList)
                return RoadNotFound;

            using (var br = new BinaryReader(File.OpenRead(_roadsFile)))
            {
                br.BaseStream.Seek(_startPosition + 1, SeekOrigin.Begin);

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    var townX = br.ReadInt32();
                    var townY = br.ReadInt32();
                    var distance = br.ReadInt32();
                    var nextX = br.ReadInt32();
                    var nextY = br.ReadInt32();
                    nextRoad = townX == IDCity ? nextX : nextY;

                    var currentTown = (int)br.BaseStream.Position - Road.Size;

                    if (townX == neighborTownId || townY == neighborTownId)
                        return currentTown;

                    previousRoad = currentTown + (townX == IDCity ? 13 : 17);

                    if (townX == IDCity && nextX == EndOfList
                        || townY == IDCity && nextY == EndOfList)
                        break;

                    br.BaseStream.Seek((townX == IDCity ? nextX : nextY) + 1, SeekOrigin.Begin);
                }
            }

            return RoadNotFound;
        }

        public Road GetRoad(int neighborTownId)
        {
            var offset = FindRoad(neighborTownId);

            if (offset == RoadNotFound)
                return default(Road);

            using (var br = new BinaryReader(File.OpenRead(_roadsFile)))
            {
                br.BaseStream.Seek(offset + 1, SeekOrigin.Begin);

                var tX = br.ReadInt32();
                var tY = br.ReadInt32();
                var d = br.ReadInt32();

                return new Road(tX, tY, d);
            }
        }

        public int AddRoad(int neighborTownId, int distance, out int startPosition)
        {
            startPosition = _startPosition;

            var road = FindRoad(neighborTownId, out var previousRoad);

            if (road != RoadNotFound)
                return RoadConflict;

            var position = FindPosition();

            Write(
                position,
                new Road(IDCity, neighborTownId, distance)
            );

            if (_startPosition == EndOfList)
                startPosition = _startPosition = position;

            if (previousRoad != RoadNotFound)
                UpdateLink(previousRoad, position);

            return position;
        }

        public void UpdateLastRoadLink(int newLink)
        {
            if (_startPosition == EndOfList)
                return;

            var road = FindRoad(-1, out var previousRoad);

            UpdateLink(previousRoad, newLink);
        }

        public bool Delete(int neighborTownId, out int startPosition)
        {
            startPosition = _startPosition;

            var road = FindRoad(
                neighborTownId,
                out var previousRoad,
                out var nextRoad
            );

            if (road == RoadNotFound)
                return false;

            var roadsCount = RoadsCount;

            using (var bw = new BinaryWriter(File.OpenWrite(_roadsFile)))
            {
                bw.Seek(road, SeekOrigin.Begin);
                bw.Write(true);

                bw.Seek(0, SeekOrigin.Begin);
                bw.Write(--roadsCount);

                if (previousRoad == RoadNotFound)
                {
                    startPosition = _startPosition = nextRoad;
                    return true;
                }
            }

            UpdateLink(previousRoad, nextRoad);

            return true; ;
        }

        private int FindPosition()
        {
            using (var br = new BinaryReader(File.OpenRead(_roadsFile)))
            {
                br.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
                while (br.BaseStream.Position < br.BaseStream.Length)
                    if (br.ReadBoolean())
                        return (int)br.BaseStream.Position - 1;
                    else
                        br.BaseStream.Seek(Road.Size - 1, SeekOrigin.Current);

                return (int)br.BaseStream.Position;
            }
        }

        private void Write(int position, Road road)
        {
            int roadsAmount;
            using (var br = new BinaryReader(File.OpenRead(_roadsFile)))
                roadsAmount = br.ReadInt32();

            using (var bw = new BinaryWriter(File.OpenWrite(_roadsFile)))
            {
                bw.Seek(position, SeekOrigin.Begin);
                bw.Write(false);
                bw.Write(road.City1);
                bw.Write(road.City2);
                bw.Write(road.Distance);
                bw.Write(RoadsList.EndOfList);
                bw.Write(RoadsList.EndOfList);

                bw.Seek(0, SeekOrigin.Begin);
                bw.Write(++roadsAmount);
            }
        }

        private void UpdateLink(int position, int newLink)
        {
            using (var bw = new BinaryWriter(File.OpenWrite(_roadsFile)))
            {
                bw.Seek(position, SeekOrigin.Begin);
                bw.Write(newLink);
            }
        }

    }
}
