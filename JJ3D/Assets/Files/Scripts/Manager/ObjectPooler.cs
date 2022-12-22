using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : Singleton<ObjectPooler>
{
    public Dictionary<string, Queue<Rigidbody>> poolDictonary;
    public List<Pool> pools;
    private Rigidbody obj;

    private void Start()
    {
        poolDictonary = new Dictionary<string, Queue<Rigidbody>>();

        foreach (Pool pool in pools)
        {
            Queue<Rigidbody> objectPool = new Queue<Rigidbody>();

            for (int i = 0; i < pool.size; i++)
            {
                obj = Instantiate(pool.prefab);
                obj.transform.SetParent(this.transform);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictonary.Add(pool.tag, objectPool);
        }
    }


    public Rigidbody SpwanObject(string tag, Vector3 pos)
    {
        obj = poolDictonary[tag].Dequeue();
        obj.transform.position = pos;
        obj.velocity = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.gameObject.SetActive(true);
        // spwanObj.GetComponent<IPooledObject>()?.OnObjectSpwan();

        poolDictonary[tag].Enqueue(obj);

        return obj;
    }
}


[System.Serializable]
public class Pool
{
    public string tag;
    public int size;
    public Rigidbody prefab;
}