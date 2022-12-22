using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : Singleton<ObjectPooler>
{
    public Dictionary<string, Queue<Rigidbody>> poolDictonary;
    public List<Pool> pools;

    private void Start()
    {
        poolDictonary = new Dictionary<string, Queue<Rigidbody>>();

        foreach (Pool pool in pools)
        {
            Queue<Rigidbody> objectPool = new Queue<Rigidbody>();

            for (int i = 0; i < pool.size; i++)
            {
                Rigidbody obj = Instantiate(pool.prefab);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictonary.Add(pool.tag, objectPool);
        }
    }


    public GameObject SpwanObject(string tag, Vector3 pos)
    {
        Rigidbody spwanObj = poolDictonary[tag].Dequeue();
        spwanObj.transform.position = pos;
        spwanObj.velocity = Vector3.zero;
        spwanObj.transform.rotation = Quaternion.identity;
        spwanObj.gameObject.SetActive(true);
        // spwanObj.GetComponent<IPooledObject>()?.OnObjectSpwan();

        poolDictonary[tag].Enqueue(spwanObj);

        return spwanObj.gameObject;
    }
}


[System.Serializable]
public class Pool
{
    public string tag;
    public int size;
    public Rigidbody prefab;
}