using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseObj;
    [SerializeField] GameObject controlObj;
    [SerializeField] GameObject inventoryObj;

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
            if (inventoryObj.activeInHierarchy || pauseObj.activeInHierarchy) DefaultUI();
            else if (controlObj.activeInHierarchy) PauseButton();
        }
    }

    public void DefaultUI()
    {
        gameManager.ButtonSound();
        Time.timeScale = 1;
        pauseObj.SetActive(false);
        controlObj.SetActive(true);
        inventoryObj.SetActive(false);
    }

    public void PauseButton()
    {
        gameManager.ButtonSound();
        Time.timeScale = 0;
        pauseObj.SetActive(true);
        controlObj.SetActive(false);
        inventoryObj.SetActive(false);
    }

    public void InventoryButton()
    {
        gameManager.ButtonSound();
        Time.timeScale = 0;
        gameManager.ButtonSound();
        pauseObj.SetActive(false);
        controlObj.SetActive(false);
        inventoryObj.SetActive(true);
    }
}
