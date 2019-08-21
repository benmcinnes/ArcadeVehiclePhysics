using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CaAbility : MonoBehaviour
{
    public string axisKey = "New Ability";
    public KeyCode abilityButton;
    public abstract void CheckInput();
    public abstract void DoAbility();

}
