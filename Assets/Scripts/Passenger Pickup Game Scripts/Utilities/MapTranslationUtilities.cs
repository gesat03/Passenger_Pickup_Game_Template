using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public static class MapTranslationUtilities 
    {
        public static bool CheckV3AndRound(Vector3 input, Vector3 targetValue, float range)
        {
            bool isXInRange = CheckAndRoundComponent(input.x, targetValue.x, range);
            bool isYInRange = CheckAndRoundComponent(input.y, targetValue.y, range);
            bool isZInRange = CheckAndRoundComponent(input.z, targetValue.z, range);

            return isXInRange && isYInRange && isZInRange;
        }

        static bool CheckAndRoundComponent(float value, float targetValue, float range)
        {
            float difference = value - targetValue;
            if (difference >= -range && difference <= range)
            {
                return true;
            }
            return false;
        }

        public static Vector3 ConvertLocalToWorld(Vector3 localPosition, Transform referenceTransform)
        {
            return referenceTransform.TransformPoint(localPosition);
        }
    }
}
