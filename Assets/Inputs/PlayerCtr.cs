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
    private bool movePress => currentMovement.x != 0 || currentMovement.y != 0;
     private Vector2 moveDirection;
     private CapsuleCollider _collider;
    

    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField] private float minSpeed = 2f;
    [SerializeField]
    private float acceleratePerSecond = 10f;
    [SerializeField]
    private float deceleratePerSecond = 10f;
    private float currentSpeed = 0;


    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField] private bool isGround;

    [SerializeField]
    private float rotationPerSecond = 10f;

    private bool isBackwarding;
    private Coroutine backwardingCoro;



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
        _collider = GetComponent<CapsuleCollider>();
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
      if(CheckGround())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool CheckGround()
    {
        Vector3 p1 = transform.position + _collider.center + Vector3.up * (-_collider.height * 0.25f-0.1f);
        Vector3 p2 = p1 + Vector3.up * _collider.height;
        var cols = Physics.OverlapCapsule(p1, p2, _collider.radius, 1 << 6);

        if (cols.Length != 0)
            return true;
        else
            return false;
    }

    private void Update()
    {
        isGround = CheckGround();
    }

    private void FixedUpdate()
    {
        float currentAngle = 0;
        if (movePress)
            currentAngle = Rotate();
        Movement(currentAngle);

    }

    private float Rotate()
    {
        var v = MathHalper.AngleBetween(currentMovement, new Vector2(transform.forward.x, transform.forward.z));
        if (v > 60)
        {
            if (backwardingCoro != null)
                return v;
            backwardingCoro = StartCoroutine(backwardingIE(600, currentMovement));
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
        moveDirection = Vector3.RotateTowards(moveDirection, currentMovement.normalized, rotationPerSecond * Mathf.Deg2Rad * Time.fixedDeltaTime, 1);
        transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.y));
        return v;

    }

    private IEnumerator backwardingIE(float rotateSpeed, Vector2 targetDir)
    {
        isBackwarding = true;
        Debug.Log("back");
        while (MathHalper.AngleBetween(targetDir, new Vector2(transform.forward.x, transform.forward.z)) > 1f)
        {
            moveDirection = Vector3.RotateTowards(moveDirection, targetDir, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1);

            transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.y));
            yield return null;
        }
        isBackwarding = false;
        backwardingCoro = null;
    }



    private void Movement(float rotatAngle)
    {
        float speedFactor = Mathf.Lerp(1, 0, (rotatAngle + 10) / 270);


        if (movePress && !isBackwarding)
        {

            currentSpeed = currentSpeed < maxSpeed ? currentSpeed + acceleratePerSecond * Time.fixedDeltaTime : maxSpeed;

        }
        else if (movePress && isBackwarding)
        {
            if (currentSpeed > minSpeed)
                currentSpeed -= deceleratePerSecond * Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed = currentSpeed > 0.01f ? currentSpeed - deceleratePerSecond * Time.fixedDeltaTime : 0;
        }


        if (currentSpeed > 0)
        {
            rb.velocity = new Vector3(moveDirection.x, 0, moveDirection.y) * currentSpeed * speedFactor + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y,0);
        }




        //  gameObject.transform.position += new Vector3(moveDirection.x, 0, moveDirection.y) * Time.fixedDeltaTime * currentSpeed* speedFactor;
        //    Debug.Log(currentMovement);

    }
}
