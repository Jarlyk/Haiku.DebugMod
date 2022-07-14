using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;
using UnityEngine.UI;

namespace Haiku.DebugMod
{
    public static class AbilityToggling
    {
        private static ItemDescriptionManager idmInstance;

        public static void InitHooks()
        {
            On.ItemDescriptionManager.Start += ItemDescriptionManager_Start; ;
            IL.ItemDescriptionManager.EnableAbility += ItemDescriptionManager_EnableAbility;
        }

        private static void ItemDescriptionManager_Start(On.ItemDescriptionManager.orig_Start orig, ItemDescriptionManager self)
        {
            orig(self);

            idmInstance = self;
            self.wallJump.onClick.AddListener(WallJumpOnClick);
            self.doubleJump.onClick.AddListener(DoubleJumpOnClick);
            self.teleport.onClick.AddListener(TeleportOnClick);
            self.roll.onClick.AddListener(RollOnClick);
            self.bomb.onClick.AddListener(BombOnClick);
            self.grapple.onClick.AddListener(GrappleOnClick);
            self.waterRes.onClick.AddListener(WaterResOnClick);
            self.fireRes.onClick.AddListener(FireResOnClick);
            self.lightBulb.onClick.AddListener(LightBulbOnClick);
        }

        private static void ItemDescriptionManager_EnableAbility(ILContext il)
        {
            var c = new ILCursor(il);

            //On handler is missing the Unity type reference for Button, so use IL instead to replace the method
            c.Emit(OpCodes.Ldarg_1);
            c.Emit(OpCodes.Ldarg_2);
            c.EmitDelegate((Action<Button, bool>)OnEnableAbility);
            c.Emit(OpCodes.Ret);
        }

        private static void OnEnableAbility(Button button, bool enabled)
        {
            //Button is always enabled with Debug active, to allow toggling
            button.interactable = true;
            var image = button.GetComponent<Image>();
            image.enabled = true;

            //To indicate whether present, we instead make image transparent
            image.color = enabled ? Color.white : new Color(1f, 1f, 1f, 0.3f);
        }

        private static void WallJumpOnClick()
        {
            GameManager.instance.canWallJump = !GameManager.instance.canWallJump;
            OnEnableAbility(idmInstance.wallJump, GameManager.instance.canWallJump);
        }

        private static void DoubleJumpOnClick()
        {
            GameManager.instance.canDoubleJump = !GameManager.instance.canDoubleJump;
            OnEnableAbility(idmInstance.doubleJump, GameManager.instance.canDoubleJump);
        }

        private static void TeleportOnClick()
        {
            GameManager.instance.canTeleport = !GameManager.instance.canTeleport;
            OnEnableAbility(idmInstance.teleport, GameManager.instance.canTeleport);
        }

        private static void RollOnClick()
        {
            GameManager.instance.canRoll = !GameManager.instance.canRoll;
            OnEnableAbility(idmInstance.roll, GameManager.instance.canRoll);
        }

        private static void BombOnClick()
        {
            GameManager.instance.canBomb = !GameManager.instance.canBomb;
            OnEnableAbility(idmInstance.bomb, GameManager.instance.canBomb);
        }

        private static void GrappleOnClick()
        {
            GameManager.instance.canGrapple = !GameManager.instance.canGrapple;
            OnEnableAbility(idmInstance.grapple, GameManager.instance.canGrapple);
        }

        private static void WaterResOnClick()
        {
            GameManager.instance.waterRes = !GameManager.instance.waterRes;
            OnEnableAbility(idmInstance.waterRes, GameManager.instance.waterRes);
        }

        private static void FireResOnClick()
        {
            GameManager.instance.fireRes = !GameManager.instance.fireRes;
            OnEnableAbility(idmInstance.fireRes, GameManager.instance.fireRes);
        }

        private static void LightBulbOnClick()
        {
            GameManager.instance.lightBulb = !GameManager.instance.lightBulb;
            OnEnableAbility(idmInstance.lightBulb, GameManager.instance.lightBulb);
        }
    }
}
