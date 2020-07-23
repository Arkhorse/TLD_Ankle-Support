﻿using Harmony;

namespace AnkleSupport
{
    [HarmonyPatch(typeof(GameManager), "Awake")]
    internal static class GameManager_Awake
    {
        private static void Postfix()
        {
            Implementation.Initialize();
        }
    }

    [HarmonyPatch(typeof(PlayerManager), "PutOnClothingItem")]
    internal static class PlayerManager_PutOnClothingItem
    {
        private static void Postfix(GearItem gi)
        {
            if (gi.m_ClothingItem != null && gi.m_ClothingItem.m_Region == ClothingRegion.Feet)
            {
                Implementation.UpdateAnkleSupport();
            }
        }
    }

    [HarmonyPatch(typeof(PlayerManager), "TakeOffClothingItem")]
    internal static class PlayerManager_TakeOffClothingItem
    {
        private static void Postfix(GearItem gi)
        {
            if (gi.m_ClothingItem != null && gi.m_ClothingItem.m_Region == ClothingRegion.Feet)
            {
                Implementation.UpdateAnkleSupport();
            }
        }
    }

    [HarmonyPatch(typeof(Sprains), "RollForSprainWhenMoving", new[] { typeof(float) })]
    internal static class Sprains_RollForSprainWhenMoving
    {
        private static void Prefix(Sprains __instance, ref float sprainChance) {
            if (Utils.IsZero(sprainChance)) return;

            if (Implementation.ShouldRollForWristSprain())
            {
                __instance.m_ChanceOfWristSprainWhenMoving = 100.0f;
            }
            else
            {
                Implementation.AdjustAnkleSprainMoveChance(ref sprainChance);
                __instance.m_ChanceOfWristSprainWhenMoving = 0.0f;
            }
        }
    }
}
