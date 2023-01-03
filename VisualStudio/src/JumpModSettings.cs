using Description = ModSettings.DescriptionAttribute;
using UnityEngine;
using ModSettings;
using System.ComponentModel;

namespace JumpMod
{
    internal class JumpModSettingsMain : JsonModSettings
    {
        [Section("General settings")]

        [Name("Jump power")]
        [Description("Base = 24")]
        [Slider(15, 42)]
        public int jumpHeightConst = 24;

        [Name("Burden limit")]
        [Description("Used as threshold for jump height, with 80% jump height at threshold \n\nBase = 30")]
        [Slider(10, 50)]
        public int weightUpperLimit = 30;

        [Name("Calorie cost")]
        [Description("How much calories it should cost to jump once. Halved when low burden \n\nBase = 14")]
        [Slider(0, 50)]
        public int calorieCost = 14;

        [Name("Stamina cost")]
        [Description("How much stamina it should cost to jump once. This is in % from full stamina circle \n\nBase = 7")]
        [Slider(0, 20)]
        public int staminaCost = 7;

        [Name("Fatigue cost")]
        [Description("How much fatigue it should cost to jump once. This is in % from full fatigue circle \n\nBase = 0,5")]
        [Slider(0f, 10f, 101)]
        public float fatigueCost = 0.5f;

        [Section("Controls")]

        [Name("Jump")]
        [Description("Well ... jump.")]
        public KeyCode jumpKeyCode = KeyCode.Space;

        protected override void OnConfirm()
        {
            base.OnConfirm();
        }
    }

    internal static class Settings
    {
        public static JumpModSettingsMain options;

        public static void OnLoad()
        { 
            options = new JumpModSettingsMain();
            options.AddToModSettings("Jump Settings");
        }
    }
}
