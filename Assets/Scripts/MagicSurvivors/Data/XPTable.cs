using UnityEngine;

namespace MagicSurvivors.Data
{
    public static class XPTable
    {
        private static readonly int[] xpRequirements = new int[]
        {
            0,
            10,
            25,
            45,
            70,
            100,
            140,
            190,
            250,
            320,
            400,
            500,
            620,
            760,
            920,
            1100,
            1300,
            1520,
            1760,
            2020,
            2300
        };
        
        public static int GetXPRequiredForLevel(int level)
        {
            if (level < 1 || level >= xpRequirements.Length)
            {
                return xpRequirements[xpRequirements.Length - 1];
            }
            return xpRequirements[level];
        }
        
        public static int GetMaxLevel()
        {
            return xpRequirements.Length - 1;
        }
    }
}
