using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public abstract class MaterialSetter : MonoBehaviour
    {
        [SerializeField] Material[] _materials;

        public Material GetMaterial(EItemColor color)
        {
            switch (color)
            {
                case EItemColor.Red:
                    return _materials[0];
                case EItemColor.Green:
                    return _materials[1];
                case EItemColor.Blue:
                    return _materials[2];
                case EItemColor.Yellow:
                    return _materials[3];
                case EItemColor.Purple:
                    return _materials[4];
                case EItemColor.Default:
                    if (_materials[5] != null) return _materials[5];
                    return _materials[0];
                default:
                    return _materials[0];
            }
        }
    }
}
