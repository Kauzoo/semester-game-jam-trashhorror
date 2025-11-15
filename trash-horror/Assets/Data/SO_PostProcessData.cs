using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



[CreateAssetMenu(fileName = "SO_PostProcessData", menuName = "Scriptable Objects/SO_PostProcessData")]

public class SO_PostProcessData : ScriptableObject
{
    public enum CurveStates
    {
        Increas,
        Decreas,
        Neutral
    }
    
    private float steps = 0.1f;
    
    [Tooltip("Set depending on the value of fear bar (0-1) when the effect should start")]
    [Range(0, 1)]
    public float lensDistortionActivation;

    [Tooltip("Set depending on the value of fear bar (0-1) when the effect should start")]
    [Range(0, 1)]
    public float channelMixerActivation;
    
    [Tooltip("Set depending on the value of fear bar (0-1) when the effect should start")]
    [Range(0, 1)]
    public float whiteBalanceActivation;
    
    [Tooltip("Set depending on the value of fear bar (0-1) when the effect should start")]
    [Range(0, 1)]
    public float colorAdjustmentsActivation;
    
    [Tooltip("Set depending on the value of fear bar (0-1) when the effect should start")]
    [Range(0, 1)]
    public float chromaticAberrationActivation;

    private void OnValidate()
    {
        lensDistortionActivation = RoundToStep(lensDistortionActivation);
        channelMixerActivation = RoundToStep(channelMixerActivation);
        whiteBalanceActivation = RoundToStep(whiteBalanceActivation);
        colorAdjustmentsActivation = RoundToStep(colorAdjustmentsActivation);
        chromaticAberrationActivation = RoundToStep(chromaticAberrationActivation);
    }
    
    private float RoundToStep(float value)
    {
        return Mathf.Round(value / steps) * steps;
    }
    
    [System.Serializable]
    public struct LensDistortionData
    {
        public AnimationCurve curve;
        
        [NonSerialized]
        public float t;
        
        [NonSerialized]
        public LensDistortion component;
        
        [NonSerialized]
        public CurveStates curveState;
        
    }

    [System.Serializable]
    public struct ChannelMixerData
    {
        public AnimationCurve curve;
        
        [NonSerialized]
        public float t;
        
        [NonSerialized]
        public CurveStates curveState;
        
        [NonSerialized]
        public ChannelMixer component;
    }
    
    [System.Serializable]
    public struct WhiteBalanceData
    {
        public AnimationCurve curve;
        
        [NonSerialized]
        public float t;
        
        [NonSerialized]
        public CurveStates curveState;
        
        [NonSerialized]
        public WhiteBalance component;
    }

    [System.Serializable]
    public struct ChromaticAberrationData
    {
        public AnimationCurve curve;
        
        [NonSerialized]
        public float t;
        
        [NonSerialized]
        public ChromaticAberration component;

        [NonSerialized]
        public CurveStates curveState;
    }
}
