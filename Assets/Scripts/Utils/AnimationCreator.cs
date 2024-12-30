using UnityEditor;
using UnityEngine;

public class AnimationCreator : MonoBehaviour
{
    public bool Reverse;

    public bool Playing;
    public bool Looping;
    public bool LoopingReverse;
    private Quaternion StartRotation;
    private Quaternion RealEndRotation;


    private Vector3 StartSize;
    private Vector3 RealEndSize;

    private Vector3 StartPosition;
    private Vector3 RealEndPosition;

    public Vector3 EndRotation;
    public Vector3 EndSize;
    public Vector3 EndPosition;

    public bool Smooth;
    public float RotationSpeed;
    public float SmoothRotationSpeed;
    public float SmoothPositionSpeed;
    public float PositionSpeed;
    public float SmoothScaleSpeed;
    public float ScaleSpeed;
    public int selectedTab = 0; // Переменная для хранения выбранной вкладки
    void Start()
    {
       
        StartRotation = transform.rotation;
        StartSize = transform.localScale;
        StartPosition = transform.localPosition;
    }


    public void Play(bool ID)
    {
        Playing = ID;
    }
    public void Reversed(bool ID)
    {
        Reverse = ID;
    }
    public void Loop(bool ID)
    {
        Looping = ID;
    }
    public void LoopReversed(bool ID)
    {
        LoopingReverse = ID;
    }


    void Update()
    {



        if (Playing)
        {
            RealEndRotation = StartRotation;
            RealEndRotation *= Quaternion.Euler(EndRotation.x, EndRotation.y, EndRotation.z);
            RealEndSize = EndSize + StartSize;
            RealEndPosition = EndPosition + StartPosition;

            if (!Reverse)
            {
                if (Smooth) transform.rotation = Quaternion.Slerp(transform.rotation, RealEndRotation, SmoothRotationSpeed * Time.deltaTime);
                else transform.rotation = Quaternion.RotateTowards(transform.rotation, RealEndRotation, RotationSpeed * Time.deltaTime);

                if (Smooth) transform.localPosition = Vector3.Lerp(transform.localPosition, RealEndPosition, SmoothPositionSpeed * Time.deltaTime);
                else transform.localPosition = Vector3.MoveTowards(transform.localPosition, RealEndPosition, PositionSpeed * Time.deltaTime);
                if (Smooth) transform.localScale = Vector3.Lerp(transform.localScale, RealEndSize, SmoothScaleSpeed * Time.deltaTime);
                else transform.localScale = Vector3.MoveTowards(transform.localScale, RealEndSize, ScaleSpeed * Time.deltaTime);


            }
            else
            {
                if (Smooth) transform.rotation = Quaternion.Slerp(transform.rotation, StartRotation, SmoothRotationSpeed * Time.deltaTime);
                else transform.rotation = Quaternion.RotateTowards(transform.rotation, StartRotation, RotationSpeed * Time.deltaTime);

                if (Smooth) transform.localPosition = Vector3.Lerp(transform.localPosition, StartPosition, SmoothPositionSpeed * Time.deltaTime);
                else transform.localPosition = Vector3.MoveTowards(transform.localPosition, StartPosition, PositionSpeed * Time.deltaTime);

                if (Smooth) transform.localScale = Vector3.Lerp(transform.localScale, StartSize, SmoothScaleSpeed * Time.deltaTime);
                else transform.localScale = Vector3.MoveTowards(transform.localScale, StartSize, ScaleSpeed * Time.deltaTime);
            }

            if (Looping)
            {
                if (LoopingReverse)
                {
                    if (!Reverse)
                    {
                        if (transform.rotation == RealEndRotation)
                        {
                            Reverse = true;
                        }
                    }
                    else
                    {
                        if (transform.rotation == StartRotation)
                        {
                            Reverse = false;
                        }
                    }
                }
                else
                {
                    if (!Reverse)
                    {
                        if (transform.rotation == RealEndRotation)
                        {
                            transform.rotation = StartRotation;
                        }
                    }
                    else
                    {
                        if (transform.rotation == StartRotation)
                        {
                            transform.rotation = RealEndRotation;
                        }
                    }
                }
            }
        }
    }

}

[CustomEditor(typeof(AnimationCreator))] // Указываем, что редактор будет работать с CustomScript
public class CustomScriptEditor : Editor
{
    // Метод для отображения кастомного интерфейса
    public override void OnInspectorGUI()
    {
        AnimationCreator customScript = (AnimationCreator)target;

        // Заголовок
        EditorGUILayout.LabelField("Создание Анимации", EditorStyles.boldLabel);

        // Сохраняем старую вкладку
        int newTab = customScript.selectedTab;

        // Создаем кнопки для переключения вкладок
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Настройки Анимации"))
        {
            customScript.selectedTab = 0;
        }
        if (GUILayout.Button("Поворот"))
        {
            customScript.selectedTab = 1;
        }
        if (GUILayout.Button("Перемещение"))
        {
            customScript.selectedTab = 2;
        }
        if (GUILayout.Button("Размер"))
        {
            customScript.selectedTab = 3;
        }
        GUILayout.EndHorizontal();


        switch (customScript.selectedTab)
        {
            case 0:
                ShowTab1(customScript);
                break;
            case 1:
                ShowTab2(customScript);
                break;
            case 2:
                ShowTab3(customScript);
                break;
            case 3:
                ShowTab4(customScript);
                break;
        }

        // Применяем изменения, если что-то было изменено
        if (GUI.changed)
        {
            EditorUtility.SetDirty(customScript);
        }
    }

    // Функции для отображения содержимого вкладок
    private void ShowTab1(AnimationCreator customScript)
    {
        customScript.Playing = EditorGUILayout.Toggle("Play", customScript.Playing);
        customScript.Reverse = EditorGUILayout.Toggle("Reverse", customScript.Reverse);
        customScript.Looping = EditorGUILayout.Toggle("Loop", customScript.Looping);
        customScript.LoopingReverse = EditorGUILayout.Toggle("Loop Reverse", customScript.LoopingReverse);
        customScript.Smooth = EditorGUILayout.Toggle("Smooth", customScript.Smooth);
    }

    private void ShowTab2(AnimationCreator customScript)
    {
        customScript.EndRotation = EditorGUILayout.Vector3Field("Rotation", customScript.EndRotation);
        customScript.SmoothRotationSpeed = EditorGUILayout.FloatField("Smooth Speed", customScript.SmoothRotationSpeed);
        customScript.RotationSpeed = EditorGUILayout.FloatField("Speed", customScript.RotationSpeed);
    }

    private void ShowTab3(AnimationCreator customScript)
    {
        customScript.EndPosition = EditorGUILayout.Vector3Field("Position", customScript.EndPosition);
        customScript.SmoothPositionSpeed = EditorGUILayout.FloatField("Smooth Speed", customScript.SmoothPositionSpeed);
        customScript.PositionSpeed = EditorGUILayout.FloatField("Speed", customScript.PositionSpeed);
    }
    private void ShowTab4(AnimationCreator customScript)
    {
        customScript.EndSize = EditorGUILayout.Vector3Field("Size", customScript.EndSize);
        customScript.SmoothScaleSpeed = EditorGUILayout.FloatField("Smooth Speed", customScript.SmoothScaleSpeed);
        customScript.ScaleSpeed = EditorGUILayout.FloatField("Speed", customScript.ScaleSpeed);
    }
}