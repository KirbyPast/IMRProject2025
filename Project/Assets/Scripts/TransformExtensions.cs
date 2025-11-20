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

    public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        var parent = transform.parent;
        if (parent != null)
        {
            Vector3 parentScale = parent.lossyScale;
            transform.localScale = new Vector3(
                globalScale.x / parentScale.x,
                globalScale.y / parentScale.y,
                globalScale.z / parentScale.z
            );
        }
        else
        {
            transform.localScale = globalScale;
        }
    }

    public static void MatchWorldSizeTo(this Transform target, Transform reference)
    {
        var refR = reference.GetComponentInChildren<Renderer>();
        var tgtR = target.GetComponentInChildren<Renderer>();
        if (refR == null || tgtR == null) { Debug.LogWarning("Renderer missing"); return; }

        // World sizes
        Vector3 refSize = refR.bounds.size;
        Vector3 tgtSize = tgtR.bounds.size;

        // Avoid divide-by-zero
        Vector3 safe = new(Mathf.Max(tgtSize.x, 1e-6f),
                           Mathf.Max(tgtSize.y, 1e-6f),
                           Mathf.Max(tgtSize.z, 1e-6f));

        // How much we must multiply the *world* size
        Vector3 worldFactor = new(refSize.x / safe.x, refSize.y / safe.y, refSize.z / safe.z);

        // Convert world factor into localScale factor (account for parent scale)
        Vector3 parentWorld = target.parent ? target.parent.lossyScale : Vector3.one;
        target.localScale = new Vector3(
            target.localScale.x * worldFactor.x / parentWorld.x,
            target.localScale.y * worldFactor.y / parentWorld.y,
            target.localScale.z * worldFactor.z / parentWorld.z
        );
    }

    static bool TryGetWorldCorners(Transform t, out List<Vector3> corners)
    {
        corners = new List<Vector3>(8);
        MeshFilter mf = t.GetComponentInChildren<MeshFilter>();
        if (mf && mf.sharedMesh)
        {
            var b = mf.sharedMesh.bounds; // local-space AABB
            AddCornersLocalBox(t, b.center, b.extents, corners, mf.transform.localToWorldMatrix);
            return true;
        }

        SkinnedMeshRenderer smr = t.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr && smr.sharedMesh)
        {
            // Skinned meshes: local bounds may shift; still better than world AABB
            var b = smr.sharedMesh.bounds;
            AddCornersLocalBox(t, b.center, b.extents, corners, smr.transform.localToWorldMatrix);
            return true;
        }

        // Fallback: world AABB from Renderer (approximate if rotated)
        var r = t.GetComponentInChildren<Renderer>();
        if (r)
        {
            var wb = r.bounds;
            AddCornersWorldAABB(wb.center, wb.extents, corners);
            return true;
        }

        return false;
    }

    static void AddCornersLocalBox(Transform root, Vector3 c, Vector3 e, List<Vector3> outCorners, Matrix4x4 l2w)
    {
        outCorners.Clear();
        for (int xi = -1; xi <= 1; xi += 2)
            for (int yi = -1; yi <= 1; yi += 2)
                for (int zi = -1; zi <= 1; zi += 2)
                {
                    var local = c + Vector3.Scale(e, new Vector3(xi, yi, zi));
                    outCorners.Add(l2w.MultiplyPoint3x4(local));
                }
    }

    static void AddCornersWorldAABB(Vector3 c, Vector3 e, List<Vector3> outCorners)
    {
        outCorners.Clear();
        for (int xi = -1; xi <= 1; xi += 2)
            for (int yi = -1; yi <= 1; yi += 2)
                for (int zi = -1; zi <= 1; zi += 2)
                {
                    outCorners.Add(c + Vector3.Scale(e, new Vector3(xi, yi, zi)));
                }
    }

    /// Returns the world-space length along plane.right (xLen) and plane.up (yLen).
    public static bool GetProjectedSizeXY(Transform source, Transform plane, out float xLen, out float yLen)
    {
        xLen = yLen = 0f;
        if (!TryGetWorldCorners(source, out var corners)) return false;

        // Scalar coordinates along plane axes
        float minX = float.PositiveInfinity, maxX = float.NegativeInfinity;
        float minY = float.PositiveInfinity, maxY = float.NegativeInfinity;

        var ax = plane.right; // plane X
        var ay = plane.up;    // plane Y

        foreach (var p in corners)
        {
            float sx = Vector3.Dot(p, ax);
            float sy = Vector3.Dot(p, ay);
            if (sx < minX) minX = sx; if (sx > maxX) maxX = sx;
            if (sy < minY) minY = sy; if (sy > maxY) maxY = sy;
        }

        xLen = Mathf.Max(1e-6f, maxX - minX);
        yLen = Mathf.Max(1e-6f, maxY - minY);
        return true;
    }

    /// Scales target so its projected size on plane’s local X/Y matches desired (in world units).
    public static void MatchProjectionXY(Transform target, Transform plane, float desiredX, float desiredY)
    {
        if (!GetProjectedSizeXY(target, plane, out float curX, out float curY)) return;

        // Multiply local scale so the projected world size reaches desired size
        var ls = target.localScale;
        float fx = desiredX / curX;
        float fy = desiredY / curY;
        target.localScale = new Vector3(ls.x * fx, ls.y * fy, ls.z);
    }

    /// Sets target XY in plane’s local space to match source’s XY (keeping target’s local Z on that plane).
    public static void SnapXYOnPlane(Transform target, Transform plane, Transform source)
    {
        // Convert source world pos into plane-local
        var planeLocal = plane.InverseTransformPoint(source.position);
        var targetLocal = plane.InverseTransformPoint(target.position);
        targetLocal.x = planeLocal.x;
        targetLocal.y = planeLocal.y;
        // keep targetLocal.z (projection plane depth)
        target.position = plane.TransformPoint(targetLocal);
    }
    public static void ClampProjectionWithin(Transform projection, Transform highlight)
    {
        // Convert both to highlight local space
        Vector3 localPos = highlight.InverseTransformPoint(projection.position);

        var r = highlight.GetComponentInChildren<Renderer>();
        if (r == null) return;

        // Use half-size extents in local space
        Vector3 extents = highlight.InverseTransformVector(r.bounds.extents);
        Vector3 center = highlight.InverseTransformPoint(r.bounds.center);

        // Clamp X and Y to remain within highlight bounds
        localPos.x = Mathf.Clamp(localPos.x, center.x - extents.x, center.x + extents.x);
        localPos.y = Mathf.Clamp(localPos.y, center.y - extents.y, center.y + extents.y);

        // Convert back to world
        projection.position = highlight.TransformPoint(localPos);
    }
    public static void MatchProjectionXY_ByTrueSize(Transform target, Transform plane, float desiredX, float desiredY)
    {
        // What is the target's *current* size on the plane?
        if (!GetProjectedSizeXY(target, plane, out float curX, out float curY)) return;

        Vector3 ls = target.localScale;
        float fx = desiredX / Mathf.Max(curX, 1e-6f);
        float fy = desiredY / Mathf.Max(curY, 1e-6f);

        target.localScale = new Vector3(ls.x * fx, ls.y * fy, ls.z);
    }
    public static bool TryGetTrueXYSize(Transform board, out float width, out float height)
    {
        width = height = 0f;

        // Prefer MeshFilter; fall back to SkinnedMeshRenderer; otherwise Renderer AABB (approx)
        var mf = board.GetComponentInChildren<MeshFilter>();
        if (mf && mf.sharedMesh)
        {
            Vector3 sz = mf.sharedMesh.bounds.size;     // LOCAL space size
            Vector3 s = mf.transform.lossyScale;       // how local axes scale into world
            width = Mathf.Abs(sz.x * s.x);             // true physical width along local X
            height = Mathf.Abs(sz.y * s.y);             // true physical height along local Y
            return true;
        }

        var smr = board.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr && smr.sharedMesh)
        {
            Vector3 sz = smr.sharedMesh.bounds.size;
            Vector3 s = smr.transform.lossyScale;
            width = Mathf.Abs(sz.x * s.x);
            height = Mathf.Abs(sz.y * s.y);
            return true;
        }

        // Fallback (approx): use Renderer AABB but deprojection already handled elsewhere
        var r = board.GetComponentInChildren<Renderer>();
        if (r)
        {
            Vector3 sz = r.bounds.size;                 // already world size
            // We *assume* local XY corresponds roughly to world X/Y
            width = Mathf.Max(1e-6f, sz.x);
            height = Mathf.Max(1e-6f, sz.y);
            return true;
        }

        return false;
    }


}
