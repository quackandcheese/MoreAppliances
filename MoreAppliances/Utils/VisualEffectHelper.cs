using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KitchenMoreAppliances.Utils
{
    public static class VisualEffectHelper
    {
        private static Dictionary<string, VisualEffectAsset> visualEffects = new Dictionary<string, VisualEffectAsset>();
        public static VisualEffect ApplyVisualEffect(this GameObject gameObject, string effectName)
        {
            visualEffects.TryGetValue(effectName, out var asset);
            var comp = gameObject.TryAddComponent<VisualEffect>();
            comp.visualEffectAsset = asset;
            return comp;
        }

        public static VisualEffectAsset GetVisualEffect(string name)
        {
            return visualEffects.TryGetValue(name, out var asset) ? asset : null;
        }

        public static void SetupEffectIndex()
        {
            if (visualEffects.Count > 0)
                return;
            foreach (VisualEffectAsset asset in Resources.FindObjectsOfTypeAll<VisualEffectAsset>())
            {
                if (!visualEffects.ContainsKey(asset.name))
                {
                    visualEffects.Add(asset.name, asset);
                }
            }
        }
    }
}
