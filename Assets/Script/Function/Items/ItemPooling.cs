
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class ItemPooling : MonoBehaviour
{
    public static ItemPooling instance;
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject rockCrystal;
    [SerializeField] private GameObject toxicCrystal;
    [SerializeField] private GameObject infCrystal;
    [SerializeField] private GameObject minesCrystal;

    private Dictionary<ItemType, Queue<ItemEffect>> effectPool;
    private Queue<ItemObj> itemPool;


    private void Awake()
    {
        instance = this;
        Init();
        CreateItems();
    }

    private void TestSpawn()
    {

        ItemType t = (ItemType)Random.Range(0, 4);
      //  ItemType t = ItemType.RockFall;
        ItemObj obj= SpawnItem(t);
        Debug.Log("Spawn:" + t);
    }

    private void CreateItems()
    {
        for(int i =0;i<10;i++)
        {
            //ItemType t = (ItemType)Random.Range(0, 4);
            GameObject _item = Instantiate(item, transform);
            //Material m = GetVisual(t).GetComponent<MeshRenderer>().material;
            ItemObj io = _item.GetComponent<ItemObj>();            
            itemPool.Enqueue(io);
        }
    }

    internal ItemObj SpawnItem(ItemType itemType)
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 5, Random.Range(-5f, 5f));
        ItemObj itemObj = itemPool.Dequeue();
        itemObj.itemType = itemType;
        //Type
        itemObj.gameObject.transform.position = spawnPos;
         Material m = GetVisual(itemType).GetComponent<MeshRenderer>().sharedMaterial;
        itemObj.ChangeTypeAndShow(itemType, m);
        return itemObj;
    }


    internal void RecycleItem(ItemObj itemObj)
    {
        itemPool.Enqueue(itemObj);
        itemObj.disSpawn();
        TestSpawn();
    }

    private void Start()
    {
        TestSpawn();
    }





    private void Init()
    {
        effectPool = new Dictionary<ItemType, Queue<ItemEffect>>();
        effectPool.Add(ItemType.Toxic, new Queue<ItemEffect>());
        effectPool.Add(ItemType.RockFall, new Queue<ItemEffect>());
        effectPool.Add(ItemType.Inf, new Queue<ItemEffect>());
        effectPool.Add(ItemType.Mines, new Queue<ItemEffect>());

        itemPool = new Queue<ItemObj>();

    }

    public GameObject GetVisual(ItemType type)
    {
        switch (type)
        {
            case ItemType.RockFall:
            default:
                return rockCrystal;

            case ItemType.Toxic:
                return toxicCrystal;

            case ItemType.Inf:
                return infCrystal;

            case ItemType.Mines:
                return minesCrystal;
        }
    }

 

}


public enum ItemType { RockFall=3, Toxic=2, Inf=0, Mines=1}
