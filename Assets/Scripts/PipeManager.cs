using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour {

    [SerializeField]
    private Transform transSpawn;

    private Coroutine spawnCoroutine;
    public bool bSpawning;
    public static PipeManager instance;
    private int iRandSpawnMin;

    private void Awake() {
        if (PipeManager.instance == null) {
            PipeManager.instance = this;
        } else if (PipeManager.instance != this) {
            Destroy(PipeManager.instance.gameObject);
            PipeManager.instance = this;
        }
        bSpawning = false;
    }

    [System.Serializable]
    public class Pool {
        public string strName;
        public GameObject goPrefab;
        public int iSize;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDict;

    private void Start() {
        PoolDict = new Dictionary<string, Queue<GameObject>>();
        for (int i = 0; i < pools.Count; i++) {
            Queue<GameObject> objects = new Queue<GameObject>();
            for (int j = 0; j < pools[i].iSize; j++) {
                GameObject obj = Instantiate(pools[i].goPrefab);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }
            PoolDict.Add(pools[i].strName, objects);
        }
        StartSpawning();
    }

    public GameObject SpawnObjFromPool(string _strName, Vector3 _v3Pos, Quaternion _qRot) {
        if (!PoolDict.ContainsKey(_strName)) {
            Debug.LogError("The pool dictionary does not contain: " + _strName);
            return null;
        }
        GameObject objToSpawn = PoolDict[_strName].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = _v3Pos;
        objToSpawn.transform.rotation = _qRot;

        if (objToSpawn.GetComponent<Pooled>()) {
            objToSpawn.GetComponent<Pooled>().OnObjectSpawn();
        }

        PoolDict[_strName].Enqueue(objToSpawn);

        return objToSpawn;
    }

    public void StartSpawning() {
        if (bSpawning) {
            return;
        }
        spawnCoroutine = StartCoroutine(IESpawnPipes());
    }

    public void StopSpawning() {
        StopCoroutine(spawnCoroutine);
    }

    private readonly WaitForSeconds wait = new WaitForSeconds(1f);

    private IEnumerator IESpawnPipes() {
        bSpawning = true;
        while (bSpawning) {
            yield return wait;
            if (Random.Range(iRandSpawnMin, 6) > 2) {
                iRandSpawnMin = 0;
                float fGapSize = Random.Range(1.25f, 3f);
                float fMidPoint = transSpawn.position.y + Random.Range(-1f, 1f);
                Vector3 v3BottomPos = new Vector3(transSpawn.position.x, (transSpawn.position.y + fMidPoint) - fGapSize, transSpawn.position.z);
                Vector3 v3TopPos = new Vector3(transSpawn.position.x, (transSpawn.position.y + fMidPoint) + fGapSize, transSpawn.position.z);
                Vector3 rot = transSpawn.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y, rot.z + 180);
                SpawnObjFromPool("Pipe", v3TopPos, Quaternion.Euler(rot));
                SpawnObjFromPool("Pipe", v3BottomPos, transSpawn.rotation);
                SpawnObjFromPool("PointsTrigger", new Vector3(transSpawn.position.x, transSpawn.position.y, (transSpawn.position.z + .5f)), transSpawn.rotation);
            } else {
                iRandSpawnMin++;
            }
        }
    }

}
