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
    None = 0,       // ������������� �������� �������� "None" ��� ������� ���������
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
                mat[MaterialID] = ReverseMaterial;
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