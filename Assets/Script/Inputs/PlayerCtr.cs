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
    //   [SerializeField]     private float maxSlopeAngle = 60f;
    private Animator ani;

    private Vector2 currentMovement => player.FindAction("Move").ReadValue<Vector2>();
    private Vector2 currentDir => new Vector2(transform.forward.x, transform.forward.z);
    private Vector2 currentVelocity => new Vector2(rb.velocity.x, rb.velocity.z);
    private Vector2 moveDir;
    private Vector2 currentWatchDir => new Vector2(playerModel.forward.x, playerModel.forward.z);
    // bool isOnSlope;

    private bool movePress => currentMovement.magnitude > 0.3f;
    private Vector2 moveDirection;
    private SphereCollider _collider;
    float betweenAngle;
    [SerializeField] private bool canJunp = false;
    [SerializeField] private Transform playerModel;
    [SerializeField]
    private float maxSpeed = 10f;

    [SerializeField]
    private float accelerateForce;
    //  [SerializeField]    private float slopeAccelerateForce;
    [SerializeField] private float decelerateForce;
    // [SerializeField]     private float backDecelerateForce;
    [SerializeField] private float jumpDecelerateForce;
    //[SerializeField]  private float maxSlideForce;
    private float speedAirFactor = 1;

    private Vector3 previusDir;

    [SerializeField]
    private float hitBackMaxForce = 5f;
    [SerializeField]
    private float maxHitBackCap = 0.1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float fallForce = 5f;
    [SerializeField] private float jumpSpeedLimit;
    private Vector2 jumpDir;

    [SerializeField] private bool isGround;

    [SerializeField]
    private float rotationPerSecond = 10f;

    //[SerializeField]private float backRotationPerSecond = 10f;

    private bool isBackwarding;
    private Coroutine backwardingCoro;
    private Coroutine jumpCoro;

    private RaycastHit hit;
    //[SerializeField] private float gravityScaleOnSlope;

    [SerializeField]
    private bool dummy = false;

    private Coroutine accumulatingCoro;
    [SerializeField] private float accumulateForce;
    [SerializeField] private float maxShootDis;
    private float shootDis;
    private bool accumulating = false;
    private RopeCtr ropeCtr;
    internal bool ropeCatching = false;

    private IndicatorCtr indicatorCtr;

    internal void SetInput(PlayerInput input)
    {

        if (dummy)
            return;
        Debug.Log("setInput");
        inputAsset = input.actions;
        player = inputAsset.FindActionMap("Player");


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
        rb = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<SphereCollider>();
        ropeCtr = GetComponentInChildren<RopeCtr>();
        ani = GetComponentInChildren<Animator>();
        indicatorCtr = GetComponentInChildren<IndicatorCtr>();
        if (inputAsset == null && !dummy)
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
        player.FindAction("Fire").performed += DoAccumulate;
        player.FindAction("Fire").canceled += DoShoot;
        //  move = player.FindAction("Move");
    }



    private void OnDisable()
    {
        if (inputAsset == null)
            return;
        player.FindAction("Jump").started -= DoJump;
        player.FindAction("Fire").performed -= DoAccumulate;
        player.FindAction("Fire").canceled -= DoShoot;
        //  player.FindAction("Attack").started -= DoAttack;
        player.Disable();
    }



    private void DoJump(InputAction.CallbackContext obj)
    {
        if (!canJunp)
            return;
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
            speedAirFactor = Mathf.Clamp01(speedAirFactor - jumpSpeedLimit * Time.deltaTime);
            yield return null;
        } while (rb.velocity.y > 0 || rb.velocity.y < 0 && !isGround);
        speedAirFactor = 1;
        jumpCoro = null;
    }

    private bool CheckGround()
    {
        if (dummy)
            return true;
        Vector3 p1 = transform.position + _collider.center;
        if (Physics.SphereCast(p1, _collider.radius - 0.1f, Vector3.down, out hit, 0.3f, 1 << 6, QueryTriggerInteraction.Ignore))
            return true;
        else
            return false;
    }

    private void SpeedControl()
    {
        //if (isOnSlope)
        //{
        //    if (rb.velocity.magnitude > maxSpeed)
        //        rb.velocity = rb.velocity.normalized * maxSpeed;
        //}
    }

    private void FixedUpdate()
    {
        isGround = CheckGround();

        float currentAngle = 0;
        if (dummy)
            return;
        if (movePress)
            //  currentAngle = RotateWithLimit();
            currentAngle = Rotate();

        //   MovementForce(currentAngle);
        Movement();
    }

    private float RotateWithLimit()
    {
        if (dummy)
            return 0;

        if (movePress)
            moveDir = currentMovement.normalized;
        betweenAngle = MathHalper.AngleBetween(moveDir, currentDir);

        moveDirection = Vector3.RotateTowards(moveDirection, moveDir, rotationPerSecond * Mathf.Deg2Rad * Time.fixedDeltaTime, 1);
        transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.y));

        return betweenAngle;

    }
    private float Rotate()
    {
        if (dummy)
            return 0;

        if (movePress)
            moveDir = currentMovement.normalized;

        var watchDir = Vector3.RotateTowards(currentWatchDir, moveDir, rotationPerSecond * Mathf.Deg2Rad * Time.fixedDeltaTime, 1);

        if (accumulating)
            playerModel.LookAt(transform.position + new Vector3(moveDir.x, 0, moveDir.y));
        else
            playerModel.LookAt(transform.position + new Vector3(watchDir.x, 0, watchDir.y));

        moveDirection = moveDir;




        return 0;

    }

    private IEnumerator backwardingIE(float rotateSpeed, Vector2 targetDir)
    {
        isBackwarding = true;

        while (MathHalper.AngleBetween(targetDir, currentDir) > 1f)
        {

            moveDirection = Vector3.RotateTowards(moveDirection, targetDir, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1);

            transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.y));
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        isBackwarding = false;
        backwardingCoro = null;
    }

    private void MovementForce(float rotatAngle)
    {

        float speedFactor = 0;

        //float speedFactor = Mathf.Lerp(0, 0.9f, (rotatAngle - 10) / 120);
        //float scaleMaxspeed = speedFactor * maxSpeed;
        float addedforceMagnitude;
        Vector3 addedforceDir = Vector3.zero;
        Vector3 orgVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //isOnSlope = IsOnSlope(out float slopeAngle);

        if (isGround)
        {

            rb.AddForce(-previusDir * decelerateForce * speedFactor);
            previusDir = Vector3.zero;

            if (movePress && !dummy)
            {
                addedforceMagnitude = accelerateForce;
                addedforceDir = new Vector3(moveDirection.x, 0, moveDirection.y);
                previusDir = addedforceDir;
                float netSpeed;
                #region OnSlope
                //if (isOnSlope)
                //{
                //    var dir = GetSlopeMoveDirection(addedforceDir);
                //    var _force = slopeAccelerateForce;
                //    netSpeed = (rb.velocity + dir * _force).magnitude;
                //    //cap max Speed
                //    if (netSpeed > maxSpeed)
                //    {
                //        Vector3 deltaV = scaleMaxspeed * dir - rb.velocity;
                //        dir = deltaV.normalized;
                //        _force = deltaV.magnitude;
                //    }

                //    var force = dir * _force;
                //    rb.AddForce(force);
                //    rb.AddForce(-GetSlopeSlideDirection() * Mathf.Lerp(0, 1, slopeAngle / 60) * maxSlideForce);

                //    if (rb.velocity.y > 0f)
                //    {
                //        rb.AddForce(Vector3.down * gravityScaleOnSlope);

                //    }


                //}
                //else
                //{
                //    netSpeed = (orgVelocity + addedforceDir * addedforceMagnitude).magnitude;
                //    //cap max Speed
                //    if (netSpeed > maxSpeed)
                //    {
                //        Vector3 deltaV = maxSpeed * addedforceDir - orgVelocity;
                //        addedforceDir = deltaV.normalized;

                //        addedforceMagnitude = deltaV.magnitude;
                //    }
                //    var force = addedforceMagnitude * addedforceDir;
                //    rb.AddForce(force);


                //}
                #endregion 
                netSpeed = (orgVelocity + addedforceDir * addedforceMagnitude).magnitude;
                //cap max Speed
                if (netSpeed > maxSpeed)
                {
                    Vector3 deltaV = maxSpeed * addedforceDir - orgVelocity;
                    addedforceDir = deltaV.normalized;

                    addedforceMagnitude = deltaV.magnitude;
                }
                var force = addedforceMagnitude * addedforceDir;
                rb.AddForce(force);

                ani.SetBool("walking", true);
            }
            else
            {
                float forceMagnitude;
                #region Backwarding
                //if (isBackwarding)
                //{
                //    forceMagnitude = backDecelerateForce;
                //    ani.SetBool("walking", true);
                //    Debug.Log("back");
                //}
                //else
                //{
                //    forceMagnitude = decelerateForce;
                //    ani.SetBool("walking", false);
                //}
                #endregion

                forceMagnitude = decelerateForce;
                ani.SetBool("walking", false);
                Vector3 force;

                //if (isOnSlope)
                //{
                //    force = rb.velocity;
                //}
                //else
                //    force = orgVelocity;

                force = orgVelocity;
                if (force.magnitude > 0.01f)
                {

                    rb.AddForce(-force * forceMagnitude);
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
            if (rb.velocity.y < 0f)
            {
                rb.AddForce(new Vector3(0, -fallForce, 0));
            }
            if (rb.velocity.magnitude > 0.2f)
            {
                addedforce += force;
            }
            rb.AddForce(addedforce);

        }

        // rb.useGravity = !isOnSlope;
    }

    private void Movement()
    {

        //   float scaleMaxspeed = speedFactor * maxSpeed;
        float addedforceMagnitude;
        Vector3 addedforceDir = Vector3.zero;
        Vector3 orgVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //  isOnSlope = IsOnSlope(out float slopeAngle);



        //     rb.AddForce(-previusDir * decelerateForce * speedFactor);
        //       previusDir = Vector3.zero;

        if (movePress && !dummy && !accumulating && !ropeCatching)
        {
            addedforceMagnitude = accelerateForce;
            addedforceDir = new Vector3(moveDirection.x, 0, moveDirection.y);
            previusDir = addedforceDir;
            float netSpeed;

            //if (isOnSlope)
            //{
            //    var dir = GetSlopeMoveDirection(addedforceDir);
            //    var _force = slopeAccelerateForce;
            //    netSpeed = (rb.velocity + dir * _force).magnitude;
            //    //cap max Speed
            //    if (netSpeed > maxSpeed)
            //    {
            //        Vector3 deltaV = maxSpeed * dir - rb.velocity;
            //        dir = deltaV.normalized;
            //        _force = deltaV.magnitude;
            //    }

            //    var force = dir * _force;
            //    rb.AddForce(force);
            //    rb.AddForce(-GetSlopeSlideDirection() * Mathf.Lerp(0, 1, slopeAngle / 60) * maxSlideForce);

            //    if (rb.velocity.y > 0f)
            //    {
            //        rb.AddForce(Vector3.down * gravityScaleOnSlope);

            //    }


            //}
            //else
            //{
            //    netSpeed = (orgVelocity + addedforceDir * addedforceMagnitude).magnitude;
            //    //cap max Speed
            //    if (netSpeed > maxSpeed)
            //    {
            //        Vector3 deltaV = maxSpeed * addedforceDir - orgVelocity;
            //        addedforceDir = deltaV.normalized;

            //        addedforceMagnitude = deltaV.magnitude;
            //    }
            //    var force = addedforceMagnitude * addedforceDir;
            //    rb.AddForce(force);


            //}
            // no slope move
            netSpeed = (orgVelocity + addedforceDir * addedforceMagnitude).magnitude;
            //cap max Speed
            if (netSpeed > maxSpeed)
            {
                Vector3 deltaV = maxSpeed * addedforceDir - orgVelocity;
                addedforceDir = deltaV.normalized;

                addedforceMagnitude = deltaV.magnitude;
            }
            var force = addedforceMagnitude * addedforceDir;
            rb.AddForce(force, ForceMode.VelocityChange);


            ani.SetBool("walking", true);
        }
        else
        {
            float forceMagnitude;
            #region Backwarding
            //if (isBackwarding)
            //{
            //    forceMagnitude = backDecelerateForce;
            //    ani.SetBool("walking", true);
            //    Debug.Log("back");
            //}
            //else
            //{
            //    forceMagnitude = decelerateForce;
            //    ani.SetBool("walking", false);
            //}
            #endregion Backwarding

            forceMagnitude = decelerateForce;
            ani.SetBool("walking", false);
            Vector3 force;

            //if (isOnSlope)
            //{
            //    force = rb.velocity;
            //}
            //else
            //    force = orgVelocity;

            force = orgVelocity;
            if (force.magnitude > 0.01f)
            {

                rb.AddForce(-force * forceMagnitude);
            }
        }

        if (!isGround)
        {
            {



                Vector3 force = -orgVelocity.normalized * jumpDecelerateForce;
                if (rb.velocity.y < 0f)
                {
                    rb.AddForce(new Vector3(0, -fallForce, 0));
                }
                //if (currentVelocity.magnitude > 0.2f)
                //{
                //    rb.AddForce(force);
                //}


            }
        }
        // rb.useGravity = !isOnSlope;

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

    //private bool IsOnSlope(out float slopeAngle)
    //{
    //    slopeAngle = 0;
    //    Vector3 p1 = transform.position + _collider.center + new Vector3(0, 0.5f, 0);
    //    if (Physics.SphereCast(p1, _collider.radius - 0.01f, Vector3.down, out hit, 10f, 1 << 6, QueryTriggerInteraction.Ignore))
    //    {
    //        slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
    //        Debug.DrawLine(hit.point, hit.point + hit.normal * 5, Color.black, 0.2f);
    //        return slopeAngle < maxSlopeAngle && slopeAngle != 0 && isGround;
    //    }
    //    return false;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PlayerBoundary"))
        {
            Rigidbody hittedRb = collision.transform.parent.GetComponent<Rigidbody>();

            Vector3 dir = transform.position - hittedRb.transform.position;
            Vector2 dir2D = new Vector2(dir.x, dir.z).normalized;
            Vector2 rotatedDir2D = new Vector2(dir.z, dir.x).normalized;
            Vector3 netVelocity = rb.velocity - hittedRb.velocity;
            Vector2 netVelocity2D = new Vector2(netVelocity.x, netVelocity.z);

            float hitVelocity = Math.Abs(Vector2.Dot(netVelocity2D, dir2D));
            float velocityFactor = hitVelocity > maxHitBackCap ? 1 : 0;


            Vector2 relativeMoveVelocity = Vector2.Dot(currentVelocity, rotatedDir2D) * rotatedDir2D;
            rb.velocity = new Vector3(relativeMoveVelocity.x, rb.velocity.y, relativeMoveVelocity.y);
            //  rb.velocity = new Vector3(0, rb.velocity.y, 0);
            // float velocityFactor = Mathf.Lerp(0,1, Vector2.Dot(rb.velocity, dir2D))
            rb.AddForce(dir * hitBackMaxForce * velocityFactor, ForceMode.Impulse);
            Debug.Log("jf");
        }
    }

    private void DoShoot(InputAction.CallbackContext obj)
    {
        if (ropeCatching)
        {
            ropeCtr.StopChargePullJew();
            return;
        }
        if (accumulatingCoro == null)
            return;
        StopCoroutine(accumulatingCoro);
        accumulatingCoro = null;
        indicatorCtr.HideRopeRangeIndacator();
        Debug.Log("dis:" + shootDis);

        ShootRope(currentWatchDir);
        accumulating = false;

    }

    private void ShootRope(Vector2 currentWatchDir)
    {
        ropeCatching = true;
        ropeCtr.Shoot(currentWatchDir, shootDis);
    }

    private void DoAccumulate(InputAction.CallbackContext obj)
    {
        if (ropeCatching)
        {
            ropeCtr.ChargePullJew();
            return;
        }

        if (accumulatingCoro != null)
            return;
        accumulatingCoro = StartCoroutine(accumulatingIE());
    }

    private IEnumerator accumulatingIE()
    {
        accumulating = true;

        shootDis = 1;
        indicatorCtr.ShowRopeRangeIndacator(maxShootDis);
        do
        {
            Debug.Log("accumulating");
            shootDis += accelerateForce * Time.deltaTime;
            indicatorCtr.UpdateRopeRangeIndacator(shootDis / maxShootDis, currentWatchDir);
            yield return null;
        }
        while (shootDis < maxShootDis);
        shootDis = maxShootDis;
        while(true)
        {
            indicatorCtr.UpdateRopeRangeIndacator(shootDis / maxShootDis, currentWatchDir);
            yield return null;
        }

    }
}
