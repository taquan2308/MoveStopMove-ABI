using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Character : MonoBehaviour
{
    public virtual void MoveCharater() { } //virtual method
    public virtual void IdleCharater() { } //virtual method
    public virtual void AttackCharater() { } //virtual method
}
