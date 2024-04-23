using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UITowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField]
    private RectTransform _rectTransform;

    [SerializeField]
    public Button button;

    [SerializeField]
    private TMP_Text _cost;

    public void Initiate(int cost, Sprite towerSprite)
    {
        _cost.text = cost.ToString();
        button.image.sprite = towerSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOScaleX(1.2f, 0.2f);
        _rectTransform.DOScaleY(1.2f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOScaleX(1, 0.2f);
        _rectTransform.DOScaleY(1, 0.2f);
    }
}
