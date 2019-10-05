using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InfectedState
{
    Managed,
    Auto
}

public class Infected : MonoBehaviour
{
    GameManager gameManager;
    private const float AutoSpeed = 3;
    private const float ManagedSpeed = 15;
    public InfectedState State {get;set;} = InfectedState.Auto;
    Rigidbody2D rb2D;
    List<RaycastHit2D> hits = new List<RaycastHit2D>(10);
    float _distance = 100f;
    ContactFilter2D contactFilter = new ContactFilter2D();

    [SerializeField] float speed;

    void Start()
    {
        gameManager = GameManager.GetInstance();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }
    void FixedUpdate()
    {
        switch(State)
        {
            case InfectedState.Auto:
                MoveAuto();
                break;
            case InfectedState.Managed:
                MoveManaged();
                break;
            default: 
                break;
        }
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Space)){
                RaycastEnemy();
            }
        }
    }

    private void MoveManaged()
    {
        if (Input.anyKey){
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            var deltaPos = transform.TransformVector(new Vector3(x,y,0) * ManagedSpeed * Time.deltaTime);
            var newPos = transform.position + deltaPos;
            var angle = Vector2.Angle(transform.right, deltaPos);
            //transform.right = newPos - transform.position;
            //var angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            //transform.LookAt(newPos, new Vector3(0,0,-1));
            rb2D.MovePosition(newPos);
            Quaternion toRotation = Quaternion.FromToRotation(transform.right, deltaPos);
            transform.localRotation = toRotation;
        }
    }
    private void MoveAuto()
    {
        var deltaPos = Vector2.right * speed * Time.deltaTime;
        var newPos = transform.position + transform.TransformVector(new Vector3(deltaPos.x, deltaPos.y,0));
        rb2D.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;
        if (go.tag == "Wall"){
            NextDir();
        } else if (go.tag == "Enemy"){
            Debug.Log("attack!");
        } else {
            NextDir();
        }
    }

    void NextDir()
    {
        transform.Rotate(0,0,-90);
        return;
    }

    void OnMouseDown()
    {
        gameManager.SetNewManaged(this);
    }
    private void RaycastEnemy()
    {
        //public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, float distance = Mathf.Infinity); 
        int results = Physics2D.Raycast(transform.position, Vector2.right, contactFilter.NoFilter(), hits, _distance);

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
