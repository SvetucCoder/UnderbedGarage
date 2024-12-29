using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour
{

    public InteractClass interact = new InteractClass();

    public List<ToggleEvent> ToggleEvent = new List<ToggleEvent>();

    public List<AudioManager> AudioManager = new List<AudioManager>();

    public List<AnimationManager> AnimationManager = new List<AnimationManager>();

    public void ExecuteAnimationEvent(int ID)
    {
        AnimationManager[ID].Execute();
    }
    public void ExecuteToggleEvent(int ID)
    {
        ToggleEvent[ID].Execute();
    }
    public void ExecuteAudioEvent(int ID)
    {
        AudioManager[ID].Execute();
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
[System.Serializable]
public class ToggleEvent
{
    [Header("Toggle Event")]

    public UnityEvent ActivateEvent;
    public UnityEvent DeactivateEvent;
    [DisplayName("Активирован?")] public bool isactivate = false;

    [ContextMenu("MethodName")]
    public void Execute()
    {
        isactivate = !isactivate;
        if (isactivate)
        {
            UnityAPI.ExecuteUnityEvent(ActivateEvent);
        }
        else
        {
            UnityAPI.ExecuteUnityEvent(DeactivateEvent);
        }

    }
}

[System.Serializable]
public class InteractClass
{
    [Header("При наведении")]
    [SerializeField] private UnityEvent _hoverEvent;

    [Header("При наведении постоянно выполняется")]
    [SerializeField] private UnityEvent _presshoverEvent;

    [Header("При отведении от обьекта")]
    [SerializeField] private UnityEvent _unhoverEvent;

    [Header("При нажатии кнопки")]
    [SerializeField] private UnityEvent _clickEvent;

    [Header("При отжатии кнопки")]
    [SerializeField] private UnityEvent _unclickEvent;

    [Header("При зажатии кнопки")]
    [SerializeField] private UnityEvent _pressEvent;
    public virtual void ExecuteHover()
    {
        UnityAPI.ExecuteUnityEvent(_hoverEvent);
    }
    public virtual void ExecutePresshover()
    {
        UnityAPI.ExecuteUnityEvent(_presshoverEvent);
    }
    public virtual void ExecuteUnhover()
    {
        UnityAPI.ExecuteUnityEvent(_unhoverEvent);
    }
    public virtual void ExecuteClick()
    {
        UnityAPI.ExecuteUnityEvent(_clickEvent);
    }
    public virtual void ExecuteUnclick()
    {
        UnityAPI.ExecuteUnityEvent(_unclickEvent);
    }
    public virtual void ExecutePress()
    {
        UnityAPI.ExecuteUnityEvent(_pressEvent);
    }
}
[System.Serializable]
public class AudioManager
{
    public List<AudioList> Audio = new List<AudioList>();

    public void Execute()
    {
        for (int i = 0; i < Audio.Count; i++)
        {
            Audio[i].source.PlayOneShot(Audio[i].audio);
        }
    }
}
[System.Serializable]
public class AudioList
{
    public AudioClip audio;
    public AudioSource source;
}
[System.Serializable]
public class AnimationManager
{
    public List<AnimationList> Animations = new List<AnimationList>();

    public void Execute()
    {
        for (int i = 0; i < Animations.Count; i++)
        {
            Animations[i].animator.Play(Animations[i].Name);
        }
    }
}
[System.Serializable]
public class AnimationList
{
    public string Name;
    public Animator animator;
}
public class DisplayNameAttribute : PropertyAttribute
{
    public string DisplayName;

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
[CustomPropertyDrawer(typeof(DisplayNameAttribute))]
public class DisplayNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DisplayNameAttribute displayName = (DisplayNameAttribute)attribute;
        label.text = displayName.DisplayName;
        EditorGUI.PropertyField(position, property, label);
    }
}