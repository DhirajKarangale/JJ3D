using UnityEngine;

public class Effects : MonoBehaviour
{
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
    [SerializeField] AudioClip clipFireballDestroy;
    [SerializeField] AudioClip clipBombExplosion;
    [SerializeField] AudioClip clipEnemyDye;
    [SerializeField] AudioClip clipButton;
    [SerializeField] AudioClip clipPick;
    [SerializeField] AudioClip clipNPCHurt;
    [SerializeField] AudioClip clipPlayerHurt;
    [SerializeField] AudioClip clipHit;
    [SerializeField] AudioClip clipCollect;
    [SerializeField] AudioClip clipChest;

    private CamController camController;


    private void Start()
    {
        camController = CamController.instance;
    }

    public void DestroyEffect(Vector3 pos)
    {
        audioSource.volume = 0.02f;
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

    public void RockEnemyEffect(Vector3 pos)
    {
        audioSource.volume = 0.2f;
        psDestroyItem.transform.position = pos;
        psDestroyItem.Play();
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
        audioSource.volume = 0.3f;
        psEnemyDye.transform.position = pos;
        psEnemyDye.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipEnemyDye);
    }

    public void FireballDestroyEffect(Vector3 pos)
    {
        audioSource.volume = 0.5f;
        psFireballDestroy.transform.position = pos;
        psFireballDestroy.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipFireballDestroy);
    }

    public void BombExplosionEffect(Vector3 pos)
    {
        camController.Shake(0.7f);
        audioSource.volume = 0.7f;
        psBombExplosion.transform.position = pos;
        psBombExplosion.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipBombExplosion);
    }

    public void ChestEffect(Vector3 pos)
    {
        audioSource.volume = 1f;
        psEnemyDye.transform.position = pos;
        psEnemyDye.Play();
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(clipChest);
    }

    public void HitSound(Vector3 pos)
    {
        audioSource.transform.position = pos;
        audioSource.volume = 1;
        audioSource.PlayOneShot(clipHit);
    }

    public void ButtonSound()
    {
        audioSource.volume = 0.6f;
        audioSource.PlayOneShot(clipButton);
    }

}
