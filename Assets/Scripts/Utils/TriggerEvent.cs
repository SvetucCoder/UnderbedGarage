using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public new List<TriggerClass> trigger = new  List<TriggerClass>();

    public List<ToggleEvent> ToggleEvent = new List<ToggleEvent>();

    public List<AudioManager> AudioManager = new List<AudioManager>();

    public List<AnimationManager> AnimationManager = new List<AnimationManager>();

    private void OnTriggerEnter(Collider other)
    {

        foreach (var item in trigger)
        {
            if (other.gameObject.tag == item.NameTag)
            {
                item.ExecuteTriggerEnter();
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {

        foreach (var item in trigger)
        {
            if (other.gameObject.tag == item.NameTag)
            {
                item.ExecuteTriggerExit();
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in trigger)
        {
            if (collision.gameObject.tag == item.NameTag)
            {
                item.ExecuteColliderEnter();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        foreach (var item in trigger)
        {
            if (collision.gameObject.tag == item.NameTag)
            {
                item.ExecuteColliderExit();
            }
        }
    }
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
[System.Serializable]
public class TriggerClass
{
    public string NameTag;
    [SerializeField] private UnityEvent TriggerEnterEvent;

    [SerializeField] private UnityEvent TriggerExitEvent;

    [SerializeField] private UnityEvent ColliderEnterEvent;

    [SerializeField] private UnityEvent ColliderExitEvent;

    public virtual void ExecuteTriggerEnter()
    {
        UnityAPI.ExecuteUnityEvent(TriggerEnterEvent);
    }
    public virtual void ExecuteTriggerExit()
    {
        UnityAPI.ExecuteUnityEvent(TriggerExitEvent);
    }
    public virtual void ExecuteColliderEnter()
    {
        UnityAPI.ExecuteUnityEvent(ColliderEnterEvent);
    }
    public virtual void ExecuteColliderExit()
    {
        UnityAPI.ExecuteUnityEvent(ColliderExitEvent);
    }
}