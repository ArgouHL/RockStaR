using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffsystem : MonoBehaviour
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
updateEvent+=buff.UpdateEvent:
//buff.Addbuff();
}

internal void Remove(Buff buff)
{
if(!buffs.contain(buff))
return;
buffs.Remove(buff);
updateEvent-=buff.UpdateEvent:
//buff.BuffEnd;
}

private void Update()
{
updateEvent.Invoke();
}


}





public class Buff:interface
{
public string buffname;

protected void AddBuff();
protected void BuffUpdate();
protected void BuffEnd();

}


