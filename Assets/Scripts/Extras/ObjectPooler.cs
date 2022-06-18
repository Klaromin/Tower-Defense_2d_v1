using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab; //seçeceğimiz herhangi bir prefab için bir alan oluşturuyoruz.
    [SerializeField] private int poolSize = 10; 

    private List<GameObject> _pool;
    private GameObject _poolContainer; //parentumuz

    private void Awake()
    {
        _pool = new List<GameObject>();
        _poolContainer = new GameObject($"Pool - {prefab.name}");
        
        CreatePooler();
    }

    private void CreatePooler()
    {
        for (int i = 0; i < poolSize; i++) //seçtiğimiz poolsize kadar aşağıda ayarladığımız özelliklerde obje ekliyoruz.
        {
            _pool.Add(CreateInstance());
        }
    }
    
    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        newInstance.transform.SetParent(_poolContainer.transform);
        newInstance.SetActive(false); //başlangıçta inaktif şekilde yaratıp gönderiyoruz.
        return newInstance;
    }

    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                return _pool[i];
            }
        }
        
        return CreateInstance();
    }

    public static void ReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
    }

    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
    }
}
