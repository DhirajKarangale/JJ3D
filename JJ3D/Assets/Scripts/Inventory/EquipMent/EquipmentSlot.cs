using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Slider slider;
    [SerializeField] Outline outline;

    public void Reset()
    {
        outline.enabled = false;
        image.sprite = null;
        slider.value = 1;
    }

    public void SetData(Sprite sprite, float currHealth)
    {
        outline.enabled = true;
        image.sprite = sprite;
        slider.value = currHealth;
    }
}
