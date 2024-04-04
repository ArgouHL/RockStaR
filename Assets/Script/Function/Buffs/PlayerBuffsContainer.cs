using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBuffsContainer : MonoBehaviour
{

    private int maxBuffNum = 15;
    private Dictionary<BuffType, Buff> buffs;
    private Dictionary<BuffType, DeBuff> deBuffs;
    private delegate void BuffUpdate();
    private BuffUpdate updateEvent;
    internal PlayerCtr playerCtr => GetComponent<PlayerCtr>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        buffs = new Dictionary<BuffType, Buff>();
        deBuffs = new Dictionary<BuffType, DeBuff>();
        updateEvent = null;
    }

    internal void AddBuff(Buff buff)
    {
        if (buffs.ContainsKey(buff.buffType))
        {
            Debug.Log("B"+buffs[buff.buffType].duration);
            buffs[buff.buffType].duration = buff.duration;
            Debug.Log("A"+buffs[buff.buffType].duration);
            return;
        }

        buffs.Add(buff.buffType, buff);
        updateEvent += buff.OnUpdate;
        buff.OnAdded();
    }

    internal void AddDeBuff(DeBuff buff)
    {
        if (buffs.ContainsKey(BuffType.inf))
            return;
        if (deBuffs.ContainsKey(buff.buffType))
        {
            Debug.Log("B" + deBuffs[buff.buffType].duration);
            deBuffs[buff.buffType].duration = buff.duration;
            Debug.Log("A" + deBuffs[buff.buffType].duration);
            return;
        }

        deBuffs.Add(buff.buffType, buff);
        updateEvent += buff.OnUpdate;
        buff.OnAdded();
    }

    //internal void EarlyEnd(BuffBase buff)
    //{
    //    Remove(buff);
    //}


    internal void RemoveBuff(Buff buff)
    {
        if (!buffs.ContainsKey(buff.buffType))
            return;
        buffs.Remove(buff.buffType);
        updateEvent -= buff.OnUpdate;
        // buff.OnCancel();
    }

    internal void RemoveDeBuff(DeBuff buff)
    {
        if (!deBuffs.ContainsKey(buff.buffType))
            return;
        deBuffs.Remove(buff.buffType);
        updateEvent -= buff.OnUpdate;
        // buff.OnCancel();
    }

    private void Update()
    {
        updateEvent?.Invoke();
    }

    internal Coroutine StartCoro(IEnumerator ie)
    {
        Coroutine coro = StartCoroutine(ie);
        return coro;
    }

    internal bool HaveBuff(BuffType buffType)
    {
        return buffs.ContainsKey(buffType);
    }
}

public enum BuffType { inf , slow }

public class InfBuff : Buff
{
    public InfBuff(PlayerBuffsContainer _player, float _duration) : base(_player)
    {
        player = _player;
        buffName = "InfBuff";
        duration = _duration;
        buffType = BuffType.inf;
    }


    protected internal override void OnAdded()
    {
        Debug.Log("infStart");
        StartCountDown();
    }
    protected internal override void OnUpdate()
    {

    }
    protected override void OnExpried()
    {

        Debug.Log("infEnd");
    }
    protected override void OnCancel()
    {

    }
}

public class SlowBuff : DeBuff
{
    public SlowBuff(PlayerBuffsContainer _player, float _duration) : base(_player)
    {
        player = _player;
        buffName = "SlowBuff";
        duration = _duration;
        buffType = BuffType.slow;
    }


    protected internal override void OnAdded()
    {

        Debug.Log("Slowed");
        StartCountDown();
        player.playerCtr.slowFactor = 0.3f;
    }
    protected internal override void OnUpdate()
    {
        
    }
    protected override void OnExpried()
    {
        Debug.Log("SlowEnd");
        player.playerCtr.slowFactor = 1;
       
    }
    protected override void OnCancel()
    {
        player.playerCtr.slowFactor = 1;
    }
}




