using Assets.Scripts.Entities.Func.Utilits;
using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{

    [Header("��� ���������")]
    [SerializeField] private UnityEvent _hoverEvent;

    [Header("��� ��������� ��������� �����������")]
    [SerializeField] private UnityEvent _presshoverEvent;

    [Header("��� ��������� �� �������")]
    [SerializeField] private UnityEvent _unhoverEvent;

    [Header("��� ������� ������")]
    [SerializeField] private UnityEvent _clickEvent;

    [Header("��� ������� ������")]
    [SerializeField] private UnityEvent _unclickEvent;

    [Header("��� ������� ������")]
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
