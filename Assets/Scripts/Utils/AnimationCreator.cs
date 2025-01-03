using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

[System.Flags]
public enum Direction
{
    None = 0,       // Рекомендуется добавить значение "None" для пустого состояния
    Forward = 1 << 0, // 1
    Backward = 1 << 1, // 2
    Left = 1 << 2,     // 4
    Right = 1 << 3,    // 8
    Up = 1 << 4,       // 16
    Down = 1 << 5      // 32
} 

public class AnimationCreator : MonoBehaviour
{

    [SerializeField] public UnityEvent BasicEvent;
    [SerializeField] public UnityEvent ReverseEvent;
    [SerializeField] public UnityEvent BasicTriggerEvent;
    [SerializeField] public UnityEvent ReverseTriggerEvent;

    //Play
    public bool Playing;
    public bool Reverse;
    public bool Smooth;

    //Interactive
    public bool Toggle;
    public bool Interactive;
    public bool EnableSound;

    //Trigger
    public bool EnableTrigger;
    public string Tag;

    //Animation
    public Transform AnimationObject;

    public Vector3 AnimationRotation;
    private Vector3 AnimationRotationUsed;
    public float RotationSpeed;

    public Vector3 AnimationMove;
    private Vector3 AnimationMoveUsed;
    public float MoveSpeed;
    public bool GlobalMove = true;

    public Vector3 AnimationScale;
    private Vector3 AnimationScaleUsed;
    public float ScaleSpeed;
    //Audio
    public AudioSource AudioPlayer;
    public AudioClip BasicAudio;
    public AudioClip ReverseAudio;

    //Rigidbody
    public Rigidbody RigidbodyObject;
    public bool ToggleGravity;
    public bool BasicGravity;
    public float Mass;
    public float Force;
    public Direction BasicDirection;
    public Direction ReverseDirection;
    public Vector3 direction;

    public int selectedTab = 0;
    public int selectedTabMain = 0;

    public bool ChangeMaterial;
    public Material BaseMaterial;
    public Material ReverseMaterial;
    public int MaterialID;
    public Renderer[] RenderObjects;

    void Start()
    {
        if (AnimationObject == null) AnimationObject = gameObject.transform;
    }
    

    public void ChangeDirection(Direction direct)
    {
        if(direct == Direction.Forward)
        {
            direction = Vector3.forward;
        }
        else if (direct == Direction.Backward)
        {
            direction = Vector3.back;
        }
        else if (direct == Direction.Left)
        {
            direction = Vector3.left;
        }
        else if (direct == Direction.Right)
        {
            direction = Vector3.right;
        }
        else if (direct == Direction.Up)
        {
            direction = Vector3.up;
        }
        else if (direct == Direction.Down)
        {
            direction = Vector3.down;
        }
    }
    public void BasicPlay()
    {
        if (ChangeMaterial)
        {
            for(int i = 0; i < RenderObjects.Length; i++)
            {
                List<Material> mat = new List<Material>();
                RenderObjects[i].GetMaterials(mat);
                mat[MaterialID] = BaseMaterial;
                RenderObjects[i].SetMaterials(mat);
            }
        }
        if (ToggleGravity)
        {
            RigidbodyObject.useGravity = BasicGravity;
        }
        ChangeDirection(BasicDirection);
        ExecuteClick();
        if (EnableSound) AudioPlayer.PlayOneShot(BasicAudio);
        if (RigidbodyObject != null) RigidbodyObject.AddForce(direction * Force, ForceMode.Impulse);

    }
    public void ReversePlay()
    {
        if (ChangeMaterial)
        {
            for (int i = 0; i < RenderObjects.Length; i++)
            {
                List<Material> mat = new List<Material>();
                RenderObjects[i].GetMaterials(mat);
                mat[MaterialID] = BaseMaterial;
                RenderObjects[i].SetMaterials(mat);
            }
        }
        if (ToggleGravity)
        {
            RigidbodyObject.useGravity = !BasicGravity;
        }
        ChangeDirection(ReverseDirection);
        ExecuteReverseClick();
        if (EnableSound) AudioPlayer.PlayOneShot(ReverseAudio);
        if(RigidbodyObject != null) RigidbodyObject.AddForce(direction * Force, ForceMode.Impulse);
    }
    public void Interact()
    {
        if (Interactive)
        {
            if (!Playing) Playing = true;
            else if (Toggle) Reverse = !Reverse;

            AnimationRotationUsed = AnimationRotation - AnimationRotationUsed;
            AnimationMoveUsed = AnimationMove - AnimationMoveUsed;
            AnimationScaleUsed = AnimationScale - AnimationScaleUsed;
            
            if (!Reverse) BasicPlay();
            else ReversePlay();

        }
    }
    public void BasicAnimationMovement()
    {
        if (Smooth)
        {
            AnimationObject.rotation = Quaternion.Slerp(AnimationObject.rotation, AnimationObject.rotation * Quaternion.Euler(AnimationRotationUsed.x, AnimationRotationUsed.y, AnimationRotationUsed.z), RotationSpeed * Time.deltaTime);
            AnimationObject.localScale = Vector3.Lerp(AnimationObject.localScale, AnimationObject.localScale + AnimationScaleUsed, ScaleSpeed * Time.deltaTime);
            if (GlobalMove)
            {
                AnimationObject.position = Vector3.Lerp(AnimationObject.position, AnimationObject.position + AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }
            else
            {
                AnimationObject.localPosition = Vector3.Lerp(AnimationObject.localPosition, AnimationObject.localPosition + AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }
            AnimationRotationUsed = Vector3.Lerp(AnimationRotationUsed, new Vector3(0, 0, 0), RotationSpeed * Time.deltaTime);
            AnimationMoveUsed = Vector3.Lerp(AnimationMoveUsed, new Vector3(0, 0, 0), MoveSpeed * Time.deltaTime);
            AnimationScaleUsed = Vector3.Lerp(AnimationScaleUsed, new Vector3(0, 0, 0), ScaleSpeed * Time.deltaTime);
        }
        else
        {
            AnimationObject.rotation = Quaternion.RotateTowards(AnimationObject.rotation, AnimationObject.rotation * Quaternion.Euler(AnimationRotationUsed.x, AnimationRotationUsed.y, AnimationRotationUsed.z), RotationSpeed * Time.deltaTime);
            AnimationObject.localScale = Vector3.MoveTowards(AnimationObject.localScale, AnimationObject.localScale + AnimationScaleUsed, ScaleSpeed * Time.deltaTime);
            if (GlobalMove)
            {
                AnimationObject.position = Vector3.MoveTowards(AnimationObject.position, AnimationObject.position + AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }
            else
            {
                AnimationObject.localPosition = Vector3.MoveTowards(AnimationObject.localPosition, AnimationObject.localPosition + AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }
            AnimationRotationUsed = Vector3.MoveTowards(AnimationRotationUsed, new Vector3(0, 0, 0), RotationSpeed * Time.deltaTime);
            AnimationMoveUsed = Vector3.MoveTowards(AnimationMoveUsed, new Vector3(0, 0, 0), MoveSpeed * Time.deltaTime);
            AnimationScaleUsed = Vector3.MoveTowards(AnimationScaleUsed, new Vector3(0, 0, 0), ScaleSpeed * Time.deltaTime);
        }
    }
    public void ReverseAnimationMovement()
    {
        if (Smooth)
        {
            AnimationObject.rotation = Quaternion.Slerp(AnimationObject.rotation, AnimationObject.rotation * Quaternion.Euler(-AnimationRotationUsed.x, -AnimationRotationUsed.y, -AnimationRotationUsed.z), RotationSpeed * Time.deltaTime);
            AnimationObject.localScale = Vector3.Lerp(AnimationObject.localScale, AnimationObject.localScale - AnimationScaleUsed, ScaleSpeed * Time.deltaTime);
            if (GlobalMove)
            {
                AnimationObject.position = Vector3.Lerp(AnimationObject.position, AnimationObject.position - AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }
            else
            {
                AnimationObject.localPosition = Vector3.Lerp(AnimationObject.localPosition, AnimationObject.localPosition - AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }

            AnimationRotationUsed = Vector3.Lerp(AnimationRotationUsed, new Vector3(0, 0, 0), RotationSpeed * Time.deltaTime);
            AnimationMoveUsed = Vector3.Lerp(AnimationMoveUsed, new Vector3(0, 0, 0), MoveSpeed * Time.deltaTime);
            AnimationScaleUsed = Vector3.Lerp(AnimationScaleUsed, new Vector3(0, 0, 0), ScaleSpeed * Time.deltaTime);
        }
        else
        {
            AnimationObject.rotation = Quaternion.RotateTowards(AnimationObject.rotation, AnimationObject.rotation * Quaternion.Euler(-AnimationRotationUsed.x, -AnimationRotationUsed.y, -AnimationRotationUsed.z), RotationSpeed * Time.deltaTime);
            AnimationObject.localScale = Vector3.MoveTowards(AnimationObject.localScale, AnimationObject.localScale - AnimationScaleUsed, ScaleSpeed * Time.deltaTime);
            if (GlobalMove)
            {
                AnimationObject.position = Vector3.MoveTowards(AnimationObject.position, AnimationObject.position - AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }
            else
            {
                AnimationObject.localPosition = Vector3.MoveTowards(AnimationObject.localPosition, AnimationObject.localPosition - AnimationMoveUsed, MoveSpeed * Time.deltaTime);
            }

            AnimationRotationUsed = Vector3.MoveTowards(AnimationRotationUsed, new Vector3(0, 0, 0), RotationSpeed * Time.deltaTime);
            AnimationMoveUsed = Vector3.MoveTowards(AnimationMoveUsed, new Vector3(0, 0, 0), MoveSpeed * Time.deltaTime);
            AnimationScaleUsed = Vector3.MoveTowards(AnimationScaleUsed, new Vector3(0, 0, 0), ScaleSpeed * Time.deltaTime);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tag)
        {
            if (EnableTrigger)
            {
                if (Toggle)
                {
                    if (Reverse)
                    {
                        ExecuteTriggerClick();
                    }
                    else
                    {
                        ExecuteTriggerReverseClick();
                    }
                    Reverse = !Reverse;
                }
                else
                {
                    ExecuteTriggerClick();
                }
            }
        }
    }
    void Update()
    {
        if (Playing)
        {
            if (!Reverse) BasicAnimationMovement();
            else ReverseAnimationMovement();
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
        if (GUILayout.Button("Animation"))
        {
            customScript.selectedTabMain = 0;
        }
        if (GUILayout.Button("Interaction"))
        {
            customScript.selectedTabMain = 1;
        }
        if (GUILayout.Button("Trigger"))
        {
            customScript.selectedTabMain = 2;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (customScript.selectedTabMain == 0)
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(200));
            if (GUILayout.Button("Settings"))
            {
                customScript.selectedTab = 0;
            }
            if (GUILayout.Button("Transform Animation"))
            {
                customScript.selectedTab = 1;
            }
            if (GUILayout.Button("Rigidbody Animation"))
            {
                customScript.selectedTab = 2;
            }
            if (GUILayout.Button("Materials"))
            {
                customScript.selectedTab = 3;
            }
            if (GUILayout.Button("Sound"))
            {
                customScript.selectedTab = 4;
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
             
            switch (customScript.selectedTab)
            {
                case 0:
                    ShowTab(customScript);
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
            }
            GUILayout.EndVertical();
        }

        if (customScript.selectedTabMain == 1) 
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(200));
            if (GUILayout.Button("Settings"))
            {
                customScript.selectedTab = 0;
            }
            if (GUILayout.Button("Events"))
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
            if (GUILayout.Button("Settings"))
            {
                customScript.selectedTab = 0;
            }
            if (GUILayout.Button("Events"))
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
        if (GUI.changed)
        {
            EditorUtility.SetDirty(customScript);
        }

    }
    SerializedProperty onClickEventProperty;
    SerializedProperty onClickEventProperty2;
    SerializedProperty onClickEventProperty3;
    SerializedProperty onClickEventProperty4;
    SerializedProperty renderObjectsProperty;
    private void OnEnable()
    {
        // Получаем ссылку на UnityEvent
        onClickEventProperty = serializedObject.FindProperty("BasicEvent");
        onClickEventProperty2 = serializedObject.FindProperty("ReverseEvent");
        onClickEventProperty3 = serializedObject.FindProperty("BasicTriggerEvent");
        onClickEventProperty4 = serializedObject.FindProperty("ReverseTriggerEvent");
        renderObjectsProperty = serializedObject.FindProperty("RenderObjects");
    }

    private void ShowInteractTab1(AnimationCreator customScript)
    {
        customScript.Interactive = EditorGUILayout.Toggle("Enable", customScript.Interactive);
        customScript.Toggle = EditorGUILayout.Toggle("Toggle", customScript.Toggle);
        
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
        customScript.Toggle = EditorGUILayout.Toggle("Toggle", customScript.Toggle);
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
    private void ShowTab(AnimationCreator customScript)
    {
        customScript.Playing = EditorGUILayout.Toggle("Play", customScript.Playing);
        customScript.Reverse = EditorGUILayout.Toggle("Reverse", customScript.Reverse);
        customScript.Smooth = EditorGUILayout.Toggle("Smooth", customScript.Smooth);
       
    }

    private void ShowTab2(AnimationCreator customScript)
    {
        customScript.AnimationObject = (Transform)EditorGUILayout.ObjectField("Transform", customScript.AnimationObject, typeof(Transform), true);
        customScript.AnimationMove = EditorGUILayout.Vector3Field("Move", customScript.AnimationMove);
        customScript.MoveSpeed = EditorGUILayout.FloatField("Move Speed", customScript.MoveSpeed);
        customScript.GlobalMove = EditorGUILayout.Toggle("Global Move", customScript.GlobalMove);

        customScript.AnimationRotation = EditorGUILayout.Vector3Field("Rotation", customScript.AnimationRotation);
        customScript.RotationSpeed = EditorGUILayout.FloatField("Rotation Speed", customScript.RotationSpeed);

        customScript.AnimationScale = EditorGUILayout.Vector3Field("Scale", customScript.AnimationScale);
        customScript.ScaleSpeed = EditorGUILayout.FloatField("Scale Speed", customScript.ScaleSpeed);

    }
    private void ShowTab3(AnimationCreator customScript)
    {
        customScript.RigidbodyObject = (Rigidbody)EditorGUILayout.ObjectField("Rigidbody", customScript.RigidbodyObject, typeof(Rigidbody), true);

        customScript.ToggleGravity = EditorGUILayout.Toggle("Toggle Gravity", customScript.ToggleGravity);
        customScript.BasicGravity = EditorGUILayout.Toggle("Gravity", customScript.BasicGravity);
        customScript.Force = EditorGUILayout.FloatField("Force", customScript.Force);
        customScript.BasicDirection = (Direction)EditorGUILayout.EnumPopup(
            "Basic Direction",
            customScript.BasicDirection
        );
        customScript.ReverseDirection = (Direction)EditorGUILayout.EnumPopup(
                    "Reverse Direction",
                    customScript.ReverseDirection
                );
    }
    private void ShowTab4(AnimationCreator customScript)
    {
        customScript.ChangeMaterial = EditorGUILayout.Toggle("Change Material", customScript.ChangeMaterial);
        customScript.MaterialID = EditorGUILayout.IntField("MaterialID", customScript.MaterialID);
        serializedObject.Update();
        EditorGUILayout.PropertyField(renderObjectsProperty, new GUIContent("Render Objects"), true);
        serializedObject.ApplyModifiedProperties();
        customScript.BaseMaterial = (Material)EditorGUILayout.ObjectField("Basic Material", customScript.BaseMaterial, typeof(Material), false);
        customScript.ReverseMaterial = (Material)EditorGUILayout.ObjectField("Reverse Material", customScript.ReverseMaterial, typeof(Material), false);
        
    }
    private void ShowTab5(AnimationCreator customScript)
    {
        customScript.EnableSound = EditorGUILayout.Toggle("Enable Audio", customScript.EnableSound);
        customScript.AudioPlayer = (AudioSource)EditorGUILayout.ObjectField("Player", customScript.AudioPlayer, typeof(AudioSource), true);
        customScript.BasicAudio = (AudioClip)EditorGUILayout.ObjectField("Basic Sound", customScript.BasicAudio, typeof(AudioClip), false);
        customScript.ReverseAudio = (AudioClip)EditorGUILayout.ObjectField("Reverse Sound", customScript.ReverseAudio, typeof(AudioClip), false);

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