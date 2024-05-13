using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JewelryCtr : MonoBehaviour
{
    private Rigidbody rig;
    [SerializeField] private float bePushedAddSpeed = 10;
    [SerializeField] private float bePushedMaxSpeed = 50;
    [SerializeField] private float bePushedMinSpeed = 10;
    private float nowSpeed;
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
    private int state1Charge = 5;
    private float state1CoolDowns = 2;
    private float state1MoveDis = 6;
    private float state1MoveSpeed = 60;


    [SerializeField] private GameObject visual;


    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        Inst();
    }


    internal void Reset(Vector3 resetPos)
    {

        visual.SetActive(false);
        transform.position = resetPos + new Vector3(0, GetComponent<SphereCollider>().radius, 0);
        Inst();


        visual.SetActive(true);
        float angle = Random.Range(60f, 120f) * Mathf.Deg2Rad;
        BePush(new Vector3(Mathf.Cos(angle), 0, -Mathf.Sin(angle)).normalized);

    }


    internal void Inst()
    {
        energy = 0;
        state = 0;
        SetTeam(Team.None);
        if (JewelrySystem.instance.modeA)
            nowSpeed = bePushedMinSpeed;
        else
            nowSpeed = bePushedMaxSpeed;
    }




    internal void SetTeam(Team team)
    {

        switch (team)
        {
            case Team.Blue:
                visual.GetComponent<MeshRenderer>().material = JewelrySystem.instance.blueJew;
                break;
            case Team.Yellow:
                visual.GetComponent<MeshRenderer>().material = JewelrySystem.instance.yellowJew;
                break;
            case Team.None:
                visual.GetComponent<MeshRenderer>().material = JewelrySystem.instance.grayJew;
                break;
            default:
                break;
        }
    }

    internal void BePush(Vector3 normalizedVector)
    {
        // energy++;
        switch (state)
        {
            case 0:
                if (energy > state1Charge)
                {
                    coolDown = state1CoolDowns;
                    if (SelfMoveRepeadCoro == null)
                        SelfMoveRepeadCoro = StartCoroutine(SelfMoveRepeadIE());
                }
                if (JewelrySystem.instance.modeA)
                    nowSpeed = nowSpeed >= bePushedMaxSpeed ? bePushedMaxSpeed : nowSpeed + bePushedAddSpeed;


                Move(normalizedVector);
                break;
            case 1:
                break;
        }
    }

    private IEnumerator MoveIE()
    {
        isMoving = true;
        float currentSpeed = nowSpeed;
        float circumference = 2 * Mathf.PI * 0.25f;
        while (currentSpeed > nowSpeed * 0.01f)
        {
            float moveVelocity = currentSpeed;
            rig.velocity = moveDir * moveVelocity;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDir).normalized;
            //    Debug.DrawLine(transform.position, transform.position + rotationAxis * 10);
            float angle = Mathf.Rad2Deg * (moveVelocity * Time.fixedDeltaTime / circumference);

            // float angle = Mathf.Rad2Deg * Mathf.Atan2(1, currentSpeed * Time.fixedDeltaTime);

            // float _angle = Mathf.Rad2Deg * (moveDistance / circumference);
            float deceleratedAngle = angle * Mathf.Clamp01(currentSpeed / nowSpeed);


            //  Quaternion rotation = Quaternion.AngleAxis(deceleratedAngle, rotationAxis);
            // Matrix4x4 originalMatrix = Matrix4x4.Rotate(transform.rotation);
            //Matrix4x4 deceleratedMatrix = Matrix4x4.Rotate(rotation);

            // Matrix4x4 newMatrix = deceleratedMatrix * originalMatrix;


            //Quaternion newRotation = Quaternion.LookRotation(newMatrix.GetColumn(2), newMatrix.GetColumn(1));



            visual.transform.Rotate(rotationAxis, deceleratedAngle, Space.World);
            //rig.MoveRotation(newRotation);

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

    internal void Stop()
    {
        if (moveCoro != null)
            StopCoroutine(moveCoro);
        rig.velocity = Vector3.zero;


    }

    private IEnumerator SelfMoveRepeadIE()
    {
        while (true)
        {
            yield return new WaitWhile(() => isMoving);
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

            // rig.MovePosition
            dis += state1MoveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isSelfMoving = false;
    }







    #region bounce
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("PlayerBoundary"))
        {
            moveDir = Vector3.Reflect(moveDir, collision.contacts[0].normal);
            //nowSpeed = nowSpeed<=bePushedMinSpeed? bePushedMinSpeed: nowSpeed - bePushedAddSpeed;

        }
    }

    #endregion
}
