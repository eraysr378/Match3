using Factories.BaseFactories;
using UnityEngine;
using VisualEffects;

namespace Factories.VisualEffectFactory
{
    [CreateAssetMenu(fileName = "RainbowPieceLineRendererFactory", menuName = "Factories/VisualEffect/RainbowPieceLineRendererFactory")]

    public class RainbowPieceLineRendererFactory :  BaseVisualEffectFactory
    {
        public override bool CanCreateEffect(VisualEffectType effectType)
        {
            return effectType == VisualEffectType.RainbowPieceLineRenderer;
        }
    }
}