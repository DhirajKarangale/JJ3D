using UnityEngine;
using System;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Slider slider;
    [SerializeField] Outline outline;
    [SerializeField] GameObject objCloseButton;
    internal Item item;

    internal Action OnRemove;

    public void Reset()
    {
        item = null;
        outline.enabled = false;
        objCloseButton.SetActive(false);
        image.sprite = null;
        slider.value = 0;
    }

    public void SetData(Item item)
    {
        this.item = item;
        outline.enabled = true;
        objCloseButton.SetActive(true);
        image.sprite = item.itemData.sprite;
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        slider.value = item.itemData.currHealth / item.itemData.mxHealth;
    }

    public void ButtonRemove()
    {
        GameManager.instance.effects.ButtonSound();
        OnRemove?.Invoke();
    }
}
