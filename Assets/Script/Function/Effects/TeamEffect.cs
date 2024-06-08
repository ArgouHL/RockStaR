using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeamEffect :MonoBehaviour
{
    internal abstract void StartEffect(Team team);

    internal abstract void StopEffect();

}
