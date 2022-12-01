using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Refrence")]
    [SerializeField] internal EquipementManagerOld equipementManager;
    [SerializeField] internal ForestGenerator forestGenerator;
    [SerializeField] internal PlayerStat playerStat;
    [SerializeField] internal PlayerAttack playerAttack;
    [SerializeField] internal PlayerHealth playerHealth;
    [SerializeField] internal PickUpSystem pickUpSystem;
    // [SerializeField] internal PlayerInteract playerInteract;
    [SerializeField] internal PlayerMovement playerMovement;
    [SerializeField] internal InventoryOld inventory;
    [SerializeField] internal EquipmentSlotOld helmetSlot;
    [SerializeField] internal EquipmentSlotOld vestSlot;
    [SerializeField] internal EquipmentSlotOld shoesSlot;
    [SerializeField] internal EquipmentSlotOld weaponSlot;
    [SerializeField] GameObject mainCanvas;

    [Header("Effect")]
    [SerializeField] ParticleSystem psDestroyItem;
    [SerializeField] ParticleSystem psEnemyDye;
    [SerializeField] ParticleSystem psEnemyBlood;
    [SerializeField] ParticleSystem psPlayerBlood;
    [SerializeField] ParticleSystem psFireballDestroy;
    [SerializeField] ParticleSystem psBombExplosion;
    [SerializeField] ParticleSystem psPick;
    [SerializeField] ParticleSystem psCollect;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipDestroyItem;
    [SerializeField] AudioClip clipEnemyDye;
    [SerializeField] AudioClip clipButton;
    [SerializeField] AudioClip clipPick;
    [SerializeField] AudioClip clipNPCHurt;
    [SerializeField] AudioClip clipPlayerHurt;
    [SerializeField] AudioClip clipHit;
    [SerializeField] AudioClip clipCollect;

    public Transform playerPos { get { return playerStat.transform; } }

    public bool isGameOver;

    private void Start()
    {
        mainCanvas.SetActive(true);
    }

    public void DestroyEffect(Vector3 pos)
    {
        audioSource.volume = 0.5f;
        psDestroyItem.transform.position = pos;
        psDestroyItem.Play();
        audioSource.PlayOneShot(clipDestroyItem);
    }

    public void PickEffect(Vector3 pos)
    {
        audioSource.volume = 0.5f;
        psPick.transform.position = pos;
        psPick.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipPick);
    }

    public void CollectEffect(Vector3 pos)
    {
        audioSource.volume = 0.5f;
        psCollect.transform.position = pos;
        psCollect.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipCollect);
    }

    public void EnemyBloodEffect(Vector3 pos)
    {
        audioSource.volume = 0.2f;
        psEnemyBlood.transform.position = pos;
        psEnemyBlood.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipNPCHurt);
    }

    public void PlayerBloodEffect(Vector3 pos)
    {
        audioSource.volume = 0.2f;
        psPlayerBlood.transform.position = pos;
        psPlayerBlood.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipPlayerHurt);
    }

    public void DestroyBodyEffect(Vector3 pos)
    {
        audioSource.volume = 0.4f;
        psEnemyDye.transform.position = pos;
        psEnemyDye.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipEnemyDye);
    }

    public void FireballDestroyEffect(Vector3 pos)
    {
        audioSource.volume = 0.4f;
        psFireballDestroy.transform.position = pos;
        psFireballDestroy.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipDestroyItem);
    }

    public void BombExplosionEffect(Vector3 pos)
    {
        audioSource.volume = 0.4f;
        psBombExplosion.transform.position = pos;
        psBombExplosion.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipDestroyItem);
    }

    public void ButtonSound()
    {
        audioSource.volume = 1;
        audioSource.PlayOneShot(clipButton);
    }

    public void HitSound(Vector3 pos)
    {
        audioSource.transform.position = pos;
        audioSource.volume = 1;
        audioSource.PlayOneShot(clipHit);
    }

    public void GameOver()
    {
        isGameOver = true;
        mainCanvas.SetActive(false);
        if (weaponSlot.equipedItem)
        {
            inventory.Remove(weaponSlot.equipedItem, true);
            weaponSlot.equipedItem.RemoveItem();
        }
        playerAttack.enabled = false;
        playerHealth.enabled = false;
        // playerInteract.enabled = false;
        playerMovement.enabled = false;
    }
}
