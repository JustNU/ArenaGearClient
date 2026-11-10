using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using Systems.Effects;
using UnityEngine;

namespace ArenaGearClient;

internal static class SmokeTint
{
    private const string Rdg2EmissionKey = "weapon_rdg2_world";

    private static readonly int[] PropertyIds =
    {
        Shader.PropertyToID("_Color"),
        Shader.PropertyToID("_TintColor"),
        Shader.PropertyToID("_FireColor"),
        Shader.PropertyToID("_IndoorTintColor"),
        Shader.PropertyToID("_IndoorColor"),
        Shader.PropertyToID("_MinAmbientColor"),
    };

    private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

    private static readonly Dictionary<string, Color[]> Looks = new()
    {
        ["670fa9618f83118aed06c89b"] = new[]
        {
            new Color(1f, 1f, 1f, 1f),
            new Color(0.74f, 0.74f, 0.44f, 1f),
            new Color(1f, 0.5f, 0f, 1f),
            new Color(0.5f, 0.5f, 0.3f, 0.77f),
            new Color(1f, 1f, 1f, 1f),
            new Color(0.1f, 0.1f, 0.1f, 0.1f),
        },
    };

    private class Original
    {
        public Color[] Colors;
        public Texture MainTex;
    }

    private static readonly Dictionary<Material, Original> Originals = new();

    private static Texture _denseTexture;

    public static void Apply(GrenadeEmission emission, string templateId)
    {
        if (emission == null) return;

        Color[] look = null;
        bool colored = templateId != null && Looks.TryGetValue(templateId, out look);
        Texture dense = colored ? DenseTexture() : null;

        foreach (ParticleSystemRenderer renderer in emission.GetComponentsInChildren<ParticleSystemRenderer>(true))
        {
            Material material = renderer.material;
            if (material == null || !material.name.Contains("smokegreen")) continue;

            if (!Originals.TryGetValue(material, out Original original))
            {
                if (!colored) continue;
                original = new Original
                {
                    Colors = PropertyIds.Select(material.GetColor).ToArray(),
                    MainTex = material.GetTexture(MainTexId),
                };
                Originals[material] = original;
            }

            for (int i = 0; i < PropertyIds.Length; i++)
                material.SetColor(PropertyIds[i], colored ? look[i] : original.Colors[i]);

            Texture texture = colored && dense != null ? dense : original.MainTex;
            material.SetTexture(MainTexId, texture);

            if (WindowsManager.InstanceIsActive())
                WindowsManager.Instance.ParticlesManager?.UpdateParticleSystemMaterial(renderer.GetComponent<ParticleSystem>(), material);
        }
    }

    private static Texture DenseTexture()
    {
        if (_denseTexture != null) return _denseTexture;
        if (!Singleton<Effects>.Instantiated) return null;

        Effects.EmissionEffect rdg2 = Singleton<Effects>.Instance.EmissionEffects?.FirstOrDefault(e => e.Key == Rdg2EmissionKey);
        if (rdg2?.Instance == null) return null;

        _denseTexture = rdg2.Instance.GetComponentsInChildren<ParticleSystemRenderer>(true)
            .SelectMany(r => r.sharedMaterials)
            .FirstOrDefault(m => m != null && m.name.Contains("Dense"))
            ?.GetTexture(MainTexId);

        return _denseTexture;
    }
}
