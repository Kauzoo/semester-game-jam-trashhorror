using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour,  IGameEventListener
{
    [SerializeField]
    private Volume globalVolume;
    
    [SerializeField]
    private GameEvent onSanityChanged;
    
    private LensDistortion lensDistortion;
    private ChannelMixer channelMixer;
    private ColorAdjustments colorAdjustments;
    private WhiteBalance whiteBalance;
    private ChromaticAberration chromaticAberration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalVolume.profile.TryGet<LensDistortion>(out lensDistortion);
        globalVolume.profile.TryGet<ChannelMixer>(out channelMixer);
        globalVolume.profile.TryGet<WhiteBalance>(out whiteBalance);
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
    }

    public void OnEnable()
    {
        if (onSanityChanged != null)
        {
            onSanityChanged.RegisterListener(this);
        }
    }

    public void OnDisable()
    {
      if(onSanityChanged != null)
      {
          onSanityChanged.UnregisterListener(this);
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEventRaised()
    {
        chromaticAberration.intensity.value = 1.0f;
        return;
    }
}
