using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Transform bar;
    [SerializeField] SpriteRenderer spriteBar;
    [SerializeField] Gradient gradient;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    internal void SetBar(float health)
    {
        spriteBar.color = gradient.Evaluate(health);
        if (health > 0)
        {
            obj.SetActive(true);
            bar.localScale = new Vector3(health, 1, 1);
            CancelInvoke();
            Invoke(nameof(DesableHealthBar), 20);
        }
        else
        {
            DesableHealthBar();
        }
    }

    private void DesableHealthBar()
    {
        obj.SetActive(false);
    }
}
