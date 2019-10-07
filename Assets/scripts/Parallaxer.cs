using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour {

	class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t)
        {
            transform = t;
        }
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }
    [System.Serializable]
    public struct XSpawnRange
    {
        public float min;
        public float max;
    }

    public GameObject Prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public XSpawnRange xSpawnRange;
    public Vector3 defaultSpawnPos;
    public Vector3 removeSpawnPos;
    public bool spawnImmediate;
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    GameManager game;

    void Awake()
    {
        Configure();
    }
    void Start()
    {
        game = GameManager.Instance;
    }
    private void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnGameLife += OnGameLife;
    }
    private void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
        GameManager.OnGameLife -= OnGameLife;
    }
    void OnGameOverConfirmed()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }
    void OnGameLife()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }
    private void Update()
    {
        if (game.GameOver) return;

        Shift();
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;
        }
    }
    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }
    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) { return; }
        Vector3 pos = Vector3.zero;
        pos.y = (defaultSpawnPos.y * Camera.main.aspect) / targetAspect;
        pos.x = Random.Range(xSpawnRange.min, xSpawnRange.max);
        t.position = pos;
    }
    void SpawnImmediate()
    {
        Transform t = GetPoolObject();
        if (t == null) { return; }
        Vector3 pos = Vector3.zero;
        pos.y = ((immediateSpawnPos.y * Camera.main.aspect) / targetAspect);
        pos.x = Random.Range(xSpawnRange.min, xSpawnRange.max);
        t.position = pos;
        Spawn();
    }
    void Shift()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.localPosition += -Vector3.up * shiftSpeed * Time.deltaTime ;
            CheckDisposeObject(poolObjects[i]);

        }
    }
    void CheckDisposeObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.y < (removeSpawnPos.y * Camera.main.aspect) / targetAspect)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    }
    Transform GetPoolObject()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
