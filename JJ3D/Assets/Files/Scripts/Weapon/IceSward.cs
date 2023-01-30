using UnityEngine;
using System.Collections;

public class IceSward : MonoBehaviour
{
    [SerializeField] GameObject freezEffect;
    private Animator animator;
    private Rigidbody rigidBody;
    private AnimalMovement animalMovement;
    private EnemyMovement enemyMovement;
    private GameObject currFreezEffect;

    public void Freez(Rigidbody rigidBody)
    {
        StartCoroutine(IEFreez(rigidBody));
    }

    private IEnumerator IEFreez(Rigidbody rigidBody)
    {
        if (rigidBody.constraints != RigidbodyConstraints.FreezeAll)
        {
            this.rigidBody = rigidBody;
            rigidBody.velocity = Vector3.zero;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;

            animalMovement = rigidBody.GetComponent<AnimalMovement>();
            if (animalMovement) animalMovement.enabled = false;

            enemyMovement = rigidBody.GetComponent<EnemyMovement>();
            if (enemyMovement) enemyMovement.enabled = false;

            animator = rigidBody.GetComponent<Animator>();
            if(animator == null) animator = rigidBody.GetComponentInChildren<Animator>();
            if (animator) animator.enabled = false;

            currFreezEffect = Instantiate(freezEffect, rigidBody.transform.position, Quaternion.identity);
            currFreezEffect.transform.SetParent(rigidBody.transform);

            yield return new WaitForSecondsRealtime(5);

            if (rigidBody)
            {
                rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

                if (animalMovement) animalMovement.enabled = true;
                if (enemyMovement) enemyMovement.enabled = true;
                if (animator) animator.enabled = true;

                Destroy(currFreezEffect);
            }
        }
    }

    public void DeFreez()
    {
        if (rigidBody)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidBody.velocity = Vector3.zero;

            if (animalMovement) animalMovement.enabled = true;
            if (enemyMovement) enemyMovement.enabled = true;
            if (animator) animator.enabled = true;

            Destroy(currFreezEffect);
        }
    }
}
