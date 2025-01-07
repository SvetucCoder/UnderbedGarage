using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[CustomEditor(typeof(AnimationCreator))] // ”казываем, что редактор будет работать с CustomScript
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