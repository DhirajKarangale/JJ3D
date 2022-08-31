using UnityEngine;

public class AnimalMovement : NPC
{
    protected override void Move(float speed)
    {
        base.Move(speed);
        if (Vector3.Distance(myTransform.position, targetPos) > 7f)
        {
            myTransform.position += myTransform.forward * speed * Time.deltaTime;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(IEWalkIdle());
        }
    }

    internal override void Hurt()
    {
        base.Hurt();
        StopAllCoroutines();
        StartCoroutine(IERun());
    }
}