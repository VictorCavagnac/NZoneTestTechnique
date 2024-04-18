using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private FadeChannelSO _fadeChannelSO;

    [SerializeField]
    private Image _fadeImage;

    private void OnEnable() 
    {
        _fadeChannelSO.OnEventRaised += InitiateFade;
    }

    private void OnDisable() 
    {
        _fadeChannelSO.OnEventRaised -= InitiateFade;
    }

    /// <summary>
	/// Controls the fade-in and fade-out.
	/// </summary>
	/// <param name="fadeIn">If false, the screen becomes black. If true, rectangle fades out and gameplay is visible.</param>
	/// <param name="duration">How long it takes to the image to fade in/out.</param>
	/// <param name="color">Target color for the image to reach. Disregarded when fading out.</param>
    private void InitiateFade(bool fadeIn, float duration, Color desiredColor)
    {
        _fadeImage.DOBlendableColor(desiredColor, duration);
    }
}
