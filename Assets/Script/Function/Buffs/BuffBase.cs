using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
{
    internal string buffName;
    internal BuffType buffType;
    internal float duration;
    protected PlayerBuffsContainer player;
    protected internal virtual void OnAdded() { }
    protected internal virtual void OnUpdate() { }
    protected virtual void OnExpried() { }
    protected virtual void OnCancel() { }
    protected Coroutine countDown;


    public BuffBase(PlayerBuffsContainer _player)
    {
        player = _player;
    }

    protected virtual void Delete()
    {
       
    }

    protected void StartCountDown()
    {
        if (countDown != null)
            return;
        countDown = player.StartCoroutine(CountDownIE());
    }

    internal IEnumerator CountDownIE()
    {

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        OnExpried();
        Delete();

    }

    protected void Cancel()
    {
        if (countDown != null)
            return;
        player.StopCoroutine(countDown);

        Delete();

    }

}

public class DeBuff:BuffBase
{
    public DeBuff(PlayerBuffsContainer _player) :base(_player)
    {
        player = _player;
    }

    protected override void Delete()
    {
        player.RemoveDeBuff(this);
    }
}


public class Buff : BuffBase
{
    public Buff(PlayerBuffsContainer _player) : base(_player)
    {
        player = _player;
    }

    protected override void Delete()
    {
        player.RemoveBuff(this);
    }

}