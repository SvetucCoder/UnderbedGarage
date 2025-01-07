using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;

[CustomEditor(typeof(Transform))] // Указываем, что редактор будет работать с CustomScript

public class MeshEditor : Editor
{
    public VisualTreeAsset VisualTree;

    private Vector3Field positionField;
    private Vector3Field rotationField;
    private Vector3Field scaleField;

    private Button positionBtn;
    private Button rotationBtn;
    private Button scaleBtn;

    private Button positionBtn2;
    private Button rotationBtn2;
    private Button scaleBtn2;

    private Button ResetpositionBtn;
    private Button ResetrotationBtn;
    private Button ResetscaleBtn;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        Transform transform = (Transform)target;
        
        VisualTree.CloneTree(root);
        positionField = root.Q<Vector3Field>("PositonVector");
        rotationField = root.Q<Vector3Field>("RotationVector");
        scaleField = root.Q<Vector3Field>("ScaleVector");

        positionBtn = root.Q<Button>("PositonBtn");
        rotationBtn = root.Q<Button>("RotationBtn");
        scaleBtn = root.Q<Button>("ScaleBtn");

        positionBtn.clicked += () => RoundPosition(transform);
        rotationBtn.clicked += () => RoundRotation(transform);
        scaleBtn.clicked += () => RoundScale(transform);

        ResetpositionBtn = root.Q<Button>("ResetPositonBtn");
        ResetrotationBtn = root.Q<Button>("ResetRotationBtn");
        ResetscaleBtn = root.Q<Button>("ResetScaleBtn");
        ResetpositionBtn.clicked += () => ResetPosition(transform);
        ResetrotationBtn.clicked += () => ResetRotation(transform);
        ResetscaleBtn.clicked += () => ResetScale(transform);

        positionBtn2 = root.Q<Button>("PositonBtn2");
        rotationBtn2 = root.Q<Button>("RotationBtn2");
        scaleBtn2 = root.Q<Button>("ScaleBtn2");

        positionBtn2.clicked += () => Round2Position(transform);
        rotationBtn2.clicked += () => Round2Rotation(transform);
        scaleBtn2.clicked += () => Round2Scale(transform);

        UpdateFields(transform);

        // Обработчики изменения значений
        positionField.RegisterValueChangedCallback(evt =>
        {
            if (transform.parent != null) transform.localPosition = evt.newValue;
            else transform.position = evt.newValue;
            EditorUtility.SetDirty(transform);
        });

        rotationField.RegisterValueChangedCallback(evt =>
        {
            transform.rotation = Quaternion.Euler(evt.newValue);
            EditorUtility.SetDirty(transform);
        });

        scaleField.RegisterValueChangedCallback(evt =>
        {
            transform.localScale = evt.newValue;
            EditorUtility.SetDirty(transform);
        });

        // Обновление полей при изменениях
        EditorApplication.update += () => UpdateFields(transform);

        return root;
    }

    private void UpdateFields(Transform transform)
    {
        if (positionField != null && rotationField != null && scaleField != null)
        {
            if (transform.parent != null) positionField.SetValueWithoutNotify(transform.localPosition);
            else positionField.SetValueWithoutNotify(transform.position);
            rotationField.SetValueWithoutNotify(transform.rotation.eulerAngles);
            scaleField.SetValueWithoutNotify(transform.localScale);
        }
    }

    private void RoundPosition(Transform transform)
    {
        if (transform.parent != null) transform.localPosition = new Vector3(
            Mathf.Round(transform.localPosition.x),
            Mathf.Round(transform.localPosition.y),
            Mathf.Round(transform.localPosition.z)
        );
        else transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );
        EditorUtility.SetDirty(transform);
    }

    private void RoundRotation(Transform transform)
    {
        transform.rotation = Quaternion.Euler(new Vector3(
            Mathf.Round(transform.rotation.eulerAngles.x),
            Mathf.Round(transform.rotation.eulerAngles.y),
            Mathf.Round(transform.rotation.eulerAngles.z)
        ));
        EditorUtility.SetDirty(transform);
    }

    private void RoundScale(Transform transform)
    {
        transform.localScale = new Vector3(
            Mathf.Round(transform.localScale.x),
            Mathf.Round(transform.localScale.y),
            Mathf.Round(transform.localScale.z)
        );
        EditorUtility.SetDirty(transform);
    }
    private void Round2Position(Transform transform)
    {
        if (transform.parent != null) transform.localPosition = new Vector3(
            RoundToTwoDecimalPlaces(transform.localPosition.x),
            RoundToTwoDecimalPlaces(transform.localPosition.y),
            RoundToTwoDecimalPlaces(transform.localPosition.z)
        );
        else transform.position = new Vector3(
            RoundToTwoDecimalPlaces(transform.position.x),
            RoundToTwoDecimalPlaces(transform.position.y),
            RoundToTwoDecimalPlaces(transform.position.z)
        );
        EditorUtility.SetDirty(transform);
    }

    private void Round2Rotation(Transform transform)
    {
        transform.rotation = Quaternion.Euler(new Vector3(
            RoundToTwoDecimalPlaces(transform.rotation.eulerAngles.x),
            RoundToTwoDecimalPlaces(transform.rotation.eulerAngles.y),
            RoundToTwoDecimalPlaces(transform.rotation.eulerAngles.z)
        ));
        EditorUtility.SetDirty(transform);
    }

    private void Round2Scale(Transform transform)
    {
        transform.localScale = new Vector3(
            RoundToTwoDecimalPlaces(transform.localScale.x),
            RoundToTwoDecimalPlaces(transform.localScale.y),
            RoundToTwoDecimalPlaces(transform.localScale.z)
        );
        EditorUtility.SetDirty(transform);
    }
    private void ResetPosition(Transform transform)
    {
        transform.position = new Vector3(
            0,
            0,
            0
        );
        EditorUtility.SetDirty(transform);
    }

    private void ResetRotation(Transform transform)
    {
        transform.rotation = Quaternion.Euler(new Vector3(
            0,
            0,
            0
        ));
        EditorUtility.SetDirty(transform);
    }

    private void ResetScale(Transform transform)
    {
        transform.localScale = new Vector3(
            0,
            0,
            0
        );
        EditorUtility.SetDirty(transform);
    }
    private float RoundToTwoDecimalPlaces(float value)
    {
        return Mathf.Round(value * 100f) / 100f; // Округление до 2 знаков после запятой
    }
}