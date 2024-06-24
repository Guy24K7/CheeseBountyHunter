using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDowns
{
    private float lastTimeUsed;//last time when used the ability 
    private float coolDownTime;//the cooldown time of the ability
    private float abilityDuration;//the duration of the ability 
    private float timeWhenActive;//time when the ability was activated

    public CoolDowns(float coolDownTime, float abilityDuration, float lastTimeUsed)//a constructor for an ability with a limited duration
    {
        this.coolDownTime = coolDownTime;
        this.abilityDuration = abilityDuration;
        this.lastTimeUsed = lastTimeUsed;
    }
    public CoolDowns(float coolDownTime, float lastTimeUsed)//a constructor for an ability with infinite duration
    {
        this.coolDownTime = coolDownTime;
        this.lastTimeUsed = lastTimeUsed;
    }
    public bool IsCooldownOff()//Can the ability be used again?
    {
        return Time.time >= lastTimeUsed + coolDownTime;
    }
    public bool IsAbilityFinish()//did the duration of the ability has ended?
    {
        return Time.time >= timeWhenActive + abilityDuration;
    }
    public void SetTimeWhenActive(float timeWhenActive)//set function for timeWhenActive
    {
        this.timeWhenActive = timeWhenActive;
    }
    public float LastTimeUsed//get and set functions for lastTimeUsed
    {
        get { return this.lastTimeUsed; }      
        set { this.lastTimeUsed = value; }
    }
    public float CoolDownTime//get and set functions for coolDownTime
    {
        get { return this.coolDownTime; }
        set { this.coolDownTime = value; }
    }
}

