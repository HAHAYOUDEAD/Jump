using Description = ModSettings.DescriptionAttribute;
using UnityEngine;
using ModSettings;
using System.ComponentModel;
using System.Reflection;

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

        [Section("Cheats for cheaters")]

        [Name("Do you really want to cheat?")]
        [Description("Think twice")]
        public bool cheat1 = false;


        [Name("Do you really want to ruin your 'Immersion™️'?")]
        [Description("Think one more time")]
        public bool cheat2 = false;

        [Name("Ignore all limitations and just jump")]
        [Description("You cheated not only the game, but yourself. You didn't grow. You didn't improve. You took a shortcut and gained nothing. You experienced a hollow jump. Nothing was risked and nothing was gained. It's sad that you don't know the difference.")]
        public bool jumpKing = false;

        protected override void OnConfirm()
        {
            base.OnConfirm();
        }

        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            if (field.Name == nameof(cheat1)) 
            {
                Settings.Cheat1((bool)newValue);
                base.OnConfirm();
            }
            if (field.Name == nameof(cheat2)) 
            {
                Settings.Cheat2((bool)newValue);
                base.OnConfirm();
            }
            

        }
    }

    internal static class Settings
    {
        public static JumpModSettingsMain options;

        public static void OnLoad()
        { 
            options = new JumpModSettingsMain();
            options.AddToModSettings("Jump Settings");

            Cheat1(options.jumpKing);
            Cheat2(options.jumpKing);
        }

        internal static void Cheat1(bool visible)
        {
            
            if (!visible)
            { 
                options.cheat2 = false; 
                options.jumpKing = false;
                options.SetFieldVisible(nameof(options.jumpKing), visible);
            }

            options.SetFieldVisible(nameof(options.cheat2), visible);

        }

        internal static void Cheat2(bool visible)
        {
            if (!visible)
            {
                options.jumpKing = false;
            }

            options.SetFieldVisible(nameof(options.jumpKing), visible);
        }
    }
}
