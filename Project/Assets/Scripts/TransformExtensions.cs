using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static Transform FindDeepChild(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            var result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }

    public static List<Transform> FindAllDeepChildren(this Transform parent, string name)
    {
        List<Transform> results = new();
        FindAllDeepChildrenRecursive(parent, name, results);
        return results;
    }

    private static void FindAllDeepChildrenRecursive(Transform parent, string name, List<Transform> results)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                results.Add(child);

            FindAllDeepChildrenRecursive(child, name, results);
        }
    }
}
