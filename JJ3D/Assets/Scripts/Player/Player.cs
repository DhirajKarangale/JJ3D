using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] internal Animator animator;
    [SerializeField] internal Rigidbody rigidBody;
    [SerializeField] internal AudioSource audioSource;
    [SerializeField] internal CapsuleCollider capsuleCollider;

    [Header("Refrences")]
    [SerializeField] internal Camera cam;
    [SerializeField] internal PlayerMovement playerMovement;
    [SerializeField] internal PlayerAttack playerAttack;
    [SerializeField] internal PlayerHealth playerHealth;
    private EquipmentManager equipmentManager;

    private float mxHealth;
    private float currhealth;
    private float defence;
    private float attack;

    private float orgForwardSpeed;
    private float orgBackSpeed;
    private float orgSideSpeed;


    private void Start()
    {
        equipmentManager = GameManager.instance.equipementManager;

        orgForwardSpeed = playerMovement.movement.forwardSpeed;
        orgBackSpeed = playerMovement.movement.backwardSpeed;
        orgSideSpeed = playerMovement.movement.sideSpeed;

        // Make Shoes Increase Speed here and reduce its health by -1
    }


    internal void PlayAudio(AudioClip clip, float volume = 1)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }

    internal void Eat(Item item)
    {
        equipmentManager.Eat(item);
    }

    internal void EquipWeapon(Item item)
    {
        equipmentManager.SetWeapon(item);
    }

    internal void ChangeSpeed(float modifier)
    {
        playerMovement.movement.forwardSpeed = orgForwardSpeed * modifier;
        playerMovement.movement.backwardSpeed = orgBackSpeed * modifier;
        playerMovement.movement.sideSpeed = orgSideSpeed * modifier;
    }

    internal void ResetSpeed()
    {
        playerMovement.movement.forwardSpeed = orgForwardSpeed;
        playerMovement.movement.backwardSpeed = orgBackSpeed;
        playerMovement.movement.sideSpeed = orgSideSpeed;
    }

    internal void GameOver()
    {
        capsuleCollider.height = 0.2f;
        capsuleCollider.material = null;
        equipmentManager.DesableWeapon();
        playerAttack.enabled = false;
        playerHealth.enabled = false;
        playerMovement.enabled = false;
    }
}
