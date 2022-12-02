using UnityEngine;

public class Fruit : MonoBehaviour
{
    private FruitGenerator fruitGenerator;
    private Rigidbody rigidBody;
    private int index;

    public void SetFruit(Rigidbody rigidBody, int index, FruitGenerator fruitGenerator)
    {
        this.index = index;
        this.rigidBody = rigidBody;
        this.fruitGenerator = fruitGenerator;
    }

    private void RemoveFruit()
    {
        GameManager.instance.effects.HitSound(transform.position);
        if (fruitGenerator)
        {
            rigidBody.isKinematic = false;
            fruitGenerator.RemoveFruit(index);
        }
        Destroy(GetComponent<Fruit>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        RemoveFruit();
    }
}
