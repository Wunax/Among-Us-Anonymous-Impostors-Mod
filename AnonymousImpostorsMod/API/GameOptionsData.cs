using PlayerControl = FFGALNAPKCD;

namespace AnonymousImpostorsMod.API
{
    public static class GameOptionsData
    {
        public static float getKillDistance()
        {
            switch (PlayerControl.GameOptions.DLIBONBKPKL)
            {
                case 0: // SHORT
                    return 1.0f;
                case 1: // NORMAL
                    return 1.8f;
                case 2: // LONG
                    return 2.5f;
                default:
                    return 0f;
            }
        }
    }
}
