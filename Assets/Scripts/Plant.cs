using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameManager gameManager;
    AudioSource m_MyAudioSource;

    void Start(){
        m_MyAudioSource = GetComponent<AudioSource>();        
    }
    void Update()
    {
        if (Input.anyKeyDown){
            if (Input.GetKeyDown(KeyCode.Space)){
                if (!gameManager.HasManageble()){
                    gameManager.UseSpores(transform.position);
                    m_MyAudioSource.Play();
                }
            }
        }
    }
}
