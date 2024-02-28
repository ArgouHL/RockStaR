using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCtr : MonoBehaviour
{
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;

    private Rigidbody rb;



    private Vector2 currentMovement => player.FindAction("Move").ReadValue<Vector2>();
    private Vector2 currentDir => new Vector2(transform.forward.x, transform.forward.z);

    [SerializeField] private Vector2 moveDir;

    private bool movePress => currentMovement.magnitude > 0.3f;
    private Vector2 moveDirection;
    private SphereCollider _collider;
    [SerializeField] float v;

    [SerializeField]
    private float maxSpeed = 10f;

    [SerializeField]
    private float accelerateForce = 10f;
    [SerializeField]
    private float decelerateForce = 10f;
    [SerializeField]
    private float backDecelerateForce = 10f;
    [SerializeField]
    private float jumpDecelerateForce = 10f;
    private float currentSpeed = 0;
    private float speedAirFactor = 1;

    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField] private float speedLimit;
    private Vector2 jumpDir;

    [SerializeField] private bool isGround;

    [SerializeField]
    private float rotationPerSecond = 10f;

    [SerializeField]
    private float backRotationPerSecond = 10f;

    private bool isBackwarding;
    private Coroutine backwardingCoro;
    private Coroutine jumpCoro;


    internal void SetInput(PlayerInput input)
    {
        Debug.Log("setInput");
        inputAsset = input.actions;
        player = inputAsset.FindActionMap("Player");


        rb = GetComponent<Rigidbody>();
        //  animator = this.GetComponent<Animator>();

        //playerActionsAsset = new ThirdPersonActionsAsset();
        // move = player.FindAction("Move");

        gameObject.SetActive(true);
        if (inputAsset != null)
            return;
        EnablePlayer();

    }


    private void Awake()
    {
        _collider = GetComponentInChildren<SphereCollider>();
        if (inputAsset == null)
            gameObject.SetActive(false);
    }



    private void OnEnable()
    {
        if (inputAsset == null)
            return;

        //     player.FindAction("Attack").started += DoAttack;

        EnablePlayer();
    }

    private void EnablePlayer()
    {
        player.Enable();
        player.FindAction("Jump").started += DoJump;
        //  move = player.FindAction("Move");




    }

    private void OnDisable()
    {
        if (inputAsset == null)
            return;
        player.FindAction("Jump").started -= DoJump;
        //  player.FindAction("Attack").started -= DoAttack;
        player.Disable();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        Debug.Log("jump");
        if (!CheckGround())
            return;

        if (jumpCoro != null)
            return;
        jumpCoro = StartCoroutine(JumpIE());

    }
    private IEnumerator JumpIE()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpDir = currentMovement;
        do
        {
            Debug.Log("air");
            speedAirFactor = Mathf.Clamp01(speedAirFactor - speedLimit * Time.deltaTime);
            yield return null;
        } while (rb.velocity.y > 0 || rb.velocity.y < 0 && !isGround);
        speedAirFactor = 1;
        jumpCoro = null;




    }

    private bool CheckGround()
    {
        Vector3 p1 = transform.position + _collider.center;
        var cols = Physics.OverlapSphere(p1, _collider.radius, 1 << 6);

        if (cols.Length != 0)
            return true;
        else
            return false;
    }

    private void FixedUpdate()
    {
        isGround = CheckGround();
        float currentAngle = 0;
        if (movePress)
            currentAngle = Rotate();
        //Movement(currentAngle);
        MovementForce(currentAngle);
    }




    private float Rotate()
    {
        if (movePress)
            moveDir = currentMovement.normalized;
        v = MathHalper.AngleBetween(moveDir, currentDir);
        if (v > 60 && isGround)
        {
            if (backwardingCoro != null)
                return v;
            backwardingCoro = StartCoroutine(backwardingIE(backRotationPerSecond, moveDir));
        }
        else if (v < 5 && movePress)
        {
            if (backwardingCoro != null)
            {
                StopCoroutine(backwardingCoro);
                backwardingCoro = null;
                isBackwarding = false;
            }

        }

        if (isBackwarding)
            return v;
        moveDirection = Vector3.RotateTowards(moveDirection, moveDir, rotationPerSecond * Mathf.Deg2Rad * Time.fixedDeltaTime, 1);
        transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.y));
        return v;

    }

    private IEnumerator backwardingIE(float rotateSpeed, Vector2 targetDir)
    {
        isBackwarding = true;
        Debug.Log("back");
        while (MathHalper.AngleBetween(targetDir, currentDir) > 1f)
        {

            moveDirection = Vector3.RotateTowards(moveDirection, targetDir, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1);

            transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.y));
            yield return null;
        }
        isBackwarding = false;
        backwardingCoro = null;
    }

    //private void Movement(float rotatAngle)
    //{
    //    float speedFactor = Mathf.Lerp(1, 0, (rotatAngle + 10) / 270);


    //    if (movePress && !isBackwarding)
    //    {

    //        currentSpeed = currentSpeed < maxSpeed ? currentSpeed + accelerateForce * Time.deltaTime : maxSpeed;

    //    }
    //    else if (movePress && isBackwarding)
    //    {
    //        if (currentSpeed > minSpeed)
    //            currentSpeed -= decelerateForce * Time.deltaTime;
    //    }
    //    else
    //    {
    //        currentSpeed = currentSpeed > 0.01f ? currentSpeed - decelerateForce * Time.deltaTime : 0;
    //    }

    //    if (currentSpeed > 0)
    //    {
    //        rb.velocity = new Vector3(moveDirection.x, 0, moveDirection.y) * currentSpeed * speedAirFactor * speedFactor + new Vector3(0, rb.velocity.y, 0);
    //    }
    //    else
    //    {
    //        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    //    }

    //}

    private void MovementForce(float rotatAngle)
    {
        float speedFactor = Mathf.Lerp(1, 0, (rotatAngle - 20) / 160);
        float scaleMaxspeed = speedFactor * maxSpeed;
        float addedforceMagnitude = 0;
        Vector3 addedforceDir = Vector3.zero;
        Vector3 orgVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);



        if (isGround)
        {

            if (movePress && !isBackwarding)
            {

                addedforceMagnitude = accelerateForce;
                addedforceDir = new Vector3(moveDirection.x, 0, moveDirection.y);
                float netSpeed = (orgVelocity + addedforceDir * addedforceMagnitude).magnitude;
                if (netSpeed > scaleMaxspeed)
                {
                    Vector3 deltaV = scaleMaxspeed * addedforceDir - orgVelocity;
                    addedforceDir = deltaV.normalized;

                    addedforceMagnitude = deltaV.magnitude;

                }
                rb.AddForce(addedforceMagnitude * addedforceDir);
            }
            else
            {
                float forceMagnitude = 0;
                if (movePress && isBackwarding)
                    forceMagnitude = backDecelerateForce;
                else
                    forceMagnitude = decelerateForce;
                Vector3 force = -orgVelocity.normalized * forceMagnitude;
                if (rb.velocity.magnitude > 0.3f)
                {
                    rb.AddForce(force);
                }
            }

        }
        else
        {


            addedforceMagnitude = accelerateForce;
            addedforceDir = new Vector3(jumpDir.x, 0, jumpDir.y);
            float netSpeed = (orgVelocity + addedforceDir * addedforceMagnitude).magnitude;
            if (netSpeed > maxSpeed)
            {
                Vector3 deltaV = maxSpeed * addedforceDir - orgVelocity;
                addedforceDir = deltaV.normalized;

                addedforceMagnitude = deltaV.magnitude;

            }
            Vector3 addedforce = addedforceMagnitude * addedforceDir;




            Vector3 force = -orgVelocity.normalized * jumpDecelerateForce;
            if (rb.velocity.magnitude > 0.2f)
            {
                addedforce += force;
            }
            rb.AddForce(addedforce);


        }














    }

}
