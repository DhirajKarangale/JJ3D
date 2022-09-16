using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] objFruit;
    [SerializeField] Transform[] spwanPoints;
    [SerializeField] int minItems;
    private int currFruits;
    private Rigidbody[] fruits;

    private void Start()
    {
        fruits = new Rigidbody[spwanPoints.Length];
        currFruits = 0;
        SpwanFruits();
    }

    private void SpwanFruits()
    {

        int itemCount = minItems + (Random.Range(0, spwanPoints.Length - currFruits - minItems + 1));

        for (int i = 0; i < itemCount; i++)
        {
            if (!fruits[i] && (Random.value > 0.55f))
            {
                fruits[i] = Instantiate(objFruit[Random.Range(0, objFruit.Length)], spwanPoints[i].position, Quaternion.identity).GetComponent<Rigidbody>();
                fruits[i].transform.SetParent(this.transform);
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
