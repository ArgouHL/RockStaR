using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JewelryCtr : MonoBehaviour
{
    private Rigidbody rig;
    [SerializeField] private float bePushedSpeed = 3;
    [SerializeField] private float decelerateForce = 1;
    private Coroutine moveCoro;
    private Coroutine SelfMoveRepeadCoro;
    private Vector3 moveDir;
    private int energy = 0;
    private int state = 0;
    private bool isMoving = false;
    private bool isSelfMoving = false;
    private float coolDown = 0;
    [Space]
    [Header("State1")]
    [SerializeField] private float state1CoolDowns=2;
    [SerializeField] private float state1MoveDis = 6;
    [SerializeField] private float state1MoveSpeed = 60;



    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        Inst();
    }

    private void Inst()
    {
        energy = 0;
        state = 0;
    }

    internal void BePush(Vector3 normalizedVector)
    {
        energy++;
        switch (state)
        {
            case 0:
                if (energy > 2)
                {
                    coolDown = state1CoolDowns;
                    if (SelfMoveRepeadCoro == null)
                        SelfMoveRepeadCoro = StartCoroutine(SelfMoveRepeadIE());
                }
                Move(normalizedVector);
                break;
            case 1:
                break;
        }

    }


    private IEnumerator MoveIE()
    {
        isMoving = true;
        float currentSpeed = bePushedSpeed;
        while (currentSpeed > bePushedSpeed * 0.5f)
        {
            rig.MovePosition(transform.position + moveDir * currentSpeed * Time.fixedDeltaTime);
            currentSpeed -= decelerateForce * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();

        }
        isMoving = false;
        moveCoro = null;
    }

    private void Move(Vector3 direction)
    {
        if (isSelfMoving)
            return;

        moveDir = direction;
        if (moveCoro != null)
            StopCoroutine(moveCoro);
        moveCoro = StartCoroutine(MoveIE());
    }

    private IEnumerator SelfMoveRepeadIE()
    {
        while (true)
        {
            yield return new WaitWhile(() =>isMoving);
            yield return SelfMove();
            yield return new WaitForSeconds(coolDown);
        }


    }

    private IEnumerator SelfMove()
    {
        yield return StateOneMove();

    }

    private IEnumerator StateOneMove()
    {
        isSelfMoving = true;
        Debug.Log("StateOneMove");
        var direction = Random.insideUnitCircle.normalized;
        moveDir = new Vector3(direction.x, 0, direction.y);
        float dis = 0;
     
        while (dis < state1MoveDis)
        {
            rig.MovePosition(transform.position + moveDir * state1MoveSpeed * Time.fixedDeltaTime);
            dis += state1MoveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isSelfMoving = false;
    }







    #region bounce
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDir = Vector3.Reflect(moveDir, collision.contacts[0].normal);
        }
    }

    #endregion
}
