using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffsContainer : MonoBehaviour
{

  private int maxBuffNum=15;
private List<Buff> buffs ;
private delegate void BuffUpdate();
private BuffUpdate updateEvent;


private void Start()
{
Init();
}

private void Init()
{
buffs= new List<Buff>(maxBuffNum);
updateEvent=null;
}

internal void AddBuff(Buff buff)
{
if(!buffs.contain(buff))
return;
buffs.Add(buff);
updateEvent+=buff.OnUpdate;
//buff.OnAdded();
}

internal void EarlyEnd(Buff buff)
{
Remove(buff);
}


internal void Remove(Buff buff)
{
if(!buffs.contain(buff))
return;
buffs.Remove(buff);
updateEvent-=buff.UpdateEvent:
}

private void Update()
{
updateEvent.Invoke();
}

}





public class Buff:interface
{
protected string buffName;
internal float duration;
protected PlayerBuffsContainer player;
protected abstract void OnAdded();
protected abstract void OnUpdate();
protected abstract void OnExpried();
protected abstract void OnCancel();
protected Corotine countDown;


public Buff(PlayerBuffsContainer _player)
{
player=_player;
}

protected void BuffDelete()
{
player.Remove(this);
}

protected void StartCountDown()
{
if(countDown!=null)
return;
countDown=StartCorotine(CountDownIE(duration));
}

protected IEnumerator CountDownIE(float duration)
{
float time=0;
while(time < duration)
{

time+=Time.deltaTime;
yield return null;
}

OnExpried();
BuffDelete();

}

protected void Cancel()
{
if(countDown!=null)
return;
StopCorotine(countDown);

BuffDelete();

}

}
