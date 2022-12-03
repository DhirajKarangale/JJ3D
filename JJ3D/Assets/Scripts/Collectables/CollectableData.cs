using TMPro;
using UnityEngine;
using System.Collections;

public class CollectableData : Singleton<CollectableData>
{
    [SerializeField] TMP_Text txtCoin;
    [SerializeField] TMP_Text txtKey;
    private GameManager gameManager;
    internal int coins;
    internal byte keys;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private IEnumerator IEShakeCoins()
    {
        txtCoin.transform.position += new Vector3(10, 0, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        txtCoin.transform.position += new Vector3(0, 0, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        txtCoin.transform.position += new Vector3(-10, 0, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        txtCoin.transform.position += new Vector3(0, 0, 0);
    }

    public void UpdateCoin(int amount, Vector3 pos)
    {
        if (amount > 0) gameManager.effects.CollectEffect(pos);

        coins += amount;
        txtCoin.text = coins.ToString();
    }

    public void UpdateKey(byte amount, Vector3 pos)
    {
        if (amount > 0) gameManager.effects.CollectEffect(pos);

        keys += amount;
        txtKey.text = keys.ToString();
    }

    public void ShakeCoins()
    {
        StartCoroutine(IEShakeCoins());
        gameManager.effects.CollectEffect(Vector3.zero);
    }
}
