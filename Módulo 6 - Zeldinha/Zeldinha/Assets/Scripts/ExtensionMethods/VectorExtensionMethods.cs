using UnityEngine;

public static class VectorExtensionMethods {

    public static bool IsZero(this Vector2 v, float sqrEpsilon = Vector2.kEpsilon) {
        return v.sqrMagnitude <= sqrEpsilon;
    }

    public static bool IsZero(this Vector3 v, float sqrEpsilon = Vector3.kEpsilon) {
        return v.sqrMagnitude <= sqrEpsilon;
    }

}
