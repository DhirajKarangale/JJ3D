using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Refrence")]
    [SerializeField] Camera cam;
    [SerializeField] Animator animator;
    [SerializeField] CapsuleCollider capsuleCollider;
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

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipSwipe;
    [SerializeField] AudioClip clipArrow;

    private AnimatorOverrideController overrideController;
    private GameManager gameManager;
    private EquipmentManager equipementManager;
    private float coolDownTime;

    private float requiredView;
    [SerializeField] float camSpeed;

    public event Action OnAttack;

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
        ShootBow();
        coolDownTime -= Time.deltaTime;
        Scope();
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
        coolDownTime = 1.1f;
        animator.Play("SwardAttack");
        overrideController[swardClips[0].name] = swardClips[UnityEngine.Random.Range(0, swardClips.Length)];
        OnAttack?.Invoke();
    }

    private void BowPunch()
    {
        animator.Play("BowPunch");
        coolDownTime = 1.1f;
        OnAttack?.Invoke();
    }

    private void ShootBow()
    {
        if (equipementManager.isBowActive)
        {
            BowAttack();
        }
        else
        {
            animator.SetBool("isAming", false);
            animator.SetBool("isShoothing", false);
            ReSetRotation();
        }
    }

    private void BowAttack()
    {
        if (Time.timeScale == 0) return;

        //(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        if (Input.GetMouseButtonDown(0) && (coolDownTime <= 0))
        {
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
            currArrow.GetComponent<FireArrow>().damage = equipementManager.slotWeapon.item.itemData.modifier;
        }
        else
        {
            GameObject currArrow = Instantiate(arrowPrefab, firePos.position, Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipementManager.slotWeapon.item.itemData.modifier;
        }

        if (equipementManager.objBowThree.activeInHierarchy)
        {
            GameObject currArrow = Instantiate(arrowPrefab, firePos1.position, Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipementManager.slotWeapon.item.itemData.modifier;

            currArrow = Instantiate(arrowPrefab, firePos2.position, Quaternion.identity);
            currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.GetComponent<Rigidbody>().AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipementManager.slotWeapon.item.itemData.modifier;
        }

        // item.currHealth -= 1;
        Invoke("ReSetRotation", 0.6f);
        coolDownTime = 1.1f;
        isAming = false;
        isShoothing = false;

        audioSource.PlayOneShot(clipArrow);
        OnAttack?.Invoke();
    }

    private void DestroyBow()
    {
        animator.SetBool("isAming", false);
        animator.SetBool("isShoothing", false);
        isAming = false;
        isShoothing = false;
    }

    private void ReSetRotation()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
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