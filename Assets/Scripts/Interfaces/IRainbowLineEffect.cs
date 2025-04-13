using UnityEngine;

namespace Interfaces
{
    public interface IRainbowLineEffect
    {
         void SetUpAtEdge(Transform from, Transform to, float radius, float widthMultiplier); 
         void SetColor(Color color);


    }
}