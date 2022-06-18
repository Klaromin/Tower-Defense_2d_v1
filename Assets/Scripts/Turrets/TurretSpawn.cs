using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawn : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnLocations = new List<GameObject>();
    
    private ObjectPooler _pooler;
    protected Turret _currentTurretLoaded;
    private int _turretnumber = 0;
    private int _initialSpawnLocations;
    void Start()
    {
        _pooler = GetComponent<ObjectPooler>();
         _initialSpawnLocations = spawnLocations.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.B)&& _turretnumber < _initialSpawnLocations)
        {
            LoadTurret();
        }
    }

    protected virtual void LoadTurret()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        int spawnLocation = Random.Range(0, spawnLocations.Count);
        
        newInstance.transform.localPosition = spawnLocations[spawnLocation].transform.position;
        newInstance.transform.SetParent(spawnLocations[spawnLocation].transform);
        _currentTurretLoaded = newInstance.GetComponent<Turret>();
        spawnLocations.RemoveAt(spawnLocation);
        _currentTurretLoaded.SpawnOwner = this;

        _turretnumber++;
        newInstance.SetActive(true);
        
    }

    
}
