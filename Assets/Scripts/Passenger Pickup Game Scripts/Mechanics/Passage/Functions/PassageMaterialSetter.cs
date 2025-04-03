using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class PassageMaterialSetter : MaterialSetter
    {
        public MeshRenderer MeshRenderer;

        public EItemColor CurrentColor = EItemColor.Default;

        public void SetMaterial(EItemColor color)
        {
            if (CurrentColor == color)
                return;
            CurrentColor = color;
            MeshRenderer.material = GetMaterial(color);
        }
    }
}
