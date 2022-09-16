using UnityEngine;

public class BruteAnim : MonoBehaviour
{
    [SerializeField] CapsuleCollider capCollider;
    [SerializeField] Brute brute;

    public void ChangeAttack()
    {
        brute.ChangeAttack();
    }

    public void ChangeCollider()
    {
        capCollider.radius = 0.1f;
        capCollider.height = 0.1f;
        capCollider.center = new Vector3(0, 1, 0);
    }
}
