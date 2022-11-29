using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField] Image imgItem;
    [SerializeField] TMP_Text txtCount;
    [SerializeField] Outline outline;

    internal bool isEmpty;

    public event Action<InventoryItem> OnClicked;
    public event Action<InventoryItem> OnItemBeginDrag;
    public event Action<InventoryItem> OnItemEndDrag;
    public event Action<InventoryItem> OnItemDrop;

    private void Awake()
    {
        Reset();
        Deselect();
    }


    public void OnPointerClick(PointerEventData pointerData)
    {
        OnClicked?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty) return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(this.name + " is Drop");
        OnItemDrop?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }



    public void SetData(Sprite sprite, int count)
    {
        imgItem.gameObject.SetActive(true);
        imgItem.sprite = sprite;
        txtCount.gameObject.SetActive(true);
        txtCount.text = count.ToString();
        isEmpty = false;
    }

    public void Reset()
    {
        imgItem.gameObject.SetActive(false);
        txtCount.gameObject.SetActive(false);
        outline.enabled = false;
        isEmpty = true;
    }

    public void Select()
    {
        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
    }
}
