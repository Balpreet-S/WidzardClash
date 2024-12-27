using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> pool;
    private GameObject prefab;

    public ObjectPool(int size, GameObject prefab)
    {
        this.prefab = prefab;
        pool = new Queue<T>();

        for (int i = 0; i < size; i++)
        {
            T obj = Object.Instantiate(prefab).GetComponent<T>();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            // Expand pool if needed
            T obj = Object.Instantiate(prefab).GetComponent<T>();
            return obj;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}





