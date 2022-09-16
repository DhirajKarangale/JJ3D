using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Refrence")]
    [SerializeField] Camera cam;
    public Animator animator;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] EquipmentSlot equipmentSlot;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Sward")]
    [SerializeField] AnimationClip[] swardClips;

    [Header("Bow")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject fireArrowPrefab;
    [SerializeField] Transform bow;
    [SerializeField] float force;
    private bool isAming;
    private bool isShoothing;
    private bool isBowDestroyed;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipSwipe;
    [SerializeField] AudioClip clipArrow;

    private AnimatorOverrideController overrideController;
    private EquipementManager equipementManager;
    private float coolDownTime;
    private Item item;
    private GameManager gameManager;

    private bool isSwardNormalActive;
    private bool isSwardIceActive;
    private bool isSwardLightningActive;

    private bool isBowNormalActive;
    private bool isBowThreeActive;
    private bool isBowFireActive;

    private void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        coolDownTime = 0;
        gameManager = GameManager.instance;
        equipementManager = gameManager.equipementManager;
    }

    private void Update()
    {
        if (gameManager.isGameOver) return;

        if (equipementManager.isBowActive)
        {
            BowAttack();
            BowPunch();
        }
        else
        {
            animator.SetBool("isAming", false);
            animator.SetBool("isShoothing", false);
            ReSetRotation();
            if (equipementManager.isSwardActive) SwardAttack();
        }

        coolDownTime -= Time.deltaTime;
    }

    private void SwardAttack()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.F) && (coolDownTime <= 0))
        {

            item = equipmentSlot.equipedItem;
            if (item.currHealth > 0)
            {
                item.currHealth -= item.armorModifire;
            }
            else
            {
                item.DestroyItem();
                return;
            }

            coolDownTime = 1.1f;
            animator.Play("SwardAttack");
            overrideController[swardClips[0].name] = swardClips[Random.Range(0, swardClips.Length)];
        }
    }

    private void BowPunch()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.G) && (coolDownTime <= 0))
        {
            item = equipmentSlot.equipedItem;
            if (item.currHealth > 0)
            {
                item.currHealth -= item.armorModifire;
            }
            else
            {
                item.DestroyItem();
                return;
            }

            animator.Play("BowPunch");
            coolDownTime = 1.1f;
        }
    }

    private void BowAttack()
    {
        if (Time.timeScale == 0) return;

        //(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        if (Input.GetMouseButtonDown(0) && (coolDownTime <= 0))
        {
            item = equipmentSlot.equipedItem;
            if (item.currHealth <= 0) return;

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
            isBowDestroyed = false;
            animator.SetBool("isShoothing", isShoothing);
        }
        animator.SetBool("isAming", isAming);
    }

    public void ShootArrow()
    {
        if (Time.timeScale == 0) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Vector3 targetPoint = ray.GetPoint(1000);
        Vector3 shootDir = (targetPoint - bow.transform.position).normalized;

        if (equipementManager.objBowFire.activeInHierarchy)
        {
            GameObject currArrow = Instantiate(fireArrowPrefab, bow.position + new Vector3(0, 1, 0), bow.rotation);
            currArrow.transform.rotation = Quaternion.LookRotation(bow.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<FireArrow>().damage = equipmentSlot.equipedItem.damageModifire;
        }
        else
        {
            GameObject currArrow = Instantiate(arrowPrefab, bow.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(bow.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipmentSlot.equipedItem.damageModifire;
        }

        if (equipementManager.objBowThree.activeInHierarchy)
        {
            GameObject currArrow = Instantiate(arrowPrefab, bow.transform.position + new Vector3(0.5f, 1f, 0), bow.rotation);
            currArrow.transform.rotation = Quaternion.LookRotation(bow.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipmentSlot.equipedItem.damageModifire;

            currArrow = Instantiate(arrowPrefab, bow.transform.position - new Vector3(0.5f, -1f, 0), bow.rotation);
            currArrow.transform.rotation = Quaternion.LookRotation(bow.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipmentSlot.equipedItem.damageModifire;
        }

        item.currHealth -= item.armorModifire;
        Invoke("ReSetRotation", 0.6f);
        coolDownTime = 1.1f;
        isAming = false;
        isShoothing = false;

        audioSource.PlayOneShot(clipArrow);
    }

    private void DestroyBow()
    {
        item.DestroyItem();
        animator.SetBool("isAming", false);
        animator.SetBool("isShoothing", false);
        isAming = false;
        isShoothing = false;
        isBowDestroyed = true;
    }

    private void ReSetRotation()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (!isBowDestroyed && item && item.currHealth <= 0) DestroyBow();
    }

    public void DesableWeapon()
    {
        isSwardNormalActive = equipementManager.objSwardNormal.activeInHierarchy;
        isSwardIceActive = equipementManager.objSwardIce.activeInHierarchy;
        isSwardLightningActive = equipementManager.objSwardLightning.activeInHierarchy;

        isBowNormalActive = equipementManager.objBowNormal.activeInHierarchy;
        isBowThreeActive = equipementManager.objBowThree.activeInHierarchy;
        isBowFireActive = equipementManager.objBowFire.activeInHierarchy;

        equipementManager.objSwardNormal.SetActive(false);
        equipementManager.objSwardIce.SetActive(false);
        equipementManager.objSwardLightning.SetActive(false);

        equipementManager.objBowNormal.SetActive(false);
        equipementManager.objBowThree.SetActive(false);
        equipementManager.objBowFire.SetActive(false);
    }

    public void EnableWeapon()
    {
        equipementManager.objSwardNormal.SetActive(isSwardNormalActive);
        equipementManager.objSwardIce.SetActive(isSwardIceActive);
        equipementManager.objSwardLightning.SetActive(isSwardLightningActive);

        equipementManager.objBowNormal.SetActive(isBowNormalActive);
        equipementManager.objBowThree.SetActive(isBowThreeActive);
        equipementManager.objBowFire.SetActive(isBowFireActive);
    }

    public void SwipeSound()
    {
        audioSource.PlayOneShot(clipSwipe);
    }

    public void WalkSound()
    {
        playerMovement.WalkSound();
    }

    public void ReduceCollider()
    {
        capsuleCollider.height = 0.2f;
        capsuleCollider.material = null;
    }
}