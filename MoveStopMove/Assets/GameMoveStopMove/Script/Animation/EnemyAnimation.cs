using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : CharaterAnimation, IInitializeVariables, ISubcribers
{
    private EnemyMain enemyMain;
    
    void Start()
    {

    }
    private void OnEnable()
    {
        InitializeVariables();
        SubscribeEvent();
    }
    private void OnDisable()
    {
        UnSubscribeEvent();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeVariables()
    {
        enemyMain = gameObject.GetComponent<EnemyMain>();
    }
    

    public void SubscribeEvent()
    {
        enemyMain.OnAttack += PlayAttackAnimation;
        enemyMain.OnDance += PlayDanceAnimation;
        enemyMain.OnDead += PlayDeadAnimation;
        enemyMain.OnIdle += PlayIdleAnimation;
        enemyMain.OnRun += PlayRunAnimation;
        enemyMain.OnWin += PlayWinAnimation;
        //throw new System.NotImplementedException();
    }

    public void UnSubscribeEvent()
    {
        enemyMain.OnAttack -= PlayAttackAnimation;
        enemyMain.OnDance -= PlayDanceAnimation;
        enemyMain.OnDead -= PlayDeadAnimation;
        enemyMain.OnIdle -= PlayIdleAnimation;
        enemyMain.OnRun -= PlayRunAnimation;
        enemyMain.OnWin -= PlayWinAnimation;
        //throw new System.NotImplementedException();
    }
}
