using System.Collections;
using UnityEngine;

public class NPC_New : MonoBehaviour
{
    [SerializeField] Animator animator;
    internal Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }

    private IEnumerator IERotate()
    {
        Quaternion newDir = Quaternion.Euler(0, Random.Range(-359, 359), 0);
        float timeSpan = 3;

        while (timeSpan > 0)
        {
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, newDir, Time.deltaTime);
            timeSpan -= Time.deltaTime;
            yield return null;
        }
    }


    internal void ChangePos()
    {
        StopAllCoroutines();
        StartCoroutine(IERotate());
    }

    internal void Dye()
    {
        animator.Play("Dye");
    }

    internal void Hurt()
    {
        animator.Play("Run");
    }
}