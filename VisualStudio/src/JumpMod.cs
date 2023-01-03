using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace JumpMod
{
    public class JumpModMain : MelonMod
    {
        public static Transform playerTransform;
        public static bool doJump;
        public static bool showStaminaRed = false;

        public static object flashCoroutine;

        public override void OnApplicationStart()
        {  
            Settings.OnLoad();
        }

        public override void OnSceneWasInitialized(int level, string scene)
        {
            if (!IsNonGameScene()) doJump = true;
            else doJump = false;

        }

        [HarmonyPatch(typeof(Panel_HUD), "ShouldShowAlternateColor")]
        private class ShowStaminaRed
        {
            public static void Postfix(ref bool __result)
            {
                __result = showStaminaRed || __result;
            }
        }

        public static bool IsNonGameScene()
        {
            return string.IsNullOrEmpty(GameManager.m_ActiveScene) || GameManager.m_ActiveScene.Contains("MainMenu") || GameManager.m_ActiveScene == "Boot" || GameManager.m_ActiveScene == "Empty";
        }

        public static void ShowStaminaBar(bool show = true, bool flashRed = false, float duration = 0f)
        {
            InterfaceManager.GetPanel<Panel_HUD>().m_SprintBar.gameObject.SetActive(show);
            if (show)
            {
                if (duration < 0.01f) duration = InterfaceManager.GetPanel<Panel_HUD>().m_SprintBar_SecondsBeforeFadeOut;
                InterfaceManager.GetPanel<Panel_HUD>().m_SprintBar.alpha = 1f;
                InterfaceManager.GetPanel<Panel_HUD>().m_SprintFadeTimeTracker = duration;
                if (flashRed)
                {
                    if (flashCoroutine != null) MelonCoroutines.Stop(flashCoroutine);
                    flashCoroutine = MelonCoroutines.Start(FlashStaminaRed());
                }
            }
            else
            {
                InterfaceManager.GetPanel<Panel_HUD>().m_SprintFadeTimeTracker = 0f;
            }

        }

        public static IEnumerator FlashStaminaRed()
        {
            float shortDelay = 0.1f;
            float longDelay = 0.15f;
                    
            showStaminaRed = true;
            for (float t = 0f; t < longDelay; t += Time.deltaTime) yield return new WaitForEndOfFrame(); // change to WaitForSeconds when it works again
            showStaminaRed = false;
            for (float t = 0f; t < shortDelay; t += Time.deltaTime) yield return new WaitForEndOfFrame();
            showStaminaRed = true;
            for (float t = 0f; t < longDelay; t += Time.deltaTime) yield return new WaitForEndOfFrame();
            showStaminaRed = false;
            for (float t = 0f; t < shortDelay; t += Time.deltaTime) yield return new WaitForEndOfFrame();
            showStaminaRed = true;
            for (float t = 0f; t < longDelay; t += Time.deltaTime) yield return new WaitForEndOfFrame();
            showStaminaRed = false;
            yield break;
        }

        public override void OnUpdate()
        {
            if (doJump) JumpModActionMain.JumpModAction();
        }
    }
}