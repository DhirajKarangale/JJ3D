using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Refrence")]
    [SerializeField] Camera cam;
    public Animator animator;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Sward")]
    [SerializeField] AnimationClip[] swardClips;

    [Header("Bow")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform bow;
    [SerializeField] float force;
    private bool isAming;
    private bool isShoothing;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipSwipe;
    [SerializeField] AudioClip clipArrow;

    private AnimatorOverrideController overrideController;
    private float coolDownTime;
    private EquipementManager equipementManager;
    private bool isSwardActive;
    private bool isBowActive;

    private void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        coolDownTime = 0;
        equipementManager = GameManager.instance.equipementManager;
    }

    private void Update()
    {
        if (equipementManager.objSward.activeInHierarchy)
        {
            animator.SetBool("isAming", false);
            animator.SetBool("isShoothing", false);
            SwardAttack();
        }
        else if (equipementManager.objBow.activeInHierarchy)
        {
            BowAttack();
            BowPunch();
        }
        else
        {
            animator.SetBool("isAming", false);
            animator.SetBool("isShoothing", false);
        }

        coolDownTime -= Time.deltaTime;
    }

    private void SwardAttack()
    {
        if (Input.GetKeyDown(KeyCode.F) && (coolDownTime <= 0))
        {
            coolDownTime = 1.1f;
            int attack = Random.Range(0, swardClips.Length);
            animator.Play("SwardAttack");
            overrideController[swardClips[0].name] = swardClips[attack];
        }
    }

    private void BowPunch()
    {
        if (Input.GetKeyDown(KeyCode.G) && (coolDownTime <= 0))
        {
            animator.Play("BowPunch");
            coolDownTime = 1.1f;
        }
    }

    private void BowAttack()
    {
        if (Input.GetMouseButtonDown(0) && (coolDownTime <= 0))
        {
            isShoothing = false;
            isAming = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShoothing = true;
        }

        if (isAming)
        {
            transform.localRotation = Quaternion.Euler(0, 65, 0);
            animator.SetBool("isAming", true);
            animator.SetBool("isShoothing", isShoothing);
        }
        else
        {
            animator.SetBool("isAming", false);
        }
    }

    public void ShootArrow()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Vector3 targetPoint = ray.GetPoint(1000);

        GameObject currArrow = Instantiate(arrowPrefab, bow.transform.position, bow.rotation);
        currArrow.GetComponent<Rigidbody>().AddForce((targetPoint - bow.transform.position).normalized * force, ForceMode.Impulse);

        Invoke("ReSetRotation", 0.6f);
        coolDownTime = 1.1f;
        isAming = false;
        isShoothing = false;

        audioSource.PlayOneShot(clipArrow);
    }

    private void ReSetRotation()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void DesableWeapon()
    {
        isSwardActive = equipementManager.objSward.activeInHierarchy;
        isBowActive = equipementManager.objBow.activeInHierarchy;

        equipementManager.objSward.SetActive(false);
        equipementManager.objBow.SetActive(false);
    }

    public void EnableWeapon()
    {
        equipementManager.objSward.SetActive(isSwardActive);
        equipementManager.objBow.SetActive(isBowActive);
    }

    public void SwipeSound()
    {
        audioSource.PlayOneShot(clipSwipe);
    }

    public void WalkSound()
    {
        playerMovement.WalkSound();
    }
}