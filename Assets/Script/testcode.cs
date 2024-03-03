using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour
{
public List<testtype> testList;

public Dictionary<int,testtype> testD;

private void Start()
{
testList=new List<testtype>();
testD=Dictionary<int,testtype>();

var t=new testclass1;
var t2=new testclass1;

int count=0;
testList.Add(t);
testList.Add(t2);

float startTime= Time.realtimeSinceStartup;

for(int i=0;i<5000;i++)
{
if(testList.Contain(x=>x.index=1);
count++;
}
float end =Time.realtimeSinceStartup;
Debug.Log(end-startTime+"ms");


startTime= Time.realtimeSinceStartup;

for(int i=0;i<5000;i++)
{
if(testD.ContainKey(1);
count++;
}
end =Time.realtimeSinceStartup;
Debug.Log(end-startTime+"ms");

}

public class testtype
{
public int index;

}

public class testclass1: testtype
{
 public testclass1
{
index=1;
}
}

public class testclass2
{
 public testclass1
{
index=2;
}
}
