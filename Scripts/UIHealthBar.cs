using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    public Image HPBar;
    float originalSize;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        originalSize = HPBar.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        HPBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}