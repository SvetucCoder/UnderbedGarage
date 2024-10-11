using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Glitch0 : MonoBehaviour
{
    Volume _postProcessVolume;
    ChromaticAberration _chromabb;
    public ClampedFloatParameter(float value, float min, float max, bool overrideState = false);

    void Awake()
    {
        _postProcessVolume = GameObject.Find("GlobalVolume").GetComponent<Volume>();
        _postProcessVolume.profile.TryGet(out _chromabb);
    }
    void OnTriggerEnter()
    {
        _chromabb.intensity = ChromaticAberrationIntensity;
        Debug.Log("1");
    }
}