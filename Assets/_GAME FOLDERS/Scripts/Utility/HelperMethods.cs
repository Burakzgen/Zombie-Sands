using System.Collections;
using UnityEngine;

public static class HelperMethods
{
    public static IEnumerator DoAfterDelay(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    public static int GetInt(int v1, int v2, bool isIncludeV2 = false)
    {
        if (isIncludeV2)
            return UnityEngine.Random.Range(v1, v2 + 1);
        else
            return UnityEngine.Random.Range(v1, v2);
    }

    public static float GetFloat(float v1, float v2)
    {
        return UnityEngine.Random.Range(v1, v2);
    }
}
