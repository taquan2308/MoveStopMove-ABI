using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAnimation : CharaterAnimation, IInitializeVariables, ISubcribers
{
    private PlayerMain playerMain;
    
    void Start()
    {
        InitializeVariables();
        SubscribeEvent();//
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        UnSubscribeEvent();//
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
    }
    

    public void SubscribeEvent()
    {
        playerMain.OnAttack += PlayAttackAnimation;//
        playerMain.OnDance += PlayDanceAnimation;
        playerMain.OnDead += PlayDeadAnimation;
        playerMain.OnIdle += PlayIdleAnimation;
        playerMain.OnRun += PlayRunAnimation;
        playerMain.OnWin += PlayWinAnimation;
        //throw new System.NotImplementedException();
    }

    public void UnSubscribeEvent()
    {
        
    }
    public void AttackFromPlayerMain()
    {
        playerMain.Attack();
    }
}
