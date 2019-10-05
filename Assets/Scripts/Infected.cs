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
    private const float AutoSpeed = 3;
    private const float ManagedSpeed = 10;
    public InfectedState State {get;set;} = InfectedState.Auto;
    Rigidbody2D rb2D;
    List<RaycastHit2D> hits = new List<RaycastHit2D>(10);
    float _distance = 100f;
    ContactFilter2D contactFilter = new ContactFilter2D();

    [SerializeField] float speed;
    private Vector2 _dir = Vector2.right;
    private Vector2 Dir {
        get {return _dir;} 
        set { 
            _dir = value; 
            /*var v3Dir = new Vector3(_dir.x, _dir.y, 0);
            var v3DirZ = new Vector3(0, 0, -1);
            transform.rotation = Quaternion.LookRotation(v3Dir, v3DirZ)*/;
        }
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
/* var dir = WorldPos - transform.position;
 var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
 transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); */
    void Update()
    {
        switch(State)
        {
            case InfectedState.Auto:
                MoveAuto();
                break;
            default: 
                break;
        }
    }
    void FixedUpdate()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Space)){
                RaycastEnemy();
            }
        }
    }

    private void MoveAuto()
    {
        var deltaPos = Dir * speed * Time.deltaTime;
        var newPos = transform.position + new Vector3(deltaPos.x, deltaPos.y,0);
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
        if (Dir == Vector2.up){
            Dir = Vector2.right;
        } else if(Dir == Vector2.right){
            Dir = Vector2.down;
        } else if (Dir == Vector2.down){
            Dir = Vector2.left;
        } else if (Dir == Vector2.left){
            Dir = Vector2.up;
        }
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
