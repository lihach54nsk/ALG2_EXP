using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALG_LAB2
{
    struct Road
    {
        public const int Size = 21;

        public int City1;
        public int City2;
        public int Distance;

        public Road(int IdCity1, int IdCity2, int distance =0)
        {
            City1 = IdCity1;
            City2 = IdCity2;
            Distance = distance;
        }
    }
}
