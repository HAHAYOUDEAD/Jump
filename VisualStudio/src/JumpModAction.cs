using UnityEngine;
using MelonLoader;
using Il2Cpp;
using System.Collections;
using HarmonyLib;

namespace JumpMod
{
    public class JumpModActionMain : MelonMod
    {

        public static void JumpModAction()
        {
            float staminaLeewayPercent = 0.5f;

            bool flag = InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.jumpKeyCode) &&
                //GameManager.GetVpFPSPlayer().Controller.m_WasGroundedLastFrame &&
                GameManager.GetVpFPSCamera().Controller.isGrounded &&
                !uConsole.m_On &&
                !InterfaceManager.IsOverlayActiveCached() &&
                !GameManager.GetPlayerManagerComponent().PlayerIsCrouched();

            if (flag)
            {
                //MelonLogger.Msg("jump");

                float jumpHeight = Settings.options.jumpHeightConst / 100f;

                if (GameManager.GetPlayerManagerComponent().m_God) // ignore calculations if in god mode
                {
                    GameManager.GetVpFPSPlayer().Controller.MotorJumpForce = jumpHeight;
                    GameManager.GetVpFPSPlayer().Controller.Jump();
                    return;
                }

                //GameManager.GetHungerComponent().GetCalorieReserved() >= 250f && 
                bool flag2 = !GameManager.GetSprainedAnkleComponent().HasSprainedAnkle() && // no sprain
                     GameManager.GetHungerComponent().GetHungerLevel() != HungerLevel.Starving && // not starving
                     GameManager.GetInventoryComponent().GetTotalWeightKG() < Settings.options.weightUpperLimit + Settings.options.weightUpperLimit * 0.334f && // not overburdened
                     GameManager.GetThirstComponent().GetThirstLevel() != ThirstLevel.Dehydrated && // hydrated
                     GameManager.GetPlayerMovementComponent().GetSprintStamina() >= Settings.options.staminaCost * staminaLeewayPercent && // has stamina
                     GameManager.GetFatigueComponent().GetFatigueLevel() != FatigueLevel.Exhausted; // not exhausted

                if (flag2)
                {
                    float burdenPercent = GameManager.GetInventoryComponent().GetTotalWeightKG() / Settings.options.weightUpperLimit * 100f;
                    float jumpMult = 100f / (1f + Mathf.Pow(burdenPercent / 115f, 10f)) / 100f;
                    GameManager.GetVpFPSPlayer().Controller.MotorJumpForce = jumpHeight * jumpMult; // apply jump power

                    if (Settings.options.calorieCost > 0) // manage calorie loss
                    {
                        if (Mathf.Round(jumpMult * 100f) >= 100f) // 50% less calorie drain when low burden
                        {
                            GameManager.GetHungerComponent().RemoveReserveCalories(Mathf.RoundToInt(Settings.options.calorieCost * staminaLeewayPercent));
                        }
                        else
                        {
                            GameManager.GetHungerComponent().RemoveReserveCalories(Settings.options.calorieCost);
                        }
                    }

                    // deduct stamina
                    float currentStamina = GameManager.GetPlayerMovementComponent().GetSprintStamina();
                    if (currentStamina < Settings.options.staminaCost && currentStamina > Settings.options.staminaCost / 2f)
                    {
                        GameManager.GetPlayerMovementComponent().AddSprintStamina(-currentStamina);
                    }
                    else GameManager.GetPlayerMovementComponent().AddSprintStamina(-Settings.options.staminaCost);
                    JumpModMain.ShowStaminaBar();

                    //dedict fatigue
                    GameManager.GetFatigueComponent().AddFatigue(Settings.options.fatigueCost);

                    GameManager.GetVpFPSPlayer().Controller.Jump();
                    if (Utils.RollChance(5)) GameManager.GetPlayerVoiceComponent().Play("Play_VOLandLevel1", Il2CppVoice.Priority.Normal);
                }

                else
                {
                    if (GameManager.GetSprainedAnkleComponent().HasSprainedAnkle() == true)
                    {
                        HUDMessage.AddMessage("You can't jump when hurt", true, true);
                        return;
                    }

                    if (GameManager.GetInventoryComponent().GetTotalWeightKG() >= Settings.options.weightUpperLimit + Settings.options.weightUpperLimit * 0.334f)
                    {
                        HUDMessage.AddMessage("You can't jump when overencumbered", true, true);
                        return;
                    }

                    if (GameManager.GetFatigueComponent().GetFatigueLevel() == FatigueLevel.Exhausted)
                    {
                        HUDMessage.AddMessage("You can't jump when you're this tired", true, true);
                        return;
                    }


                    if (GameManager.GetThirstComponent().GetThirstLevel() == ThirstLevel.Dehydrated)
                    {
                        HUDMessage.AddMessage("You can't jump when you're this thirsty", true, true);
                        return;
                    }

                    if (GameManager.GetHungerComponent().GetHungerLevel() == HungerLevel.Starving)
                    {
                        HUDMessage.AddMessage("You can't jump when you're this hungry", true, true);
                        return;
                    }

                    if (GameManager.GetPlayerMovementComponent().GetSprintStamina() < Settings.options.staminaCost * staminaLeewayPercent)
                    {
                        JumpModMain.ShowStaminaBar(true, true, 0.5f);
                    }



                }
            }
        }
    }
}