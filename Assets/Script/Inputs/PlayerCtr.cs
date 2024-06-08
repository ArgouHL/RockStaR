using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[DefaultExecutionOrder(-1)]
public class PlayerCtr : MonoBehaviour
{

    internal PlayerConfig playerConfig;
    private CharaterMovement charaterMovement;


    private Rigidbody rb;
    public int playerID;




    //   [SerializeField]     private float maxSlopeAngle = 60f;
    private Animator ani;

    private Vector2 currentMovement => playerConfig.gameInput.FindAction("Move").ReadValue<Vector2>();
    private Vector2 currentDir => new Vector2(transform.forward.x, transform.forward.z);
    private Vector2 currentVelocity => new Vector2(rb.velocity.x, rb.velocity.z);



    private Vector2 moveDir;
    private Vector2 rotateDir;
    private Vector2 currentWatchDir => new Vector2(playerModel.forward.x, playerModel.forward.z);
    // bool isOnSlope;

    private bool movePress => currentMovement.magnitude > 0.3f;
    private bool movePressForRotate => currentMovement.magnitude > 0.5f;
    private Vector2 moveDirection;
    private SphereCollider _collider;
    float betweenAngle;
    [SerializeField] private bool canJunp = false;
    public Transform playerModel;
    [SerializeField]
    private float noramlMMaxSpeed = 10f;
    private float maxSpeed => noramlMMaxSpeed * slowFactor;
    [SerializeField]
    private float accelerateForce;
    //  [SerializeField]    private float slopeAccelerateForce;
    [SerializeField] private float decelerateForce;

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




    private Coroutine jumpCoro;

    private RaycastHit hit;


    [SerializeField]
    private bool dummy = false;


    public delegate void CharaAction();
    public CharaAction OnUseItem;

    private PowerGun powerGun;

    private Coroutine stunCoro;
    private float stunTime = 0;

    [SerializeField] internal Team choosedTeam = Team.None;
    [SerializeField] internal TeamEffect teamShowEffect;

    internal PlayerSfxControl playerSfxControl;


    internal void SetInput(PlayerConfig config)
    {

        if (dummy)

            return;

        Debug.Log("setInput");
        playerConfig = config;


        gameObject.SetActive(true);
        CharaSwitcher.instance.AddPlayerSkinManagment(GetComponent<PlayerSkinManagment>());
        EnablePlayer();
        playerID = playerConfig.PlayerIndex;
        choosedTeam = playerConfig.PlayerTeam;
        GetComponentInChildren<TeamPlateShow>().ChangeTeam(choosedTeam);

        Debug.Log("SetInput");
        if (choosedTeam == Team.None)
            return;
        teamShowEffect.StartEffect(choosedTeam);

    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<SphereCollider>();
        powerGun = GetComponentInChildren<PowerGun>();
        playerSfxControl = GetComponentInChildren<PlayerSfxControl>();
        ReGetAnimator();
        charaterMovement = GetComponent<CharaterMovement>();
        if (playerConfig == null && !dummy)
            gameObject.SetActive(false);
        if (dummy)
            playerID = 999;
    }

    internal void ReGetAnimator()
    {
        ani = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        if (playerConfig == null)
            return;

        CanMove(true);
    }

    private void EnablePlayer()
    {
        //playerConfig.ChangeInputMap(InputType.player);
        playerConfig.gameInput.FindAction("Jump").started += DoJump;
        playerConfig.gameInput.FindAction("Fire").performed += DoShoot;

        playerConfig.gameInput.FindAction("UseItem").performed += UseItem;
        //  move = player.FindAction("Move");
    }



    private void OnDisable()
    {
        if (playerConfig == null)
            return;
        playerConfig.gameInput.FindAction("Jump").started -= DoJump;
        playerConfig.gameInput.FindAction("Fire").performed -= DoShoot;

        playerConfig.gameInput.FindAction("UseItem").performed -= UseItem;
        //  player.FindAction("Attack").started -= DoAttack;
        playerConfig.gameInput.Disable();
        CanMove(false);
    }



    private void DoJump(InputAction.CallbackContext obj)
    {

        if (!canJunp)
            return;

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
    private delegate void MovementEvent();
    private MovementEvent OnMove;
    private MovementEvent OnRotate;
    internal void CanMove(bool v)
    {
        if (v)
        {

            OnMove += Move;
            OnRotate += CharatarRotate;
        }
        else
        {
            playerModel.rotation = Quaternion.Euler(0, 180, 0);
            OnMove -= Move;
            OnRotate -= CharatarRotate;
            StopMove();
        }
    }

    private void FixedUpdate()
    {
        OnMove?.Invoke();
    }

    private void Update()
    {
        OnRotate?.Invoke();
    }

    private void Move()
    {
        isGround = CheckGround();

        //float currentAngle = 0;
        if (dummy || stunned)
            return;


        if (movePress)
        {
            //currentAngle = Rotate();
            Rotate();

            rotateDir = currentMovement.normalized;
        }
        if (jewHitback)
            Movement(false);
        else
            Movement(true);

    }
    private void CharatarRotate()
    {

        if (stunned)
            return;

        playerModel.LookAt(transform.position + new Vector3(rotateDir.x, 0, rotateDir.y));
    }

    private float Rotate()
    {
        if (stunned)
            return 0;
        if (dummy)
            return 0;

        if (movePress)
            moveDir = currentMovement.normalized;

        if (movePressForRotate)
            rotateDir = currentMovement.normalized;

        moveDirection = moveDir;




        return 0;

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

    private void Movement(bool canMove)
    {

        #region SlowDown
        float addedforceMagnitude;
        Vector3 addedforceDir = Vector3.zero;
        Vector3 orgVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        #endregion

        if (isGround)
            if (movePress && canMove)
            {
                addedforceMagnitude = accelerateForce;
                addedforceDir = new Vector3(moveDirection.x, 0, moveDirection.y);
                previusDir = addedforceDir;
                float netSpeed;


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


                ani.SetBool("running", true);
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
                ani.SetBool("running", false);
                Vector3 force;

                force = orgVelocity;
                if (force.magnitude > 0.01f)
                {

                    rb.AddForce(-force * forceMagnitude * 0.2f);
                }
            }

        else
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
        //if (collision.transform.CompareTag("PlayerBoundary"))
        //{
        //    Rigidbody hittedRb = collision.transform.parent.GetComponent<Rigidbody>();

        //    Vector3 dir = transform.position - hittedRb.transform.position;
        //    Vector2 dir2D = new Vector2(dir.x, dir.z).normalized;
        //    Vector2 rotatedDir2D = new Vector2(dir.z, dir.x).normalized;
        //    Vector3 netVelocity = rb.velocity - hittedRb.velocity;
        //    Vector2 netVelocity2D = new Vector2(netVelocity.x, netVelocity.z);

        //    float hitVelocity = Math.Abs(Vector2.Dot(netVelocity2D, dir2D));
        //    float velocityFactor = hitVelocity > maxHitBackCap ? 1 : 0;


        //    Vector2 relativeMoveVelocity = Vector2.Dot(currentVelocity, rotatedDir2D) * rotatedDir2D;
        //    rb.velocity = new Vector3(relativeMoveVelocity.x, rb.velocity.y, relativeMoveVelocity.y);
        //    //  rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //    // float velocityFactor = Mathf.Lerp(0,1, Vector2.Dot(rb.velocity, dir2D))
        //    rb.AddForce(dir * hitBackMaxForce * velocityFactor, ForceMode.Impulse);
        //    Debug.Log("jf");
        //}
    }

    private bool stunned = false;
    internal bool isStunned { get { return stunned; } }
    [SerializeField]
    [Range(0, 1)]
    private float pushedRecoverForce = 0;
    internal float slowFactor = 1;

    private bool jewHitback = false;

    #region ShootPower
    private bool shootCoolDowning = false;
    [SerializeField] private float shootCoolDownTime = 0.5f;
    [SerializeField] private bool shootReocvery = false;
    [SerializeField] private float shootReocveryTime = 0.5f;

    private Coroutine ShootReocveryCoro;
    private void DoShoot(InputAction.CallbackContext obj)
    {
        if (choosedTeam == Team.None)
            return;
        ani.ResetTrigger("Shoot");
        if (stunned)
            return;
        if (shootCoolDowning)
            return;
        powerGun.Shoot(playerModel.forward);
        StartCoroutine(ShootCoolDownIE(shootCoolDownTime));
        ani.SetTrigger("Shoot");
        if (ShootReocveryCoro != null)
            StopCoroutine(ShootReocveryCoro);
        ShootReocveryCoro = StartCoroutine(ShootReocveryIE(shootReocveryTime));
    }

    private IEnumerator ShootCoolDownIE(float shootCoolDownTime)
    {
        shootCoolDowning = true;
        yield return new WaitForSeconds(shootCoolDownTime);
        shootCoolDowning = false;

    }
    private IEnumerator ShootReocveryIE(float shootReocveryTime)
    {
        shootReocvery = true;
        ani.SetBool("running", false);
        yield return new WaitForSeconds(shootReocveryTime);
        shootReocvery = false;
        ShootReocveryCoro = null;
    }


    #endregion



    private void UseItem(InputAction.CallbackContext obj)
    {
        Debug.Log("UseItem");
        OnUseItem?.Invoke();
    }


    internal void BePowerPushed(Vector3 force, Team team)
    {
        if (team == choosedTeam || stunned)
            return;
        Debug.Log("bePush");
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);

        if (stunned)
            return;
        playerModel.LookAt(transform.position - force);
        playerSfxControl.PlayGetEnergyHitBackSfx();
        StartCoroutine(StunIE(1.5f));

    }

    internal void VelocityChange(Vector3 force)
    {
        if (GetComponent<PlayerBuffsContainer>().HaveBuff(BuffType.inf))
            return;
        rb.velocity = force;

        if (stunCoro != null)
        {
            if (stunTime < 0.6f)
                stunTime = 0.6f;
            return;
        }
        stunCoro = StartCoroutine(StunIE(3));
    }

    internal void BeJewPushed(Vector3 force)
    {
        Debug.Log("bePush");
        rb.velocity = force;
        StartCoroutine(StunIE(force));
    }

    private IEnumerator StunIE(Vector3 force)
    {
        jewHitback = true;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitWhile(() => rb.velocity.sqrMagnitude > 0.01f);

        Debug.Log("End");
        jewHitback = false;
        stunCoro = null;

    }

    private IEnumerator StunIE(float time)
    {
        float _time = time;
        stunned = true;
        ani.ResetTrigger("Up");
        ani.SetBool("falldown", true);
        while (_time > 0.45f)
        {
            _time -= Time.deltaTime;
            yield return null;
        }

        ani.SetTrigger("Up");

        while (_time > 0)
        {
            _time -= Time.deltaTime;
            yield return null;
        }
        stunned = false;
        stunCoro = null;
        ani.SetBool("falldown", false);
    }

    internal void Stun(Vector3 hitPosition, float v)
    {
        playerModel.LookAt(hitPosition);
        if (stunCoro != null)
        {
            if (stunTime < v)
                stunTime = v;
            return;
        }
        stunCoro = StartCoroutine(StunIE(v));
    }

    internal void StopMove()
    {
        rb.velocity = Vector3.zero;
        ani.SetBool("running", false);
    }



}
