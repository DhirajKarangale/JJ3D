using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Refrence")]
    [SerializeField] Player player;
    private Camera cam;
    private Animator animator;

    [Header("Sward")]
    [SerializeField] AnimationClip[] swardClips;

    [Header("Bow")]
    [SerializeField] Transform firePos;
    [SerializeField] Transform firePos1;
    [SerializeField] Transform firePos2;
    [SerializeField] float force;
    internal bool isAming;
    internal bool isShoothing;

    [Header("Audio Clips")]
    [SerializeField] AudioClip clipSwipe;
    [SerializeField] AudioClip clipArrow;
    [SerializeField] float coolDownTime = 0.8f;

    private AnimatorOverrideController overrideController;
    private GameManager gameManager;
    private ObjectPooler objectPooler;
    private EquipmentManager equipementManager;
    private Rigidbody currArrow;
    private float currCoolDownTime;

    private Ray ray;
    private Vector3 shootDir;

    private float requiredView;
    [SerializeField] float camSpeed;

    public event Action OnAttack;

    private void Start()
    {
        cam = player.cam;
        animator = player.animator;
        gameManager = GameManager.instance;
        objectPooler = ObjectPooler.instance;
        equipementManager = gameManager.equipementManager;
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        currCoolDownTime = coolDownTime;
        requiredView = 60;
        cam.fieldOfView = 60;
    }

    private void Update()
    {
        if (gameManager.isGameOver || (Time.timeScale == 0))
        {
            cam.fieldOfView = 60;
            return;
        }

        if (Input.GetKeyDown(KeyCode.F)) Attack();
        ShootBow();
        Scope();
        currCoolDownTime -= Time.deltaTime;
    }

    public void Attack()
    {
        if ((currCoolDownTime <= 0))
        {
            if (equipementManager.isSwardActive) SwardAttack();
            else if (equipementManager.isBowActive) BowPunch();
        }
    }

    private void SwardAttack()
    {
        animator.Play("SwardAttack");
        overrideController[swardClips[0].name] = swardClips[UnityEngine.Random.Range(0, swardClips.Length)];
        OnAttack?.Invoke();
        currCoolDownTime = coolDownTime;
    }

    private void BowPunch()
    {
        animator.Play("BowPunch");
        OnAttack?.Invoke();
        currCoolDownTime = coolDownTime;
    }

    private void ShootBow()
    {
        if (equipementManager.isBowActive && (Time.timeScale != 0))
        {
            BowAttack();
        }
        else
        {
            animator.SetBool("isAming", false);
            animator.SetBool("isShoothing", false);
            ReSetRotation();
            ResetView();
        }
    }

    private void BowAttack()
    {
        if (Time.timeScale == 0) return;

        // (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        if (Input.GetMouseButtonDown(0) && (currCoolDownTime <= 0))
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

    private void ReSetRotation()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Accessed by Anim Event
    public void ShootArrow()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        shootDir = (ray.GetPoint(1000) - firePos.transform.position).normalized;

        if (equipementManager.objBowFire.activeInHierarchy)
        {
            // currArrow = Instantiate(fireArrowPrefab, firePos.position, Quaternion.identity);
            currArrow = objectPooler.SpwanObject("FireArrow", firePos.position);
            // currArrow.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.transform.localRotation = Quaternion.LookRotation(firePos.forward);
            currArrow.AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<FireArrow>().damage = equipementManager.slotWeapon.itemData.modifier;
        }
        else
        {
            // GameObject currArrow = Instantiate(arrowPrefab, firePos.position, Quaternion.identity);
            currArrow = objectPooler.SpwanObject("Arrow", firePos.position);
            currArrow.transform.localRotation = Quaternion.LookRotation(firePos.forward);
            currArrow.AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipementManager.slotWeapon.itemData.modifier;
        }

        if (equipementManager.objBowThree.activeInHierarchy)
        {
            // GameObject currArrow = Instantiate(arrowPrefab, firePos1.position, Quaternion.identity);
            currArrow = objectPooler.SpwanObject("Arrow", firePos1.position);
            // currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.transform.localRotation = Quaternion.LookRotation(firePos.forward);
            currArrow.AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipementManager.slotWeapon.itemData.modifier;

            currArrow = objectPooler.SpwanObject("Arrow", firePos2.position);
            // currArrow.transform.rotation = Quaternion.LookRotation(firePos.forward);
            currArrow.transform.localRotation = Quaternion.LookRotation(firePos.forward);
            currArrow.AddForce(shootDir * force, ForceMode.Impulse);
            currArrow.GetComponent<PlayerWeapon>().damage = equipementManager.slotWeapon.itemData.modifier;
        }

        Invoke("ReSetRotation", 0.6f);
        currCoolDownTime = coolDownTime;
        isAming = false;
        isShoothing = false;

        player.PlayAudio(clipArrow, 0.5f);
        OnAttack?.Invoke();
        player.ResetSpeed();
    }

    // Accessed by Anim Event
    public void WalkSound()
    {
        player.playerMovement.WalkSound();
    }

    // Accessed by Anim Event
    public void SwipeSound()
    {
        player.PlayAudio(clipSwipe);
    }
}