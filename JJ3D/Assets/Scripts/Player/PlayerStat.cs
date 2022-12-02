using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] EquipmentManager equipmentManager;

    private float mxHealth;
    private float currhealth;
    private float speed;
    private float defence;
    private float attack;

    internal void ChangeHealth(Item item)
    {
        equipmentManager.Eat(item);
    }

    internal void EquipWeapon(Item item)
    {
        equipmentManager.SetWeapon(item);
    }
}
