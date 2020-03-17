using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }
    #region ObjectPoolerStaticInstance
    public static ObjectPooler CentralObjectPool;

    private void Awake()
    {
        CentralObjectPool = this;
    }

    #endregion

    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    public List<Pool> Pools;

    private GameObject ObjectToSpawn;
    private GameObject Obj;

    // Start is called before the first frame update
    void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool CurrentPool in Pools)
        {
            Queue<GameObject> CurrentObjectPool = new Queue<GameObject>();
            for(int i = 0; i < CurrentPool.Size; i++)
            {
                Obj = Instantiate(CurrentPool.Prefab);
                if (!(Obj.GetComponent<BaseEnemy>() || Obj.GetComponent<BaseWeapon>()))
                {
                    Obj.SetActive(false);
                }
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
