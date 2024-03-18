using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFoot : MonoBehaviour
{
    Animator ani;
    [SerializeField] float heightToGroundAdj;


    private void Awake()
    {
        ani = GetComponent<Animator>();    
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ani)
        {
            //ani.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.5f);
           // ani.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.5f);

            RaycastHit leftFootHit;
            if (Physics.Raycast(ani.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down, out leftFootHit, heightToGroundAdj+1, 1 << 6))
            {
                Vector3 pos = leftFootHit.point;
                pos.y += heightToGroundAdj;
                ani.SetIKPosition(AvatarIKGoal.LeftFoot, pos);
                //var rot= new Quaternion(Quaternion.FromToRotation(Vector3.up, leftFootHit.normal).x, Quaternion.LookRotation(transform.forward, leftFootHit.normal).y, Quaternion.FromToRotation(Vector3.up, leftFootHit.normal).z, Quaternion.LookRotation(transform.forward, leftFootHit.normal).w);

                //ani.SetIKRotation(AvatarIKGoal.LeftFoot, rot);
                Debug.DrawLine(ani.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, leftFootHit.point);
            }

            RaycastHit rightFootHit;
            if (Physics.Raycast(ani.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down, out rightFootHit, heightToGroundAdj + 1, 1 << 6))
            {
                Vector3 pos = rightFootHit.point;
                pos.y += heightToGroundAdj;
                ani.SetIKPosition(AvatarIKGoal.RightFoot, pos);
                //ani.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(ani.GetIKRotation(AvatarIKGoal.LeftFoot)*Vector3.forward, rightFootHit.normal));
                Debug.DrawLine(ani.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, leftFootHit.point);
            }

        }
    }


}
