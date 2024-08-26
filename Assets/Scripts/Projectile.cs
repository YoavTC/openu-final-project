using System;
using CodeMonkey.Utils;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private float damage;
    private float speed;
    private bool slerp;
    private float slerpAngle;

    private Action method;

    public void Init(Enemy target, float damage, float speed, bool slerp, float slerpAngle = 0f)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.slerp = slerp;
        this.slerpAngle = slerpAngle;

        //method = slerp ? SlerpUpdate : LerpUpdate;
        
        //calculate target's isDead state
        target.CalculateDamage(damage);
    }

    private Vector3 difference;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            difference = target.transform.position - transform.position;
       
            //call slerp/lerp according to angle of projectile
            LerpUpdate();
            //method?.Invoke();

            if (difference.sqrMagnitude <= 1f)
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void LerpUpdate()
    {
        Vector3 moveDirection = difference.normalized;
        float angle = UtilsClass.GetAngleFromVector(moveDirection);
        
        transform.eulerAngles = new Vector3(0, 0, angle);
        transform.position += moveDirection * (speed * Time.deltaTime);
    }

    private void SlerpUpdate()
    {
        //Use DoTween
    }
}
