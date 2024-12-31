using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UIElements;


public class AnimationCreator : MonoBehaviour
{
    [SerializeField] public UnityEvent BasicEvent;
    [SerializeField] public UnityEvent ReverseEvent;
    [SerializeField] public UnityEvent BasicTriggerEvent;
    [SerializeField] public UnityEvent ReverseTriggerEvent;

    public bool Reverse;
    public bool EnableSound;
    public bool Playing;
    public bool Interactive;
    public bool EnableEvent;
    public bool EnableTrigger;
    public bool Toggle;
    public bool ToggleEvent;
    public bool ToggleTrigger;
    public bool Looping;
    public bool LoopingReverse;
    private Quaternion StartRotation;
    private Quaternion RealEndRotation;
    public bool isactivate = false;
    public bool isactivatetrigger = false;
    public string Tag;
    public void InteractEvent()
    {
        if (EnableEvent)
        {
            if (ToggleEvent)
            {
                if (isactivate)
                {
                    ExecuteClick();
                }
                else
                {
                    ExecuteReverseClick();
                }
                isactivate = !isactivate;
            }
            else
            {
                ExecuteClick();
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Tag)
        {
            if (EnableTrigger)
            {
                if (ToggleTrigger)
                {
                    if (isactivate)
                    {
                        ExecuteTriggerClick();
                    }
                    else
                    {
                        ExecuteTriggerReverseClick();
                    }
                    isactivatetrigger = !isactivatetrigger;
                }
                else
                {
                    ExecuteTriggerClick();
                }
            }
        }

    }
    public virtual void ExecuteClick()
    {
        UnityAPI.ExecuteUnityEvent(BasicEvent);
    }
    public virtual void ExecuteReverseClick()
    {
        UnityAPI.ExecuteUnityEvent(ReverseEvent);
    }
    public virtual void ExecuteTriggerClick()
    {
        UnityAPI.ExecuteUnityEvent(BasicTriggerEvent);
    }
    public virtual void ExecuteTriggerReverseClick()
    {
        UnityAPI.ExecuteUnityEvent(ReverseTriggerEvent);
    }

    private Vector3 StartSize;
    private Vector3 RealEndSize;

    private Vector3 StartPosition;
    private Vector3 RealEndPosition;

    public Vector3 EndRotation;
    public Vector3 EndSize;
    public Vector3 EndPosition;

    public GameObject Obj;
    bool playsound;
    public bool Smooth;
    public float RotationSpeed;
    public float SmoothRotationSpeed;
    public float SmoothPositionSpeed;
    public float PositionSpeed;
    public float SmoothScaleSpeed;
    public float ScaleSpeed;
    public int selectedTab = 0; // Переменная для хранения выбранной вкладки
    public int selectedTabMain = 0;

    public AudioSource audiosource;
    public AudioClip StartAudio;
    public AudioClip EndAudio;
    void Start()
    {
        if (Obj == null) Obj = gameObject;
        StartRotation = Obj.transform.rotation;
        StartSize = Obj.transform.localScale;
        StartPosition = Obj.transform.localPosition;
        
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

    public void Interact()
    {
        Playing = true;

        if (Interactive)
        {
            if (Toggle) Reverse = !Reverse;
            else
            {
                if (!Reverse)
                {
                    Obj.transform.rotation = StartRotation;
                }
                else
                {
                    Obj.transform.rotation = RealEndRotation;
                }
                StartRotation = Obj.transform.rotation;
                StartSize = Obj.transform.localScale;
                StartPosition = Obj.transform.localPosition;
            }
            playsound = false;
        }

    }
    void Update()
    {
        


        if (Playing)
        {
            if (EnableSound)
            {
                if (!playsound)
                {
                    if (!Reverse) audiosource.PlayOneShot(StartAudio);
                    else audiosource.PlayOneShot(EndAudio);
                }
                playsound = true;
            }


            RealEndRotation = StartRotation;
            RealEndRotation *= Quaternion.Euler(EndRotation.x, EndRotation.y, EndRotation.z);
            RealEndSize = EndSize + StartSize;
            RealEndPosition = EndPosition + StartPosition;

            if (!Reverse)
            {
                if (Smooth) Obj.transform.rotation = Quaternion.Slerp(Obj.transform.rotation, RealEndRotation, SmoothRotationSpeed * Time.deltaTime);
                else Obj.transform.rotation = Quaternion.RotateTowards(Obj.transform.rotation, RealEndRotation, RotationSpeed * Time.deltaTime);

                if (Smooth) Obj.transform.localPosition = Vector3.Lerp(Obj.transform.localPosition, RealEndPosition, SmoothPositionSpeed * Time.deltaTime);
                else Obj.transform.localPosition = Vector3.MoveTowards(Obj.transform.localPosition, RealEndPosition, PositionSpeed * Time.deltaTime);
                if (Smooth) Obj.transform.localScale = Vector3.Lerp(Obj.transform.localScale, RealEndSize, SmoothScaleSpeed * Time.deltaTime);
                else Obj.transform.localScale = Vector3.MoveTowards(Obj.transform.localScale, RealEndSize, ScaleSpeed * Time.deltaTime);


            }
            else
            {
                if (Smooth) Obj.transform.rotation = Quaternion.Slerp(Obj.transform.rotation, StartRotation, SmoothRotationSpeed * Time.deltaTime);
                else Obj.transform.rotation = Quaternion.RotateTowards(Obj.transform.rotation, StartRotation, RotationSpeed * Time.deltaTime);

                if (Smooth) Obj.transform.localPosition = Vector3.Lerp(Obj.transform.localPosition, StartPosition, SmoothPositionSpeed * Time.deltaTime);
                else Obj.transform.localPosition = Vector3.MoveTowards(Obj.transform.localPosition, StartPosition, PositionSpeed * Time.deltaTime);

                if (Smooth) Obj.transform.localScale = Vector3.Lerp(Obj.transform.localScale, StartSize, SmoothScaleSpeed * Time.deltaTime);
                else Obj.transform.localScale = Vector3.MoveTowards(Obj.transform.localScale, StartSize, ScaleSpeed * Time.deltaTime);
            }

            if (Looping)
            {
                if (LoopingReverse)
                {
                    if (!Reverse)
                    {
                        if (Obj.transform.rotation == RealEndRotation)
                        {
                            Reverse = true;
                        }
                    }
                    else
                    {
                        if (Obj.transform.rotation == StartRotation)
                        {
                            Reverse = false;
                        }
                    }
                }
                else
                {
                    if (!Reverse)
                    {
                        if (Obj.transform.rotation == RealEndRotation)
                        {
                            Obj.transform.rotation = StartRotation;
                        }
                    }
                    else
                    {
                        if (Obj.transform.rotation == StartRotation)
                        {
                            Obj.transform.rotation = RealEndRotation;
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
        int newTabMain = customScript.selectedTabMain;

        // Создаем кнопки для переключения вкладок
        GUILayout.BeginVertical();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Анимации"))
        {
            customScript.selectedTabMain = 0;
        }
        if (GUILayout.Button("Интерактив"))
        {
            customScript.selectedTabMain = 1;
        }
        if (GUILayout.Button("Триггеры"))
        {
            customScript.selectedTabMain = 2;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (customScript.selectedTabMain == 0)
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(200));
            if (GUILayout.Button("Настройки"))
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
            if (GUILayout.Button("Звук"))
            {
                customScript.selectedTab = 4;
            }
            if (GUILayout.Button("Взаимодействие"))
            {
                customScript.selectedTab = 5;
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();

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
                case 4:
                    ShowTab5(customScript);
                    break;
                case 5:
                    ShowTab6(customScript);
                    break;
            }
            GUILayout.EndVertical();
        }

        if (customScript.selectedTabMain == 1) 
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(200));
            if (GUILayout.Button("Настройки"))
            {
                customScript.selectedTab = 0;
            }
            if (GUILayout.Button("Ивенты"))
            {
                customScript.selectedTab = 1;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            switch (customScript.selectedTab)
            {
                case 0:
                    ShowInteractTab1(customScript);
                    break;
                case 1:
                    ShowInteractTab2(customScript);
                    break;
            }
            GUILayout.EndVertical();
        }
        if (customScript.selectedTabMain == 2)
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(200));
            if (GUILayout.Button("Настройки"))
            {
                customScript.selectedTab = 0;
            }
            if (GUILayout.Button("Ивенты"))
            {
                customScript.selectedTab = 1;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            switch (customScript.selectedTab)
            {
                case 0:
                    ShowTriggerTab1(customScript);
                    break;
                case 1:
                    ShowTriggerTab2(customScript);
                    break;
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        // Применяем изменения, если что-то было изменено
        if (GUI.changed)
        {
            EditorUtility.SetDirty(customScript);
        }

    }
    SerializedProperty onClickEventProperty;
    SerializedProperty onClickEventProperty2;
    SerializedProperty onClickEventProperty3;
    SerializedProperty onClickEventProperty4;
    private void OnEnable()
    {
        // Получаем ссылку на UnityEvent
        onClickEventProperty = serializedObject.FindProperty("BasicEvent");
        onClickEventProperty2 = serializedObject.FindProperty("ReverseEvent");
        onClickEventProperty3 = serializedObject.FindProperty("BasicTriggerEvent");
        onClickEventProperty4 = serializedObject.FindProperty("ReverseTriggerEvent");
    }

    private void ShowInteractTab1(AnimationCreator customScript)
    {
        customScript.EnableEvent = EditorGUILayout.Toggle("Enable", customScript.EnableEvent);
        customScript.ToggleEvent = EditorGUILayout.Toggle("Toggle", customScript.ToggleEvent);
        
    }
    private void ShowInteractTab2(AnimationCreator customScript)
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(onClickEventProperty, new GUIContent("Basic Event"));

        EditorGUILayout.PropertyField(onClickEventProperty2, new GUIContent("Reverse Event"));

        serializedObject.ApplyModifiedProperties();


    }
    private void ShowTriggerTab1(AnimationCreator customScript)
    {
        customScript.EnableTrigger = EditorGUILayout.Toggle("Enable", customScript.EnableTrigger);
        customScript.ToggleTrigger = EditorGUILayout.Toggle("Toggle", customScript.ToggleTrigger);
        customScript.Tag = EditorGUILayout.TagField("Tag", customScript.Tag);
    }
    private void ShowTriggerTab2(AnimationCreator customScript)
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(onClickEventProperty3, new GUIContent("Basic Event"));

        EditorGUILayout.PropertyField(onClickEventProperty4, new GUIContent("Reverse Event"));

        serializedObject.ApplyModifiedProperties();

         
    }
    // Функции для отображения содержимого вкладок
    private void ShowTab1(AnimationCreator customScript)
    {
        customScript.Playing = EditorGUILayout.Toggle("Play", customScript.Playing);
        customScript.Reverse = EditorGUILayout.Toggle("Reverse", customScript.Reverse);
        customScript.Looping = EditorGUILayout.Toggle("Loop", customScript.Looping);
        customScript.LoopingReverse = EditorGUILayout.Toggle("Loop Reverse", customScript.LoopingReverse);
        customScript.Smooth = EditorGUILayout.Toggle("Smooth", customScript.Smooth);
        customScript.Obj = (GameObject)EditorGUILayout.ObjectField("Обьект для Анимации", customScript.Obj, typeof(GameObject), true);
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
    private void ShowTab5(AnimationCreator customScript)
    {
        customScript.EnableSound = EditorGUILayout.Toggle("Enable Audio", customScript.EnableSound);
        customScript.audiosource = (AudioSource)EditorGUILayout.ObjectField("Проигрыватель", customScript.audiosource, typeof(AudioSource));
        customScript.StartAudio = (AudioClip)EditorGUILayout.ObjectField("Звук Открытия", customScript.StartAudio, typeof(AudioClip), false);
        customScript.EndAudio = (AudioClip)EditorGUILayout.ObjectField("Звук Закрытия", customScript.EndAudio, typeof(AudioClip), false);
    }
    private void ShowTab6(AnimationCreator customScript)
    {
        customScript.Interactive = EditorGUILayout.Toggle("Interactive", customScript.Interactive);
        customScript.Toggle = EditorGUILayout.Toggle("Toggle", customScript.Toggle);
        
    }
}
internal static class UnityAPI
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ExecuteUnityEvent(this UnityEvent unityEvent)
    {
        if (unityEvent != null)
        {
            unityEvent.Invoke();
        }
    }
}