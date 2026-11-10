using BepInEx;
using ColoredSmokeGrenades.Patches;

namespace ColoredSmokeGrenades;

[BepInPlugin("com.sage.coloredsmokegrenades", "Colored Smoke Grenades", "1.0.0")]
public class ColoredSmokePlugin : BaseUnityPlugin
{
    private void Awake()
    {
        new Patch_BaseGrenadeHandsController_Cook().Enable();
        new Patch_ObservedPlayerGrenadeHandsController_HandleCook().Enable();
        new Patch_TripwireSynchronizableObject_HandleCook().Enable();
        new Patch_NetworkClientGrenadeFactory_CreateStillSmokeGrenade().Enable();
    }
}
