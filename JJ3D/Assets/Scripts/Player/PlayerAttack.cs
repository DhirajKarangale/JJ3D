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
    [SerializeField] Transform firePos;
    [SerializeField] Transform firePos1;
    [SerializeField] Transform firePos2;
    [SerializeField] float force;
    internal bool isAming;
    internal bool isShoothing;
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

    private float requiredView;
    [SerializeField] float camSpeed;

    private void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        coolDownTime = 0;
        gameManager = GameManager.instance;
        equipementManager = gameManager.equipementManager;

        requiredView = 60;
        cam.fieldOfView = 60;
    }

    private void Update()
    {
        if (gameManager.isGameOver)
        {
            cam.fieldOfView = 60;
            return;
        }

        Attack();

        if (equipementManager.isBowActive)
        {
            BowAttack();
            Scope();
        }
        else
        {
            cam.fieldOfView = 60;
            animator.SetBool("isAming", false);
            animator.SetBool("isShoothing", false);
            ReSetRotation();
        }

        coolDownTime -= Time.deltaTime;

    }

    private void Attack()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.F) && (coolDownTime <= 0))
        {
            if (equipementManager.isSwardActive) SwardAttack();
            else if (equipementManager.isBowActive) BowPunch();
        }
    }

    private void SwardAttack()
    {
        item = equipmentSlot.equipedItem;
        if (item.currHealth > 0)
        {
            item.currHealth -= 1;
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

    private void BowPunch()
    {
        item = equipmentSlot.equipedItem;
        if (item.currHealth > 0)
        {
            item.currHealth -= 1;
        }
        else
        {
            item.DestroyItem();
            return;
        }

        animator.Play("BowPunch");
        coolDownTime = 1.1f;
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

            Invoke("GetView", 0.3f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShoothing = true;

            Invoke("ResetView", 1);
        }

        if (isAming)
        {
            transform.localRotation = Quaternion.Euler(0, 65, 0);
            isBowDestroyed = false;
            animator.SetBool("isShoothing", isShoothing);
        }
        animator.SetBool("isAming", isAming);
    }

    private void Scope()
    {
        if (requiredView != 60)
        {
            if (cam.fieldOfView > requiredView)
            {
                cam.fieldOfView -= Time.deltaTime * camSpeed;
            }
        }
        else if (cam.fieldOfView < 60)
        {
            cam.fieldOfView += Time.deltaTime * camSpeed;
        }
    }

    private void ResetView()
    {
        requiredView = 60;
    }

    private void GetView()
    {
        if (isShoothing) return;
        requiredView = 20;
    }

    public void ShootArrow()
    {
        if (Time.timeScale == 0) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Vector3 targetPoint = ray.GetPoint(1000);
        Vector3 shootDir = (targetPoint - firePos.transform.position).normalized;

        if (equipementManager.objBowFire.activeInHierarchy)
        {
            GameObject currArrow = Instantiate(fireArrowPrefab, firePos.position, Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<FireArrow>().damage = equipmentSlot.equipedItem.modifire;
        }
        else
        {
            GameObject currArrow = Instantiate(arrowPrefab, firePos.position, Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipmentSlot.equipedItem.modifire;
        }

        if (equipementManager.objBowThree.activeInHierarchy)
        {
            GameObject currArrow = Instantiate(arrowPrefab, firePos1.position, Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipmentSlot.equipedItem.modifire;

            currArrow = Instantiate(arrowPrefab, firePos2.position, Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipmentSlot.equipedItem.modifire;
        }

        item.currHealth -= 1;
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