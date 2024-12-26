using System.Collections.Generic;

namespace Source.Libraries.KBLib2
{
    public static class Kb2List
    {
        public static List<T> Of<T>(params T[] pArgs)
        {
            return new List<T>(pArgs);
        }
    }
}