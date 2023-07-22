using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject target;

    public float speed;
    public float rotateSpeed;

    public bool moving = true;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(-8, -2);
        rotateSpeed= Random.Range(-60, 60);
    }
    private void FixedUpdate()
    {
        if (moving)
        {
            Vector2 vector = (Vector2)target.transform.position - rb.position;
            vector.Normalize();

            float rotateAmunt = Vector3.Cross(vector, transform.up).z;

            rb.angularVelocity = -rotateAmunt * rotateSpeed;

            rb.velocity = transform.up * speed;
            speed += 0.5f;
            rotateSpeed += 10f;
            speed = Mathf.Clamp(speed, -Mathf.Infinity, 20f);

            if(Vector2.Distance(target.transform.position, rb.position) < 1f)
            {
                if (target.GetComponent<UnitPanel>() != null)
                {
                    target.GetComponent<UnitPanel>().Attack();
                }else if (target.GetComponent<EnemyPanel>() != null)
                {
                    target.GetComponent<EnemyPanel>().Attack();
                }
                else
                {
                    HexegonBubleManager.instance.Heal();
                }
                Destroy(gameObject);
            }
        }
    }
}
