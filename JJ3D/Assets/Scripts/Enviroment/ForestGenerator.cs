using UnityEngine;

[System.Serializable]
public class ForestItem
{
    public GameObject item;
    public int itemCount;
    public int currCount;
}

public class ForestGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] grass;
    [SerializeField] ForestItem[] rocks;
    [SerializeField] ForestItem[] flowers;
    [SerializeField] ForestItem[] greenTrees;
    [SerializeField] ForestItem[] yellowTrees;
    [SerializeField] ForestItem[] pinkTrees;
    [SerializeField] ForestItem[] deadTrees;
    [SerializeField] ForestItem[] farmAnimals;
    [SerializeField] ForestItem[] dinasours;
    [SerializeField] ForestItem[] smallEnemies;
    [SerializeField] ForestItem[] midEnemies;
    [SerializeField] ForestItem[] chests;
    [SerializeField] GameObject testObj;
    int testCount = 0;

    private float lastHeight;

    // private void Start()
    // {
    //     ResetItemCount();
    // }

    public void Generate(Vector3[] vertices, Transform itemParent)
    {
        ResetItemCount();

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 itemPos = transform.TransformPoint(vertices[i]);
            float currHeight = itemPos.y;

            // Grass
            if (currHeight >= 0.35f && Random.value > 0.993f && (Mathf.Abs(lastHeight - currHeight) < 2.5f) && currHeight <= 3)
            {
                Vector3 grassPos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y, vertices[i].z + itemParent.position.z);
                GameObject currGrass = grass[Random.Range(0, grass.Length)];
                GameObject obj = Instantiate(currGrass, grassPos, Quaternion.identity);
                obj.transform.SetParent(itemParent.transform);
                obj.isStatic = true;
                RaycastHit hit;
                Ray ray = new Ray(obj.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hit.normal) * obj.transform.rotation;
                }

                grassPos += new Vector3(1, 0, 0);
                obj = Instantiate(currGrass, grassPos, Quaternion.identity);
                obj.transform.SetParent(itemParent.transform);
                obj.isStatic = true;
                ray = new Ray(obj.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hit.normal) * obj.transform.rotation;
                }

                grassPos += new Vector3(1, 0, 1);
                obj = Instantiate(currGrass, grassPos, Quaternion.identity);
                obj.transform.SetParent(itemParent.transform);
                obj.isStatic = true;
                ray = new Ray(obj.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hit.normal) * obj.transform.rotation;
                }

                grassPos += new Vector3(1, 0, -1);
                obj = Instantiate(currGrass, grassPos, Quaternion.identity);
                obj.transform.SetParent(itemParent.transform);
                obj.isStatic = true;
                ray = new Ray(obj.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hit.normal) * obj.transform.rotation;
                }

                grassPos += new Vector3(-1, 0, 0);
                obj = Instantiate(currGrass, grassPos, Quaternion.identity);
                obj.transform.SetParent(itemParent.transform);
                obj.isStatic = true;
                ray = new Ray(obj.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hit.normal) * obj.transform.rotation;
                }

                grassPos += new Vector3(-1, 0, 1);
                obj = Instantiate(currGrass, grassPos, Quaternion.identity);
                obj.transform.SetParent(itemParent.transform);
                obj.isStatic = true;
                ray = new Ray(obj.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hit.normal) * obj.transform.rotation;
                }

                grassPos += new Vector3(-1, 0, -1);
                obj = Instantiate(currGrass, grassPos, Quaternion.identity);
                obj.transform.SetParent(itemParent.transform);
                obj.isStatic = true;
                ray = new Ray(obj.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hit.normal) * obj.transform.rotation;
                }

                // Debug.Log("Grass");
            }

            // Trees
            if ((Mathf.Abs(lastHeight - currHeight) < 15) && (Random.value >= 0.9985f))
            {
                if (currHeight > 7.9f)
                {
                    ForestItem deadTree = deadTrees[Random.Range(0, deadTrees.Length)];
                    if (deadTree.currCount < deadTree.itemCount)
                    {
                        Vector3 deadTreePos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y - 1, vertices[i].z + itemParent.position.z);
                        GameObject obj = Instantiate(deadTree.item, deadTreePos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        deadTree.currCount++;
                        // Debug.Log("Dead Tree");
                    }
                }
                else if (currHeight > 6f)
                {
                    ForestItem yellowTree = yellowTrees[Random.Range(0, yellowTrees.Length)];
                    if (yellowTree.currCount < yellowTree.itemCount)
                    {
                        Vector3 yellowTreePos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y - 1, vertices[i].z + itemParent.position.z);
                        GameObject obj = Instantiate(yellowTree.item, yellowTreePos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        yellowTree.currCount++;
                        // Debug.Log("Yellow Tree");
                    }
                }
                else if (currHeight > 5f)
                {
                    ForestItem pinkTree = pinkTrees[Random.Range(0, pinkTrees.Length)];
                    if (pinkTree.currCount < pinkTree.itemCount)
                    {
                        Vector3 pinkTreePos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y - 1, vertices[i].z + itemParent.position.z);
                        GameObject obj = Instantiate(pinkTree.item, pinkTreePos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        pinkTree.currCount++;
                        // Debug.Log("Pink Tree");
                    }
                }
                else if (currHeight > 4.2f)
                {
                    ForestItem greenTree = greenTrees[Random.Range(0, greenTrees.Length)];
                    if (greenTree.currCount < greenTree.itemCount)
                    {
                        Vector3 greenTreePos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y - 1, vertices[i].z + itemParent.position.z);
                        GameObject obj = Instantiate(greenTree.item, greenTreePos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        greenTree.currCount++;
                        // Debug.Log("Green Tree");
                    }
                }
                else if (currHeight >= 1f)
                {
                    ForestItem flower = flowers[Random.Range(0, flowers.Length)];
                    if (flower.currCount < flower.itemCount)
                    {
                        Vector3 greenTreePos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y, vertices[i].z + itemParent.position.z);
                        GameObject obj = Instantiate(flower.item, greenTreePos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        flower.currCount++;
                        // Debug.Log("Flowers");
                    }
                }
                else if (currHeight >= 0.35f)
                {
                    ForestItem rock = rocks[Random.Range(0, rocks.Length)];
                    if (rock.currCount < rock.itemCount)
                    {
                        Vector3 rockPos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y, vertices[i].z + itemParent.position.z);
                        GameObject obj = Instantiate(rock.item, rockPos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        rock.currCount++;
                        // Debug.Log("Rock");
                    }
                }
            }

            // Farm Animals
            if (Random.value > 0.9999)
            {
                ForestItem farmAnimal = farmAnimals[Random.Range(0, farmAnimals.Length)];
                if (farmAnimal.currCount < farmAnimal.itemCount)
                {
                    Vector3 farmAnimalPos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y + 5, vertices[i].z + itemParent.position.z);
                    AnimalMovement currFarmAnimal = Instantiate(farmAnimal.item, farmAnimalPos, Quaternion.identity).GetComponent<AnimalMovement>();
                    currFarmAnimal.StartPos(vertices, itemParent);
                    currFarmAnimal.transform.SetParent(itemParent.transform);
                    farmAnimal.currCount++;
                }
            }

            // Dinasours
            if (Random.value > 0.99997f)
            {
                ForestItem dinasour = dinasours[Random.Range(0, dinasours.Length)];
                if (dinasour.currCount < dinasour.itemCount)
                {
                    Vector3 dinasourPos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y + 5, vertices[i].z + itemParent.position.z);
                    EnemyMovement currDinasour = Instantiate(dinasour.item, dinasourPos, Quaternion.identity).GetComponent<EnemyMovement>();
                    currDinasour.StartPos(vertices, itemParent);
                    currDinasour.transform.SetParent(itemParent.transform);
                    dinasour.currCount++;
                }
            }

            // Small Enemis
            if (Random.value > 0.99991f)
            {
                ForestItem smallEnemy = smallEnemies[Random.Range(0, smallEnemies.Length)];
                if (smallEnemy.currCount < smallEnemy.itemCount)
                {
                    Vector3 smallEnemyPos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y + 2, vertices[i].z + itemParent.position.z);
                    EnemyMovement currSmallEnemy = Instantiate(smallEnemy.item, smallEnemyPos, Quaternion.identity).GetComponent<EnemyMovement>();
                    currSmallEnemy.StartPos(vertices, itemParent);
                    currSmallEnemy.transform.SetParent(itemParent.transform);
                    smallEnemy.currCount++;
                }
            }

            // Mid Enemis
            if (Random.value > 0.99992f)
            {
                ForestItem midEnemy = midEnemies[Random.Range(0, midEnemies.Length)];
                if (midEnemy.currCount < midEnemy.itemCount)
                {
                    Vector3 midEnemyPos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y + 5, vertices[i].z + itemParent.position.z);
                    EnemyMovement currMidEnemy = Instantiate(midEnemy.item, midEnemyPos, Quaternion.identity).GetComponent<EnemyMovement>();
                    currMidEnemy.StartPos(vertices, itemParent);
                    currMidEnemy.transform.SetParent(itemParent.transform);
                    midEnemy.currCount++;
                }
            }

            // Chest
            if (Random.value > 0.9995f)
            {
                ForestItem chest = chests[Random.Range(0, chests.Length)];
                if (chest.currCount < chest.itemCount)
                {
                    Vector3 chestPos = new Vector3(vertices[i].x + itemParent.position.x, vertices[i].y + 4, vertices[i].z + itemParent.position.z);
                    GameObject obj = Instantiate(chest.item, chestPos, Quaternion.identity);
                    obj.transform.SetParent(itemParent.transform);
                    obj.isStatic = true;
                    chest.currCount++;
                }
            }

            // Test Object
            if (testObj && testCount < 1)
            {
                Vector3 testPos = new Vector3(0, 10, 0);
                EnemyMovement currTestObj = Instantiate(testObj, testPos, Quaternion.identity).GetComponent<EnemyMovement>();
                currTestObj.StartPos(vertices, itemParent);
                testCount++;
            }

            lastHeight = currHeight;
        }
    }

    private void ResetItemCount()
    {
        ResetCount(rocks);
        ResetCount(flowers);
        ResetCount(greenTrees);
        ResetCount(yellowTrees);
        ResetCount(pinkTrees);
        ResetCount(deadTrees);
        ResetCount(farmAnimals);
        ResetCount(dinasours);
        ResetCount(smallEnemies);
        ResetCount(midEnemies);
        ResetCount(chests);
    }

    private void ResetCount(ForestItem[] items)
    {
        foreach (ForestItem item in items)
        {
            item.itemCount = 1;
            item.currCount = 0;
        }
    }
}