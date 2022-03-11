using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Character : MonoBehaviour, IHit
{
    public virtual void Move() { }
    public virtual void Attack() { }
    public void OnHit(int _damge)
    {
        throw new System.NotImplementedException();
    }
    public void OnTakeDamge()
    {
        throw new System.NotImplementedException();
    }
    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

}
