using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtention
{
    public static void DestroyAllChildren(this Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void SetActiveAllChilder(this Transform parent, bool state)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(state);
        }
    }

    public static Transform FindRecursive(this Transform transform, string n)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name.Trim() == n.Trim())
            {
                return child;
            }
        }
        return null;
    }

    public static void ActivateAllChildren(this Transform parent, bool state)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(state);
        }
    }
}
