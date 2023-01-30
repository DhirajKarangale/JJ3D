using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Refrence")]
    [SerializeField] internal Player player;
    [SerializeField] internal Effects effects;
    [SerializeField] internal PickUpSystem pickUpSystem;
    [SerializeField] internal ForestGenerator forestGenerator;
    [SerializeField] internal CollectableData collectableData;
    [SerializeField] internal EquipmentManager equipementManager;
    [SerializeField] GameObject mainCanvas;

    public Transform playerPos { get { return player.transform; } }

    public bool isGameOver;

    private void Start()
    {
        isGameOver = false;
        mainCanvas.SetActive(true);
        Invoke("InitializeWeapon", 1);
        // InvokeRepeating("ClearGarbage", 60, 60);
    }

    private void ClearGarbage()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    private void InitializeWeapon()
    {
        ObjectPooler.instance.SpwanObject("BowThree", playerPos.position + new Vector3(0, 11, 0));
        ObjectPooler.instance.SpwanObject("BowFire", playerPos.position + new Vector3(0, 11, 0));
        ObjectPooler.instance.SpwanObject("BowNormal", playerPos.position + new Vector3(0, 11, 0));
        ObjectPooler.instance.SpwanObject("SwardNormal", playerPos.position + new Vector3(0, 11, 0));
        ObjectPooler.instance.SpwanObject("SwardIce", playerPos.position + new Vector3(0, 11, 0));
        ObjectPooler.instance.SpwanObject("SwardLightning", playerPos.position + new Vector3(0, 11, 0));
    }

    public void GameOver()
    {
        isGameOver = true;
        mainCanvas.SetActive(false);
        player.GameOver();
    }
}
