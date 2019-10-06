using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;

    Rigidbody2D rb2D;
    List<RaycastHit2D> hits = new List<RaycastHit2D>(10);
    ContactFilter2D contactFilter = new ContactFilter2D();
    public float _distance = 80;
    private Infected target;
    private List<Infected> nearInfected = new List<Infected>(15);

    public float IntencityOfAttack = 1f;
    private DateTime lastAttack;
    private Animator anima;
    int wallMask;
    AudioSource m_MyAudioSource;
    void Start()
    {
        gameManager = GameManager.GetInstance();
        rb2D = GetComponent<Rigidbody2D>();
        wallMask = LayerMask.GetMask("Wall");
        lastAttack = DateTime.Now;
        anima = GetComponent<Animator>();
        m_MyAudioSource = GetComponent<AudioSource>();        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastEnemy();
        if (target != null){
            transform.right = target.transform.position - transform.position;
            if ((DateTime.Now - lastAttack).TotalSeconds > IntencityOfAttack){
                Attack();
            }
        }
    }

    private void Attack(){
        lastAttack = DateTime.Now;
        anima.SetTrigger("pew");
        m_MyAudioSource.Play();
        target.Damaged();
    }

    private void RaycastEnemy()
    {
        nearInfected.Clear();
        target = null;
        var colliders = Physics2D.OverlapCircleAll(transform.position, _distance);
        for (int i=0;i< colliders.Length;i++){
            var infected = colliders[i].GetComponent<Infected>();
            if (infected == null)
                continue;
            nearInfected.Add(infected);
        }
        if (nearInfected.Count > 0) Debug.Log(nearInfected.Count);
        if (nearInfected.Count > 0){
            for (int i=0;i< nearInfected.Count;i++){
                var walls = Physics2D.Linecast(transform.position, nearInfected[i].transform.position, wallMask);
                
                if (walls.collider == null){

                    if (target == null 
                        || Vector3.Distance(transform.position, target.transform.position)
                            > 
                            Vector3.Distance(transform.position, nearInfected[i].transform.position)){
                                target = nearInfected[i];
                            }
                }
            }
        }
    }
}
