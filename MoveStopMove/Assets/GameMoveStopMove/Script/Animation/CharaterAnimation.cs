using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterAnimation : MonoBehaviour
{
    private Animator animator;
    //Check State Player
    [HideInInspector]
    public enum StateCharacter
    {
        attack_Bool,
        dance_Bool,
        dead_Bool,
        idle_Bool,
        run_Bool,
        ulti_Bool,
        win_Bool
    }
    //
    protected bool isAttack;
    protected bool isDance;
    protected bool isDead;
    protected bool isIdle;
    protected bool isRun;
    protected bool isUlti;
    protected bool isWin;
    protected bool[] arrayBoolState;
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool IsDance { get => isDance; set => isDance = value;}
    public bool IsDead { get => isDead; set => isDead = value;}
    public bool IsIdle { get => isIdle; set => isIdle = value;}
    public bool IsRun { get => isRun; set => isRun = value;}
    public bool IsUlti { get => isUlti; set => isUlti = value;}
    public bool IsWin { get => isWin; set => isWin = value;}
    private void Awake()
    {
        InitializeVariablesCharAnim();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeVariablesCharAnim()
    {
        animator = GetComponent<Animator>();
        isAttack = false;
        isDance = false;
        isDead = false;
        isRun = false;
        isUlti = false;
        isWin = false;
        arrayBoolState = new bool[] { isAttack, isDance, isDead, isIdle, isRun, isUlti, isWin };
        SetStateAnimation(StateCharacter.idle_Bool);
        //throw new System.NotImplementedException();
    }
    #region Set State Player Function
    public void SetStateAnimation(StateCharacter _stateCharacter)
    {
        if (_stateCharacter == StateCharacter.attack_Bool)
        {
            isAttack = true;
            isDance = false;
            isDead = false;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = false;
            animator.SetBool("IsAttack", true);
            animator.SetBool("IsDead", false);
            animator.SetBool("IsWin", false);
            animator.SetBool("IsUlti", false);
        }
        else if (_stateCharacter == StateCharacter.dance_Bool)
        {
            isAttack = false;
            isDance = true;
            isDead = false;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = false;
            animator.SetBool("IsDead", false);
            animator.SetBool("IsWin", true);
        }
        else if (_stateCharacter == StateCharacter.dead_Bool)
        {
            isAttack = false;
            isDance = false;
            isDead = true;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = false;
            animator.SetBool("IsDead", true);
        }
        else if (_stateCharacter == StateCharacter.idle_Bool)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isRun = false;
            isIdle = true;
            isUlti = false;
            isWin = false;
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsDead", false);
            animator.SetBool("IsWin", false);
            animator.SetBool("IsDance", false);
        }
        else if (_stateCharacter == StateCharacter.run_Bool)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isRun = true;
            isIdle = false;
            isUlti = false;
            isWin = false;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsDead", false);
            animator.SetBool("IsWin", false);
        }
        else if (_stateCharacter == StateCharacter.ulti_Bool)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isRun = false;
            isIdle = false;
            isUlti = true;
            isWin = false;
            animator.SetBool("IsAttack", true);
            animator.SetBool("IsDead", false);
            animator.SetBool("IsWin", false);
            animator.SetBool("IsUlti", true);
        }
        else if (_stateCharacter == StateCharacter.win_Bool)
        {
            isAttack = false;
            isDance = false;
            isDead = false;
            isRun = false;
            isIdle = false;
            isUlti = false;
            isWin = true;
            animator.SetBool("IsWin", true);
        }
    }
    #endregion
    #region PlayAnimation
    protected void PlayAttackAnimation()
    {
        SetStateAnimation(StateCharacter.attack_Bool);
    }
    protected void PlayDanceAnimation()
    {
        SetStateAnimation(StateCharacter.dance_Bool);
    }
    protected void PlayDeadAnimation()
    {
        SetStateAnimation(StateCharacter.dead_Bool);
    }
    protected void PlayIdleAnimation()
    {
        SetStateAnimation(StateCharacter.idle_Bool);
    }
    protected void PlayRunAnimation()
    {
        SetStateAnimation(StateCharacter.run_Bool);
    }
    protected void PlayUltiAnimation()
    {
        SetStateAnimation(StateCharacter.ulti_Bool);
    }
    protected void PlayWinAnimation()
    {
        SetStateAnimation(StateCharacter.win_Bool);
    }
    #endregion
}
