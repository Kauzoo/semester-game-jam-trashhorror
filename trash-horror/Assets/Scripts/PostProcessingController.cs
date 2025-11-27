using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour, IGameEventListener
{

    [SerializeField]
    private Volume globalVolume;

    [SerializeField]
    private SO_PostProcessData.spotLight spotLight;

    [SerializeField]
    private GameEvent onSanityChanged;

    [SerializeField]
    private GameEvent onRespawn;

    [SerializeField]
    private FloatVariable sanityData;

    [SerializeField]
    private SO_PostProcessData postProcessData;

    [SerializeField]
    private SO_PostProcessData.LensDistortionData lensDistortionData;

    [SerializeField]
    private SO_PostProcessData.ChannelMixerData channelMixerData;

    [SerializeField]
    private SO_PostProcessData.WhiteBalanceData whiteBalanceData;

    [SerializeField]
    private SO_PostProcessData.ChromaticAberrationData chromaticAberrationData;

    [SerializeField]
    private FloatVariable lightFadeModifier;

    public static PostProcessingController Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalVolume.profile.TryGet<LensDistortion>(out lensDistortionData.component);
        globalVolume.profile.TryGet<ChannelMixer>(out channelMixerData.component);
        globalVolume.profile.TryGet<WhiteBalance>(out whiteBalanceData.component);
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberrationData.component);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            onRespawn.RegisterListener(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.spotLight = spotLight;
            Instance.globalVolume = globalVolume;
            Destroy(gameObject);
        }
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
        if (onSanityChanged != null)
        {
            onSanityChanged.UnregisterListener(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleSpotlight();
        HandleLensDistortion();
        HandleChannelMixer();
        HandleWhiteBalance();
        HandleChromaticAberration();

    }

    private void HandleSpotlight()
    {
        if (spotLight.t > 0.5f)
            spotLight.t = 0.5f;
        if (spotLight.t < 0f)
            spotLight.t = 0f;

        switch (spotLight.curveState)
        {
            case SO_PostProcessData.CurveStates.Increas:
                spotLight.t += Time.deltaTime;
                break;
            case SO_PostProcessData.CurveStates.Decreas:
                spotLight.t -= Time.deltaTime;
                break;
        }
        spotLight.light.pointLightOuterRadius = Math.Min(lightFadeModifier.value, spotLight.curve.Evaluate(spotLight.t));
    }

    private void HandleLensDistortion()
    {
        if (lensDistortionData.t <= 0f)
            lensDistortionData.t = 0f;

        if (lensDistortionData.t > 2f)
            lensDistortionData.t = 0f;

        switch (lensDistortionData.curveState)
        {
            case SO_PostProcessData.CurveStates.Increas:
                lensDistortionData.t += Time.deltaTime;
                break;

            case SO_PostProcessData.CurveStates.Decreas:
                lensDistortionData.t -= Time.deltaTime;
                break;
        }

        lensDistortionData.component.intensity.value = lensDistortionData.curve.Evaluate(lensDistortionData.t);
    }

    private void HandleChannelMixer()
    {
        if (channelMixerData.t <= 0f)
            channelMixerData.t = 0f;

        if (channelMixerData.t > 1.5f)
            channelMixerData.t = 0f;

        switch (channelMixerData.curveState)
        {
            case SO_PostProcessData.CurveStates.Increas:
                channelMixerData.t += Time.deltaTime;
                break;

            case SO_PostProcessData.CurveStates.Neutral:
                channelMixerData.t = 0;
                break;
        }
        channelMixerData.component.greenOutGreenIn.value = channelMixerData.curve.Evaluate(channelMixerData.t); ;
    }

    private void HandleWhiteBalance()
    {
        if (whiteBalanceData.t <= 0f)
            whiteBalanceData.t = 0f;
        if (whiteBalanceData.t > 4f)
            whiteBalanceData.t = 0f;

        switch (whiteBalanceData.curveState)
        {
            case SO_PostProcessData.CurveStates.Increas:
                whiteBalanceData.t += Time.deltaTime;
                break;
            case SO_PostProcessData.CurveStates.Decreas:
                whiteBalanceData.t -= Time.deltaTime;
                break;
        }

        whiteBalanceData.component.temperature.value = whiteBalanceData.curve.Evaluate(whiteBalanceData.t);
    }
    private void HandleChromaticAberration()
    {

        if (chromaticAberrationData.t <= 0)
            chromaticAberrationData.t = 0f;
        if (chromaticAberrationData.t >= 1f)
        {
            chromaticAberrationData.t = 1f;
        }

        switch (chromaticAberrationData.curveState)
        {
            case SO_PostProcessData.CurveStates.Increas:
                chromaticAberrationData.t += Time.deltaTime;
                break;
            case SO_PostProcessData.CurveStates.Decreas:
                chromaticAberrationData.t -= Time.deltaTime;
                break;
            default:
                return;
        }
        chromaticAberrationData.component.intensity.value = chromaticAberrationData.curve.Evaluate(chromaticAberrationData.t);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void OnEventRaised()
    {
        if (onSanityChanged && sanityData)
        {
            if (sanityData.value > postProcessData.spotLightManipulation)
            {
                spotLight.curveState = SO_PostProcessData.CurveStates.Increas;
            }

            else if (sanityData.value < postProcessData.spotLightManipulation)
            {
                spotLight.curveState = SO_PostProcessData.CurveStates.Decreas;
            }

            if (sanityData.value < postProcessData.chromaticAberrationActivation)
            {
                chromaticAberrationData.curveState = SO_PostProcessData.CurveStates.Increas;
            }
            else if (sanityData.value > postProcessData.chromaticAberrationActivation)
            {
                chromaticAberrationData.curveState = SO_PostProcessData.CurveStates.Decreas;
            }

            if (sanityData.value < postProcessData.lensDistortionActivation)
            {
                lensDistortionData.curveState = SO_PostProcessData.CurveStates.Increas;
            }

            else if (sanityData.value > postProcessData.lensDistortionActivation)
            {
                lensDistortionData.curveState = SO_PostProcessData.CurveStates.Decreas;
            }

            if (sanityData.value < postProcessData.channelMixerActivation)
            {
                channelMixerData.curveState = SO_PostProcessData.CurveStates.Increas;
            }

            else if (sanityData.value > postProcessData.channelMixerActivation)
            {
                channelMixerData.curveState = SO_PostProcessData.CurveStates.Neutral;
            }

            if (sanityData.value < postProcessData.whiteBalanceActivation)
            {
                whiteBalanceData.curveState = SO_PostProcessData.CurveStates.Increas;
            }

            else if (sanityData.value > postProcessData.whiteBalanceActivation)
            {
                whiteBalanceData.curveState = SO_PostProcessData.CurveStates.Decreas;
            }


        }


        Debug.Log("No Valid Objects");
    }
}
