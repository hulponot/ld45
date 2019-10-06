using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using  UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    Infected managed;
    public Camera mainCamera;
    public ParticleSystem[] spores;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite heartFull;
    public Sprite heartNull;
    private int lastSpore=0;
    private int lastHealth=1;
    private int infectableMask;
    public GameObject InfectedPrefab;
    public GameObject win;
    public GameObject lose;
    public GameObject buttonToMenu;
    public GameObject tut;
    AudioSource m_MyAudioSource;
    public void SetNewManaged(Infected newManaged)
    {
        if (managed != null)
            managed.State = InfectedState.Auto;
        newManaged.State = InfectedState.Managed;
        managed = newManaged;
        var mPos = managed.transform.position;
        mainCamera.transform.position = new Vector3(mPos.x, mPos.y, mainCamera.transform.position.z);
        mainCamera.transform.SetParent(managed.transform);
    }

    public bool HasManageble(){
        return managed != null;
    }
    public static GameManager GetInstance()
    {
        return _instance;
    }
    private GameManager(){
        if (_instance == null){
            _instance = this;
        }
    }

    void Start(){
        
        infectableMask = LayerMask.GetMask("Infectable");
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if (managed != null){
            var nHealth = managed.GetHealth();
            if (nHealth != lastHealth){
                lastHealth = nHealth;
                if (nHealth > 2){
                    heart3.sprite = heartFull;
                }
                else {heart3.sprite = heartNull;}
                if (nHealth > 1){
                    heart2.sprite = heartFull;
                } else {heart2.sprite = heartNull;}
                if (nHealth > 0){
                    heart1.sprite = heartFull;
                } else {heart1.sprite = heartNull;}
            }
        }
        else {
            if (lastHealth != 0){
                heart1.sprite = heartNull;
                heart2.sprite = heartNull;
                heart3.sprite = heartNull;
            }
        }
    }
    public void InfetedDead(Infected infected){
        if (managed == infected){
            managed = null;
            var infecteds = FindObjectsOfType(typeof(Infected));
            for (int i=0;i < infecteds.Length;i++){
                if (infecteds[i] == infected) continue;
                if (((Infected)infecteds[i]).IsDead()) continue;

                SetNewManaged((Infected)infecteds[i]);
                infected.transform.gameObject.SetActive(false);
                break;
            }
            if (managed == null){
                
                
                mainCamera.transform.SetParent(transform.parent);
                Lose();
            }
            
        }
        else {
            infected.transform.gameObject.SetActive(false);
        }
    }

    private void Lose(){
        lose.SetActive(true);
        buttonToMenu.SetActive(true);
    }
    public void UseSpores(Vector3 position)
    {
        m_MyAudioSource.Play();
        lastSpore = ++lastSpore % spores.Length;

        var spore = spores[lastSpore];
        spore.transform.position = position;
        spore.Play();

        var colliders = Physics2D.OverlapCircleAll(position, 5, infectableMask);
        for (int i=0;i< colliders.Length;i++){
            MakeInfactableInfected(colliders[i].gameObject);
        }


        var enemies = FindObjectsOfType(typeof(Enemy));
        if (enemies.Length == 0){
            var simpleMans = FindObjectsOfType(typeof(SImpleMan));
            if (simpleMans.Length == 0){
                Win();
            }
        }              
        
    }

    void Win(){
        win.SetActive(true);
        buttonToMenu.SetActive(true);
    }
    public void MakeInfactableInfected(GameObject infectble){
        Instantiate(InfectedPrefab, infectble.transform.position,Quaternion.identity);
        infectble.SetActive(false);
    }

    public void BackToMenu(){
        SceneManager.LoadScene("Menu");
    }

    public void HideTut(){
        tut.SetActive(false);
    }
}
