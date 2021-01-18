using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonymousImpostorsMod
{
    public static class CustomGameOptions
    {
        public enum customGameOptionsRpc : byte {
            impostorSoloWin = 61,
        }

        public static bool impostorSoloWin;
    }
}
