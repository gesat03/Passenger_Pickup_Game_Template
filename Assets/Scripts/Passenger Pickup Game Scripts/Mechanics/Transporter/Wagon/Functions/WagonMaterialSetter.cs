using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class WagonMaterialSetter : MaterialSetter
    {
        [SerializeField] MeshRenderer _meshRenderer;

        public void SetMaterial(EItemColor color)
        {
            GetComponent<WagonData>().WagonColor = color;
            _meshRenderer.material = GetMaterial(color);
        }
    }
}
