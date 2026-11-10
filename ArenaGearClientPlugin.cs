using BepInEx;
using ArenaGearClient.Patches;

namespace ArenaGearClient;

[BepInPlugin("com.sage.ArenaGearClient", "Client Part Of Arena Gear", "1.0.0")]
public class ArenaGearClientPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        new Patch_BaseGrenadeHandsController_Cook().Enable();
        new Patch_ObservedPlayerGrenadeHandsController_HandleCook().Enable();
        new Patch_TripwireSynchronizableObject_HandleCook().Enable();
        new Patch_NetworkClientGrenadeFactory_CreateStillSmokeGrenade().Enable();
    }
}
