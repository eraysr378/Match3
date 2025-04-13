using VisualEffects;

namespace Factories.BaseFactories
{
    public abstract class BaseVisualEffectFactory : BasePoolableObjectFactory<BaseVisualEffect>
    {
        public abstract bool CanCreateEffect(VisualEffectType effectType);

    }
}