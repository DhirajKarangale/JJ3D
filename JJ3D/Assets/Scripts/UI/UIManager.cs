using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseObj;
    [SerializeField] GameObject controlObj;
    [SerializeField] Inventory inventory;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
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
        gameManager.ButtonSound();
        Time.timeScale = 1;
        pauseObj.SetActive(false);
        controlObj.SetActive(true);
        inventory.ButtonActive(false);
    }

    public void PauseButton()
    {
        gameManager.ButtonSound();
        Time.timeScale = 0;
        pauseObj.SetActive(true);
        controlObj.SetActive(false);
        inventory.ButtonActive(false);
    }

    public void InventoryButton()
    {
        gameManager.ButtonSound();
        Time.timeScale = 0;
        gameManager.ButtonSound();
        pauseObj.SetActive(false);
        controlObj.SetActive(false);
        inventory.ButtonActive(true);
    }
}
