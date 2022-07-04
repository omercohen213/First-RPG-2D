using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hp = 10;
    public int maxHp = 10;
    public float pushTolerance = 0.2f;

    // Immunity 
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    // Push
    protected Vector3 pushDirection;

    protected virtual void RecieveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hp -= dmg.dmgAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            GameManager.instance.ShowText(dmg.dmgAmount.ToString(), 20, Color.red, dmg.origin, Vector3.up, 1f);


            if (hp < 0)
            {
                hp = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        Debug.Log("dead");
    }
}
