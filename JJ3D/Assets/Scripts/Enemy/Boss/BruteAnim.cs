using UnityEngine;

public class BruteAnim : MonoBehaviour
{
    [SerializeField] Brute brute;
    public void ChangeAttack()
    {
        brute.ChangeAttack();
    }
}
