using UnityEngine;

public class ScaleNeutralizer
{
    public Transform CreateNeutralAnchor(Transform parent, string anchorName)
    {
        var anchor = new GameObject(anchorName).transform;
        anchor.SetParent(parent, false);

        Vector3 parentScale = parent.lossyScale;
        anchor.localScale = new Vector3(
            1f / parentScale.x,
            1f / parentScale.y,
            1f / parentScale.z);

        return anchor;
    }
}