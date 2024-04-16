using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class FrogSpawnManager : MonoBehaviour
{
    public static FrogSpawnManager Instance;

    [SerializeField] private GameObject smallFrogSpawnerGameObject;
    [SerializeField] private GameObject mediumFrogSpawnerGameObject;
    [SerializeField] private GameObject bigFrogSpawnerGameObject;

    private Bounds _smallFrogBounds;
    private Bounds _mediumFrogBounds;
    private Bounds _bigFrogBounds;

    private ObjectPool _smallFrogPool;
    private ObjectPool _mediumFrogPool;
    private ObjectPool _bigFrogPool;

    private int _smallWave = 1;
    [SerializeField] private int maxSmallFrogs = 10;
    
    private int _mediumWave = 0;
    [SerializeField] private int maxMediumFrogs = 8;
    
    private int _bigWave = 0;
    [SerializeField] private int maxBigFrogs = 6;
    private int _currentFrogs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _smallFrogPool = smallFrogSpawnerGameObject.GetComponent<ObjectPool>();
        _smallFrogBounds = smallFrogSpawnerGameObject.GetComponent<SpriteRenderer>().bounds;
        _mediumFrogPool = mediumFrogSpawnerGameObject.GetComponent<ObjectPool>();
        _mediumFrogBounds = mediumFrogSpawnerGameObject.GetComponent<SpriteRenderer>().bounds;
        _bigFrogPool = bigFrogSpawnerGameObject.GetComponent<ObjectPool>();
        _bigFrogBounds = bigFrogSpawnerGameObject.GetComponent<SpriteRenderer>().bounds;
    }

    private void Update()
    {
        if (_currentFrogs <= 2)
        {
            CheckWave();
            SpawnSmallFrogs();
            SpawnMediumFrogs();
            SpawnBigFrogs();
            if (_smallWave < maxSmallFrogs)
            {
                _smallWave++;
            }
        }
    }

    private void CheckWave()
    {
        if (_smallWave == maxSmallFrogs && _mediumWave == maxMediumFrogs && _bigWave == maxBigFrogs) return;
        if (_smallWave == maxSmallFrogs && _mediumWave < maxMediumFrogs)
        {
            _smallWave = 1;
            _mediumWave++;
        }
        else if (_mediumWave == maxMediumFrogs && _bigWave < maxBigFrogs)
        {
            _smallWave = 1;
            _mediumWave = 1;
            _bigWave++;
        }
    }

    private void SpawnSmallFrogs()
    {
        for (int i = 0; i < _smallWave; i++)
        {
            GetFrogFromPool(_smallFrogPool, _smallFrogBounds);
        }
    }
    
    private void SpawnMediumFrogs()
    {
        for (int i = 0; i < _mediumWave; i++)
        {
            GetFrogFromPool(_mediumFrogPool, _mediumFrogBounds);
        }
    }

    private void SpawnBigFrogs()
    {
        for (int i = 0; i < _bigWave; i++)
        {
            GetFrogFromPool(_bigFrogPool, _bigFrogBounds);
        }
    }

    private void GetFrogFromPool(ObjectPool pool, Bounds bounds)
    {
        var randomPositions = GetRandomPos(bounds);

        pool.GetPooledObject(randomPositions);
        _currentFrogs++;
    }

    private Vector3 GetRandomPos(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y), 0.12f);
    }

    public void ReturnFrogToPool(GameObject frog)
    {
        switch (frog.tag)
        {
            case "smallFrog":
                _smallFrogPool.ReturnObjectToPool(frog);
                break;
            case "mediumFrog":
                _mediumFrogPool.ReturnObjectToPool(frog);
                break;
            case "bigFrog":
                _bigFrogPool.ReturnObjectToPool(frog);
                break;
        }

        _currentFrogs--;
    }
}