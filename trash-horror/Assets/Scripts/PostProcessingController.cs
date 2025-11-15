using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour,  IGameEventListener
{
    
    [SerializeField]
    private Volume globalVolume;
    
    [SerializeField]
    private GameEvent onSanityChanged;
    
    [SerializeField]
    private FloatVariable sanityData;
    
    [SerializeField]
    private SO_PostProcessData postProcessData;
    
    [SerializeField]
    private SO_PostProcessData.LensDistortionData  lensDistortionData;
    
    [SerializeField]
    private SO_PostProcessData.ChannelMixerData  channelMixerData;
    
    [SerializeField]
    private SO_PostProcessData.WhiteBalanceData whiteBalanceData;
    
    [SerializeField]
    private SO_PostProcessData.ColorAdjustmentsData colorAdjustmentsData;
    
    [SerializeField]
    private SO_PostProcessData.ChromaticAberrationData chromaticAberrationData;
    
    private float chromaticCurveTime;
    private float lensDistortionCurveTime;
    private float channelMixerCurveTime;
    private float whiteBalanceCurveTime;
    private float colorAdjustmentsCurveTime;
    
    //private CurveStates curveState;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalVolume.profile.TryGet<LensDistortion>(out lensDistortionData.component);
        globalVolume.profile.TryGet<ChannelMixer>(out channelMixerData.component);
        globalVolume.profile.TryGet<WhiteBalance>(out whiteBalanceData.component);
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustmentsData.component);
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberrationData.component);
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
      if(onSanityChanged != null )
      {
          
          onSanityChanged.UnregisterListener(this);
      }
    }

    // Update is called once per frame
    void Update()
    {
        HandleChromaticAberration();
    }
    
    private void HandleChromaticAberration()
    {
        
        if(chromaticCurveTime <= 0)
            chromaticCurveTime = 0f;
        if (chromaticCurveTime >= 1f)
        {
            chromaticCurveTime = 1f;
        }
        
        switch (chromaticAberrationData.curveState)
        {
            case SO_PostProcessData.CurveStates.Increas:
                chromaticCurveTime += Time.deltaTime;
                break;
            case SO_PostProcessData.CurveStates.Decreas:
                chromaticCurveTime -= Time.deltaTime;
                break;
            default:
                return;
        }
        chromaticAberrationData.component.intensity.value = chromaticAberrationData.curve.Evaluate(chromaticCurveTime);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void OnEventRaised()
    {
        if (onSanityChanged && sanityData)
        {
            if (sanityData.value > postProcessData.chromaticAberrationActivation)
            {
                chromaticAberrationData.curveState = SO_PostProcessData.CurveStates.Increas;
            }
            else
            {
                chromaticAberrationData.curveState = SO_PostProcessData.CurveStates.Decreas;
            }
            
            
            
        }
        Debug.Log("No Valid Objects");
    }
}
