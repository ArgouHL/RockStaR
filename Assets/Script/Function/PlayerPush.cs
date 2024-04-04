using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float bePushForce;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Jewelry"))
        {
            Rigidbody hitRig = collision.transform.GetComponent<Rigidbody>();
            Vector3 forceDir = transform.position - collision.transform.position;
            forceDir.y = 0;
            forceDir = forceDir.normalized;

            GetComponent<Rigidbody>().AddForce(forceDir * bePushForce* hitRig.velocity.magnitude, ForceMode.Impulse);



        }


    }


}
