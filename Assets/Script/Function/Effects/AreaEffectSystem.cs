using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectSystem : MonoBehaviour
{
    public static AreaEffectSystem instance;
    [SerializeField] private  GameObject mine;
    [SerializeField] private GameObject toxic;
    [SerializeField] private GameObject rockFall;

    private void Awake()
    {
        instance = this;
    }

    internal void SetMine(Vector3 position, int playerID)
    {
        GameObject _mine = Instantiate(mine, position, Quaternion.identity, this.transform);
        _mine.GetComponent<MineEffect>().EffectStart(playerID);
    }

    internal void ShootToxic(Vector3 forward,Vector3 playerOrignal)
    {
        GameObject _toxic = Instantiate(toxic, playerOrignal+ forward*5, Quaternion.identity, this.transform);
        _toxic.GetComponent<SlowToxic>().EffectStart();
    }
    internal void DropRock()
    {
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-6f, 6f), 0, UnityEngine.Random.Range(-6f, 6f));
        GameObject _rockFall = Instantiate(rockFall, spawnPos, Quaternion.identity, this.transform);
        _rockFall.GetComponent<RockDrop>().EffectStart();
    }
}
