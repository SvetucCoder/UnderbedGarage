using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Glitch0 : MonoBehaviour
{
	public Volume GlobalVolume;

	private bool _isEntered;

	[Range(0f, 1f)]
	public float ChromaticAberrationLimit = 0.5f;
    [Range(0f, 1f)]
    public float FilmGrainLimit = 0.5f;
    [Range(0f, 1f)]
    public float VignetteLimit = 0.5f;

    private ChromaticAberration ChromaticAberration;
    private FilmGrain FilmGrain;
    private Vignette Vignette;

    public void Start()
	{
		GlobalVolume.profile.TryGet(out ChromaticAberration chrobb);
		ChromaticAberration = chrobb;
        GlobalVolume.profile.TryGet(out FilmGrain grain);
        FilmGrain = grain;
        GlobalVolume.profile.TryGet(out Vignette vign);
        Vignette = vign;
    }
	void OnTriggerEnter()
	{
		_isEntered = true;
	}

	private void FixedUpdate()
	{
		ChromAbb();
		Grain();
		VignetteVoid();
    }

	void ChromAbb()
	{
		if (!_isEntered) return;
		if (ChromaticAberration.intensity.value < ChromaticAberrationLimit)
		{
			ChromaticAberration.intensity.value += Time.fixedDeltaTime * 0.15f;
		}
	}

	void Grain()
	{
        if (!_isEntered) return;
        if (FilmGrain.intensity.value < FilmGrainLimit)
		{
			FilmGrain.intensity.value += Time.fixedDeltaTime * 0.35f;
		}
	}

    void VignetteVoid()
    {
        if (!_isEntered) return;
        if (Vignette.intensity.value < VignetteLimit)
        {
            Vignette.intensity.value += Time.fixedDeltaTime * 0.35f;
        }
    }
}