using UnityEngine;
using System;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Slider slider;
    [SerializeField] UnityEngine.UI.Outline outline;
    [SerializeField] GameObject objCloseButton;
    internal ItemData itemData;

    internal Action OnRemove;

    public void Reset()
    {
        itemData = null;
        outline.enabled = false;
        objCloseButton.SetActive(false);
        image.sprite = null;
        slider.value = 0;
    }

    public void SetData(ItemData itemData)
    {
        this.itemData = itemData;
        outline.enabled = true;
        objCloseButton.SetActive(true);
        image.sprite = itemData.sprite;
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        slider.value = itemData.currHealth / itemData.mxHealth;
    }

    public void ButtonRemove()
    {
        GameManager.instance.effects.ButtonSound();
        OnRemove?.Invoke();
    }
}
