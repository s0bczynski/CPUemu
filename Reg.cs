using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUemu
{
    public static class Reg
    {
        public static byte AH { get; set; } = 0;
        public static byte BH { get; set; } = 0;
        public static byte CH { get; set; } = 0;
        public static byte DH { get; set; } = 0;

        public static byte AL { get; set; } = 0;
        public static byte BL { get; set; } = 0;
        public static byte CL { get; set; } = 0;
        public static byte DL { get; set; } = 0;

    }
}
