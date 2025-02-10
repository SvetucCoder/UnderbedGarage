#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(AnimationCreator))]
public class AnimationCreatorEditor : Editor
{
    public VisualTreeAsset VisualTree;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        VisualTree.CloneTree(root);
        return root;
    }
}
#endif
