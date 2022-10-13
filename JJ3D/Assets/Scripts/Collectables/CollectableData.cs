using UnityEngine.UI;
using UnityEngine;

public class CollectableData : Singleton<CollectableData>
{
    [SerializeField] Text txtCoin;
    [SerializeField] Text txtKey;
    private GameManager gameManager;
    private int coins;
    private byte keys;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void UpdateCoin(int amount, Vector3 pos)
    {
        if (amount > 0) gameManager.CollectEffect(pos);

        coins += amount;
        txtCoin.text = coins.ToString();
    }

    public void UpdateKey(byte amount, Vector3 pos)
    {
        if (amount > 0) gameManager.CollectEffect(pos);

        keys += amount;
        txtKey.text = keys.ToString();
    }
}
