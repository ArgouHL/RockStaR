using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesSys : MonoBehaviour
{
    public static HolesSys instance;
    [SerializeField] private HoleCtr redHole;
    [SerializeField] private HoleCtr blueHole;

    public bool modeC;

    private Coroutine shieldCoro;


    private void Awake()
    {
        instance = this;
    }

    internal void Shield()
    {
        if (!modeC)
            return;
        if (shieldCoro != null)
            return;
        shieldCoro = StartCoroutine(ShieldIE());
    }

    private IEnumerator ShieldIE()
    {
        SetShield();
        yield return new WaitForSeconds(5);
        DisableShield();
        shieldCoro = null;
    }


    internal void SetShield()
    {
        redHole.EnableShield(true);
        blueHole.EnableShield(true);
    }

    internal void DisableShield()
    {
        redHole.EnableShield(false);
        blueHole.EnableShield(false);
    }
}
