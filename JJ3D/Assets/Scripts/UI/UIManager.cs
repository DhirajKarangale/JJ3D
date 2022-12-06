using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseObj;
    [SerializeField] GameObject controlObj;
    [SerializeField] Inventory inventory;

    private GameManager gameManager;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    private void Start()
    {
        gameManager = GameManager.instance;
        playerMovement = gameManager.player.playerMovement;
        playerAttack = gameManager.player.playerAttack;
        DefaultUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventory.isActive || pauseObj.activeInHierarchy) DefaultUI();
            else if (controlObj.activeInHierarchy) PauseButton();
        }
    }

    public void DefaultUI()
    {
        gameManager.effects.ButtonSound();
        Time.timeScale = 1;
        pauseObj.SetActive(false);
        controlObj.SetActive(true);
        inventory.ButtonActive(false);
    }

    public void PauseButton()
    {
        gameManager.effects.ButtonSound();
        Time.timeScale = 0;
        pauseObj.SetActive(true);
        controlObj.SetActive(false);
        inventory.ButtonActive(false);
    }

    public void InventoryButton()
    {
        gameManager.effects.ButtonSound();
        Time.timeScale = 0;
        gameManager.effects.ButtonSound();
        pauseObj.SetActive(false);
        controlObj.SetActive(false);
        inventory.ButtonActive(true);
    }

    public void ButtonJump()
    {
        playerMovement.Jump();
    }

    public void ButtonAttack()
    {
        playerAttack.Attack();
    }
}
