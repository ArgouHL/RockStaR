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
    [SerializeField]
    private float maxSlopeAngle = 60f;

    private Vector2 currentMovement => player.FindAction("Move").ReadValue<Vector2>();
    private Vector2 currentDir => new Vector2(transform.forward.x, transform.forward.z);

    [SerializeField] private Vector2 moveDir;
    [SerializeField] bool isOnSlope;

    private bool movePress => currentMovement.magnitude > 0.3f;
    private Vector2 moveDirection;
    private SphereCollider _collider;
    [SerializeField] float v;

    [SerializeField]
    private float maxSpeed = 10f;

    [SerializeField]
    private float accelerateForce;
    [SerializeField]
    private float slopeAccelerateForce;
    [SerializeField]
    private float decelerateForce;
    [SerializeField]
    private float backDecelerateForce;
    [SerializeField]
    private float jumpDecelerateForce;
    [SerializeField]
    private float maxSlideForce;
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

    private RaycastHit hit;
    [SerializeField] private float gravityScaleOnSlope;

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
        if (Physics.SphereCast(p1, _collider.radius - 0.1f, Vector3.down, out hit, 0.3f, 1 << 6, QueryTriggerInteraction.Ignore))
            return true;
        else
            return false;



    }

    private void Update()
    {

    }

    private void SpeedControl()
    {
        if (isOnSlope)
        {
            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;
        }
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
        float addedforceMagnitude;
        Vector3 addedforceDir;
        Vector3 orgVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        isOnSlope = IsOnSlope(out float slopeAngle);
        if (isGround)
        {
            if (movePress && !isBackwarding)
            {
                addedforceMagnitude = accelerateForce;
                addedforceDir = new Vector3(moveDirection.x, 0, moveDirection.y);
                float netSpeed;


                if (isOnSlope)
                {

                    var dir = GetSlopeMoveDirection(addedforceDir);
                    var force = slopeAccelerateForce;
                    netSpeed = (rb.velocity + dir * force).magnitude;
                    if (netSpeed > scaleMaxspeed)
                    {
                        Vector3 deltaV = scaleMaxspeed * dir - rb.velocity;
                        dir = deltaV.normalized;

                        force = deltaV.magnitude;

                    }




                    rb.AddForce(dir * force );
                    rb.AddForce(-GetSlopeSlideDirection() * Mathf.Lerp(0, 1, slopeAngle / 60) * maxSlideForce);

                    if (rb.velocity.y > 0f)
                    {
                        rb.AddForce(Vector3.down * gravityScaleOnSlope);

                    }

                }
                else
                {
                    netSpeed = (orgVelocity + addedforceDir * addedforceMagnitude).magnitude;
                    if (netSpeed > scaleMaxspeed)
                    {
                        Vector3 deltaV = scaleMaxspeed * addedforceDir - orgVelocity;
                        addedforceDir = deltaV.normalized;

                        addedforceMagnitude = deltaV.magnitude;
                    }
                    rb.AddForce(addedforceMagnitude * addedforceDir);

                    Debug.Log("A");
                }


            }
            else
            {
                float forceMagnitude;
                if (movePress && isBackwarding)
                    forceMagnitude = backDecelerateForce;
                else
                    forceMagnitude = decelerateForce;
                Vector3 force;

                if (isOnSlope)
                {
                    force = rb.velocity;
                }
                else
                    force = orgVelocity;


                if (force.magnitude > 0.01f)
                {
                    rb.AddForce(-force * forceMagnitude);
             
                }
               
            }

        }
        else
        {
            Debug.Log("D3");
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


        rb.useGravity = !isOnSlope;

    }

    private Vector3 GetSlopeMoveDirection(Vector3 addedforceDir)
    {
        var v3 = Vector3.ProjectOnPlane(addedforceDir, hit.normal).normalized;
        return v3;
    }


    private Vector3 GetSlopeSlideDirection()
    {
        var v3 = Vector3.ProjectOnPlane(Vector3.one, hit.normal).normalized;
        return v3;
    }

    private bool IsOnSlope(out float slopeAngle)
    {
        slopeAngle = 0;
        Vector3 p1 = transform.position + _collider.center + new Vector3(0, 0.5f, 0);
        if (Physics.SphereCast(p1, _collider.radius - 0.01f, Vector3.down, out hit, 0.6f, 1 << 6, QueryTriggerInteraction.Ignore))
        {

            slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            Debug.DrawLine(hit.point, hit.point + hit.normal * 5, Color.black, 0.2f);

            return slopeAngle < maxSlopeAngle && slopeAngle != 0 && isGround;
        }

        return false;
    }

}
