using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    [SerializeField] string fruit;
    [SerializeField] Transform[] spwanPoints;
    [SerializeField] int minItems;
    private Rigidbody[] fruits;
    private ObjectPooler objectPooler;
    private int currFruits;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
        fruits = new Rigidbody[spwanPoints.Length];
        currFruits = 0;
        Invoke("SpwanFruits", 2);
    }

    private void SpwanFruits()
    {
        int itemCount = Random.Range(minItems, spwanPoints.Length);

        for (int i = 0; i < itemCount; i++)
        {
            if (!fruits[i] && (Random.value > 0.5f))
            {
                fruits[i] = objectPooler.SpwanObject(fruit, spwanPoints[i].position).GetComponent<Rigidbody>();
                // fruits[i].transform.SetParent(this.transform);
                fruits[i].isKinematic = true;
                fruits[i].GetComponent<Fruit>().SetFruit(fruits[i], i, this);
                currFruits++;
            }
        }
    }

    public void RemoveFruit(int index)
    {
        fruits[index] = null;
        currFruits--;
        if (currFruits < minItems)
        {
            CancelInvoke();
            Invoke("SpwanFruits", 20);
        }
    }
}
