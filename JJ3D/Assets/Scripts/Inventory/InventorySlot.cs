using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField] Image imgItem;
    [SerializeField] internal Outline outline;
    [SerializeField] GameObject objSelect;
    [SerializeField] TMP_Text txtActionName;

    internal bool isEmpty;

    public event Action<InventorySlot> OnClicked;
    public event Action<InventorySlot> OnDragStart;
    public event Action<InventorySlot> OnDragEnd;
    public event Action<InventorySlot> OnDropped;
    public event Action<InventorySlot> OnUseButton;
    public event Action<InventorySlot> OnDropButton;

    private void Awake()
    {
        Reset();
        Deselect();
    }


    public void OnPointerClick(PointerEventData pointerData)
    {
        OnClicked?.Invoke(this);
        if(!isEmpty) GameManager.instance.effects.ButtonSound();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty) return;
        OnDragStart?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropped?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }



    public void SetData(Sprite sprite, string actionName = "")
    {
        imgItem.gameObject.SetActive(true);
        imgItem.sprite = sprite;
        txtActionName.text = actionName;
        isEmpty = false;
    }

    public void Reset()
    {
        imgItem.gameObject.SetActive(false);
        Deselect();
        isEmpty = true;
    }

    public void Select()
    {
        outline.enabled = true;
        objSelect.SetActive(true);
    }

    public void Deselect()
    {
        outline.enabled = false;
        objSelect.SetActive(false);
    }


    public void ButtonDrop()
    {
        GameManager.instance.effects.ButtonSound();
        OnDropButton?.Invoke(this);
    }

    public void ButtonUse()
    {
        GameManager.instance.effects.ButtonSound();
        OnUseButton?.Invoke(this);
    }
}
