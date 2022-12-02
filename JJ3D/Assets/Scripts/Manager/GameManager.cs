using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Refrence")]
    [SerializeField] internal Player player;
    [SerializeField] internal Effects effects;
    [SerializeField] internal PickUpSystem pickUpSystem;
    [SerializeField] internal ForestGenerator forestGenerator;
    [SerializeField] internal EquipmentManager equipementManager;
    [SerializeField] GameObject mainCanvas;

    public Transform playerPos { get { return player.transform; } }

    public bool isGameOver;

    private void Start()
    {
        isGameOver = false;
        mainCanvas.SetActive(true);
    }

    public void GameOver()
    {
        isGameOver = true;
        mainCanvas.SetActive(false);
        player.GameOver();
    }
}
