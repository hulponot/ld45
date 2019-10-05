using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody2D rb2D;
    List<RaycastHit2D> hits = new List<RaycastHit2D>(10);

    private Infected target;
    void Start()
    {
        gameManager = GameManager.GetInstance();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RaycastEnemy()
    {
        int results = Physics2D.CircleCastAll(transform.position, Vector2.right, contactFilter.NoFilter(), hits, _distance);

        for (int i=0;i<results;i++)
        {
            if(hits[i].collider != null)
            {
                var go = hits[i].collider.gameObject;
                if (go.tag == "Wall") break;

                go.SetActive(false);
            }
        }
        /*
        if (hit.collider != null)
        {
            // Calculate the distance from the surface and the "error" relative
            // to the floating height.
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            float heightError = floatHeight - distance;

            // The force is proportional to the height error, but we remove a part of it
            // according to the object's speed.
            float force = liftForce * heightError - rb2D.velocity.y * damping;

            // Apply the force to the rigidbody.
            rb2D.AddForce(Vector3.up * force);
        }
        */
    }
}
