using UnityEngine;

namespace JumpMod
{
    internal class JumpModSettingsMainTemp
    {
        public int jumpHeightConst = 24;
        public int weightUpperLimit = 30;
        public int calorieCost = 15;
        public int staminaCost = 7;
        public float fatigueCost = 0.5f;
        public KeyCode jumpKeyCode = KeyCode.Space;
    }

    internal static class Settings
    {
        public static JumpModSettingsMainTemp options;

        public static void OnLoad()
        { 
            options = new JumpModSettingsMainTemp();
        }
    }
}
