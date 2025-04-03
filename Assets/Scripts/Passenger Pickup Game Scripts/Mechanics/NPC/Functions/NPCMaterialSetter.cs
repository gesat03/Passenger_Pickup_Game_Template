using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class NPCMaterialSetter : MaterialSetter
    {

        [SerializeField] SkinnedMeshRenderer _meshRenderer;

        public void SetMaterial(EItemColor color)
        {
            _meshRenderer.material = GetMaterial(color);
        }

    }
}
