using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 5;

    private readonly Queue<GameObject> _objectPool = new Queue<GameObject>();

    private void Awake()
    {
        InitializeObjectPool();
    }

    private void InitializeObjectPool()
    {
        for (var i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(prefab, transform.position, Quaternion.identity);
            obj.gameObject.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }

    public GameObject GetPooledObject(Vector3 pos)
    {
        if (_objectPool.Count > 0)
        {
            var pooledObject = _objectPool.Dequeue();
            pooledObject.transform.position = pos;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        var newObj = Instantiate(prefab, pos, Quaternion.identity);
        newObj.gameObject.SetActive(true);
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        _objectPool.Enqueue(obj);
    }
    
}