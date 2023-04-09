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
    [SerializeField] RenderObject renderObject;
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
    [SerializeField] GameObject[] grass;
    [SerializeField] GameObject testObj;

    private int currRockCount;
    private int currPlantCount;
    private int currWoodCount;
    private float lastHeight;
    private int testCount = 0;
    private float currHeight;

    private Vector3 itemPos;
    private Vector3 grassPos;
    private GameObject obj;
    private ForestItem forestItem;
    private AnimalMovement animalMovement;
    private EnemyMovement enemyMovement;


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

        currRockCount = 0;
        currPlantCount = 0;
        currWoodCount = 0;
    }

    private void ResetCount(ForestItem[] items)
    {
        foreach (ForestItem item in items)
        {
            item.itemCount = 1;
            item.currCount = 0;
        }
    }


    public void Generate(Vector3[] vertices, Transform itemParent)
    {
        ResetItemCount();

        for (int i = 0; i < vertices.Length; i++)
        {
            currHeight = transform.TransformPoint(vertices[i]).y;

            itemPos.x = vertices[i].x + itemParent.position.x;
            itemPos.y = vertices[i].y;
            itemPos.z = vertices[i].z + itemParent.position.z;

            //  Render Grass
            if (currHeight >= 0.4f && Random.value > 0.998f && (Mathf.Abs(lastHeight - currHeight) < 2.5f) && currHeight <= 3f)
            {
                renderObject.AddGrass(itemPos);
                // Debug.Log("Grass");
            }

            //  Render Rock
            if ((currRockCount < 6) && (currHeight >= 0.3f) && (currHeight < 4) && (Random.value > 0.9995f))
            {
                renderObject.AddRock(itemPos);
                currRockCount++;
            }

            // Render Plant
            if ((currPlantCount < 6) && (currHeight >= 2.5) && (currHeight < 6.5f) && (Random.value > 0.999f))
            {
                renderObject.AddPlant(itemPos);
                currPlantCount++;
            }

            // Render Wood
            if ((currWoodCount < 6) && (currHeight >= 6.5f) && (Random.value > 0.999f))
            {
                renderObject.AddWood(itemPos);
                currWoodCount++;
            }

            if ((Mathf.Abs(lastHeight - currHeight) < 15) && (Random.value >= 0.9987f))
            {
                itemPos.y--; // Trees should spwan below ground at -1 height

                if (currHeight > 7.9f)
                {
                    forestItem = deadTrees[Random.Range(0, deadTrees.Length)];
                    if (forestItem.currCount < forestItem.itemCount)
                    {
                        obj = Instantiate(forestItem.item, itemPos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        forestItem.currCount++;
                        // Debug.Log("Dead Tree");
                    }
                }
                else if (currHeight > 6f)
                {
                    forestItem = yellowTrees[Random.Range(0, yellowTrees.Length)];
                    if (forestItem.currCount < forestItem.itemCount)
                    {
                        obj = Instantiate(forestItem.item, itemPos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        forestItem.currCount++;
                        // Debug.Log("Yellow Tree");
                    }
                }
                else if (currHeight > 5f)
                {
                    forestItem = pinkTrees[Random.Range(0, pinkTrees.Length)];
                    if (forestItem.currCount < forestItem.itemCount)
                    {
                        obj = Instantiate(forestItem.item, itemPos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        forestItem.currCount++;
                        // Debug.Log("Pink Tree");
                    }
                }
                else if (currHeight > 4.4f)
                {
                    forestItem = greenTrees[Random.Range(0, greenTrees.Length)];
                    if (forestItem.currCount < forestItem.itemCount)
                    {
                        obj = Instantiate(forestItem.item, itemPos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        forestItem.currCount++;
                        // Debug.Log("Green Tree");
                    }
                }
                else if (currHeight > 3.9f)
                {
                    itemPos.y++; // Rocks should spwan on ground at 0 height
                    forestItem = rocks[Random.Range(0, rocks.Length)];
                    if (forestItem.currCount < forestItem.itemCount)
                    {
                        obj = Instantiate(forestItem.item, itemPos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        forestItem.currCount++;
                        // Debug.Log("Rock");
                    }
                }
                else if (currHeight >= 1f)
                {
                    forestItem = flowers[Random.Range(0, flowers.Length)];
                    if (forestItem.currCount < forestItem.itemCount)
                    {
                        itemPos.y++;
                        obj = Instantiate(forestItem.item, itemPos, Quaternion.identity);
                        obj.transform.SetParent(itemParent.transform);
                        obj.isStatic = true;
                        forestItem.currCount++;
                        // Debug.Log("Flowers");
                    }
                }
            }

            itemPos.y += 4; // animals should spwan above ground at +5 height

            // Farm Animals
            // if (Random.value > 0.9999)
            // {
            //     forestItem = farmAnimals[Random.Range(0, farmAnimals.Length)];
            //     if (forestItem.currCount < forestItem.itemCount)
            //     {
            //         animalMovement = Instantiate(forestItem.item, itemPos, Quaternion.identity).GetComponent<AnimalMovement>();
            //         animalMovement.StartPos(vertices, itemParent);
            //         animalMovement.transform.SetParent(itemParent.transform);
            //         forestItem.currCount++;
            //         // AddCulling(animalMovement.gameObject);
            //     }
            // }

            // Dinasours
            // if (Random.value > 0.99997f)
            // {
            //     forestItem = dinasours[Random.Range(0, dinasours.Length)];
            //     if (forestItem.currCount < forestItem.itemCount)
            //     {
            //         enemyMovement = Instantiate(forestItem.item, itemPos, Quaternion.identity).GetComponent<EnemyMovement>();
            //         enemyMovement.StartPos(vertices, itemParent);
            //         enemyMovement.transform.SetParent(itemParent.transform);
            //         forestItem.currCount++;
            //         // AddCulling(enemyMovement.gameObject);
            //     }
            // }

            // Small Enemis
            // if (Random.value > 0.99991f)
            // {
            //     forestItem = smallEnemies[Random.Range(0, smallEnemies.Length)];
            //     if (forestItem.currCount < forestItem.itemCount)
            //     {
            //         enemyMovement = Instantiate(forestItem.item, itemPos, Quaternion.identity).GetComponent<EnemyMovement>();
            //         enemyMovement.StartPos(vertices, itemParent);
            //         enemyMovement.transform.SetParent(itemParent.transform);
            //         forestItem.currCount++;
            //         // AddCulling(enemyMovement.gameObject);
            //     }
            // }

            // // Mid Enemis
            // if (Random.value > 0.99992f)
            // {
            //     forestItem = midEnemies[Random.Range(0, midEnemies.Length)];
            //     if (forestItem.currCount < forestItem.itemCount)
            //     {
            //         enemyMovement = Instantiate(forestItem.item, itemPos, Quaternion.identity).GetComponent<EnemyMovement>();
            //         enemyMovement.StartPos(vertices, itemParent);
            //         enemyMovement.transform.SetParent(itemParent.transform);
            //         forestItem.currCount++;
            //         // AddCulling(enemyMovement.gameObject);
            //     }
            // }

            // Chest
            if (Random.value > 0.9995f)
            {
                forestItem = chests[Random.Range(0, chests.Length)];
                if (forestItem.currCount < forestItem.itemCount)
                {
                    obj = Instantiate(forestItem.item, itemPos, Quaternion.identity);
                    obj.transform.SetParent(itemParent.transform);
                    obj.isStatic = true;
                    forestItem.currCount++;
                    // AddCulling(obj);
                }
            }

            // Test Object
            if (testObj && testCount < 1)
            {
                Vector3 testPos = new Vector3(0, 10, 0);
                NPC_New currTestObj = Instantiate(testObj, testPos, Quaternion.identity).GetComponent<NPC_New>();
                // currTestObj.StartPos(vertices, itemParent);
                testCount++;
                // AddCulling(currTestObj.gameObject);
            }

            lastHeight = currHeight;
        }
    }
}