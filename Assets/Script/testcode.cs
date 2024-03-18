using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour
{
    public List<testtype> testList;

    public Dictionary<int, testtype> testD;

    private void Start()
    {
        testList = new List<testtype>();
        testD = new Dictionary<int, testtype>();

       

        int count = 0;
        //testList.Add(t);
        //testList.Add(t2);
        //testList.Add(t3);
        //testList.Add(t4);

        //testD.Add(t.index, t);
        //testD.Add(t2.index, t2);
        //testD.Add(t3.index, t3);
        //testD.Add(t4.index, t4);


       


    }



    private void FixedUpdate()
    {
        var t = new testclass1();
        var t2 = new testclass2();
        var t3 = new testclass3();
        var t4 = new testclass4();
        float startTime = Time.realtimeSinceStartup;

        
        for (int i = 0; i < 1000; i++)
        {
            testList.Add(t);
            
        }

        float end = Time.realtimeSinceStartup;
        Debug.Log((end - startTime) * 1000 + "ms");


        startTime = Time.realtimeSinceStartup;

        for (int i = 0; i < 1000; i++)
        {
            testD.Add(t.index, t);
            
        }
        end = Time.realtimeSinceStartup;
        Debug.Log((end - startTime) * 1000 + "ms");
    }
}
public class testtype
{
    public int index;

}

public class testclass1 : testtype
{
    public testclass1()
    {
        index = 1;
    }
}

public class testclass2 : testtype
{
    public testclass2()
    {
        index = 2;
    }
}

public class testclass3 : testtype
{
    public testclass3()
    {
        index = 3;
    }
}

public class testclass4 : testtype
{
    public testclass4()
    {
        index = 4;
    }
}
