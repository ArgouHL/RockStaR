using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JewelryCtr : MonoBehaviour
{
    private Rigidbody rig;
    private Collider collider;
    [SerializeField] private float bePushedAddSpeed = 10;
    [SerializeField] private float bePushedMaxSpeed = 50;
    [SerializeField] private float bePushedMinSpeed = 10;
    private float nowSpeed;
    [SerializeField] private float decelerateForce = 1;
    private Coroutine moveCoro;


    private Coroutine SelfMoveRepeadCoro;
    private Vector3 moveDir;


    private bool isMoving = false;
    private bool isSelfMoving = false;
    private float coolDown = 0;

    [SerializeField] private GameObject visual;
    [SerializeField] internal Material grayJew;
    [SerializeField] internal JewLight grayLight;
    [SerializeField] internal Material cyanJew;
    [SerializeField] internal JewLight cyanLight;
    [SerializeField] internal Material yellowJew;
    [SerializeField] internal JewLight yellowLight;

    [SerializeField] internal ParticleSystem appearEffect;
    [SerializeField] internal EffectSwitcher countEffect;
    [SerializeField] internal EffectSwitcher disappearEffect;
    [SerializeField] internal EffectSwitcher absorbEnergyEffect;
    internal CrystalSfxControl crystalSfxControl;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        crystalSfxControl = GetComponent<CrystalSfxControl>();
        Inst();
    }


    internal void Appear(float radius)
    {
        appearEffect.Play();

        var v2 = Random.insideUnitCircle * (radius-4f);
        transform.position = new Vector3(v2.x, 1, v2.y);
        Inst();
        collider.enabled = true;
        crystalSfxControl.PlayCrystalAppearSfx();
        VisualAppear();
        float angle = Random.Range(0f,360f) * Mathf.Deg2Rad;
        BePush(new Vector3(Mathf.Cos(angle), 0, -Mathf.Sin(angle)).normalized);
        countEffect.StartEffect(Team.None);
    }

   

    internal void Inst()
    {
        SetTeamVisual(Team.None);

        if (JewelrySystem.instance.modeA)
            nowSpeed = bePushedMinSpeed;
        else
            nowSpeed = bePushedMaxSpeed;
    }




    internal void SetTeamVisual(Team team)
    {
        Light light = GetComponentInChildren<Light>();
        switch (team)
        {
            case Team.Blue:
                visual.GetComponent<MeshRenderer>().material = new Material(cyanJew);
                light.color = cyanLight.color;
                light.intensity = cyanLight.intensity;
                break;
            case Team.Yellow:
                visual.GetComponent<MeshRenderer>().material= new Material(yellowJew);

                light.color = yellowLight.color;
                light.intensity = yellowLight.intensity;

                break;
            case Team.None:
                visual.GetComponent<MeshRenderer>().material= new Material(grayJew);
                light.color = grayLight.color;
                //light.intensity = grayLight.intensity;
                ;
                break;
            default:
                break;
        }
    }

    private void VisualAppear()
    {
        SetTeamVisual(Team.None);
        Light light = GetComponentInChildren<Light>();
        Material mat= visual.GetComponent<MeshRenderer>().material;
        float emmisonStrange = mat.GetFloat("_EmiStrenge");
        LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => {
            mat.SetFloat("_EmiStrenge", emmisonStrange * val);
            light.intensity = grayLight.intensity * val;
        });
        visual.SetActive(true);
    }

    internal void BePush(Vector3 normalizedVector)
    {

        if (JewelrySystem.instance.modeA)
            nowSpeed = nowSpeed >= bePushedMaxSpeed ? bePushedMaxSpeed : nowSpeed + bePushedAddSpeed;
        Move(normalizedVector);
        crystalSfxControl.PlayCrystalHitSfx();
        
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
        visual.SetActive(false);
        GetComponentInChildren<Light>().intensity = 0;
        collider.enabled = false ;
        GetComponentInChildren<Light>().intensity = 0;
        countEffect.StopEffect();


    }
    internal void EndStop()
    {
        if (moveCoro != null)
            StopCoroutine(moveCoro);
        Stop();


    }

    #region bounce
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("PlayerBoundary"))
        {
            crystalSfxControl.PlayCrystalHitSfx();
            moveDir = Vector3.Reflect(moveDir, collision.contacts[0].normal);
            //nowSpeed = nowSpeed<=bePushedMinSpeed? bePushedMinSpeed: nowSpeed - bePushedAddSpeed;
            
        }
    }

    #endregion
}
