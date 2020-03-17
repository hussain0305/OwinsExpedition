using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler_Canvas : MonoBehaviour
{

    [System.Serializable]
    public class CanvasPool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }
    #region ObjectPoolerCanvasStaticInstance
    public static ObjectPooler_Canvas CanvasObjectPool;

    private void Awake()
    {
        CanvasObjectPool = this;
    }

    #endregion

    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    public List<CanvasPool> Pools;
    private GameObject CanvasObject;
    private GameObject Obj;
    private GameObject ObjectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        CanvasObject = GameObject.Find("Canvas");
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (CanvasPool CurrentPool in Pools)
        {
            Queue<GameObject> CurrentObjectPool = new Queue<GameObject>();
            for (int i = 0; i < CurrentPool.Size; i++)
            {
                Obj = Instantiate(CurrentPool.Prefab);
                Obj.transform.SetParent(CanvasObject.transform);
                Obj.SetActive(false);
                CurrentObjectPool.Enqueue(Obj);
            }

            PoolDictionary.Add(CurrentPool.Tag, CurrentObjectPool);
        }
    }

    public GameObject SpawnFromPool(string Tag, Vector3 Position, Quaternion Rotation)
    {
        if (!PoolDictionary.ContainsKey(Tag))
        {
            return null;
        }
        ObjectToSpawn = PoolDictionary[Tag].Dequeue();
        ObjectToSpawn.transform.position = Position;
        ObjectToSpawn.transform.rotation = Rotation;
        ObjectToSpawn.SetActive(true);

        PoolDictionary[Tag].Enqueue(ObjectToSpawn);

        return ObjectToSpawn;
    }
}
