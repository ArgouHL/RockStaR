using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObj :MonoBehaviour
{
    public PullableObjType type;
    private Rigidbody rig;


    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    public void Pushed(Vector3 force)
    {
        switch(type)
        {
            case PullableObjType.Player:
              GetComponent<PlayerCtr>().BePushed(force);
                break;
            case PullableObjType.Jewelry:
                GetComponent<JewelryCtr>().BePush(force.normalized);

                break;
        }
    }

    public void HardPushed(Vector3 force)
    {
        switch (type)
        {
            case PullableObjType.Player:
                GetComponent<PlayerCtr>().VelocityChange(force);
                break;
            case PullableObjType.Jewelry:
              //  GetComponent<JewelryCtr>().BePush(force.normalized);

                break;
        }
    }

}

public enum PullableObjType
{
    Player,Jewelry
}
