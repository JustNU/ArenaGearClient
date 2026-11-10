using System.Reflection;
using EFT;
using EFT.InventoryLogic;
using EFT.NextObservedPlayer;
using EFT.SynchronizableObjects;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace ArenaGearClient.Patches;

internal class Patch_BaseGrenadeHandsController_Cook : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(Player.BaseGrenadeHandsController), nameof(Player.BaseGrenadeHandsController.Cook));

    [PatchPostfix]
    private static void Postfix(Player.BaseGrenadeHandsController __instance)
    {
        SmokeTint.Apply(__instance.GrenadeEmission, __instance.Item?.TemplateId.ToString());
    }
}

internal class Patch_ObservedPlayerGrenadeHandsController_HandleCook : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(ObservedPlayerGrenadeHandsController), nameof(ObservedPlayerGrenadeHandsController.HandleCook));

    [PatchPostfix]
    private static void Postfix(GrenadeEmission ____grenadeEmission, ObservedPlayerHandsController ____handsController)
    {
        SmokeTint.Apply(____grenadeEmission, ____handsController?.ItemInHands?.TemplateId.ToString());
    }
}

internal class Patch_TripwireSynchronizableObject_HandleCook : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(TripwireSynchronizableObject), nameof(TripwireSynchronizableObject.HandleCook));

    [PatchPostfix]
    private static void Postfix(ThrowWeap item, GrenadeEmission ____grenadeEmission)
    {
        SmokeTint.Apply(____grenadeEmission, item?.TemplateId.ToString());
    }
}

internal class Patch_NetworkClientGrenadeFactory_CreateStillSmokeGrenade : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(NetworkClientGrenadeFactory), nameof(NetworkClientGrenadeFactory.CreateStillSmokeGrenade));

    [PatchPostfix]
    private static void Postfix(SmokeGrenade __result)
    {
        if (__result == null) return;
        SmokeTint.Apply(__result.GetComponentInChildren<GrenadeEmission>(), __result.WeaponSource?.TemplateId.ToString());
    }
}
