using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    Infected managed;

    public void SetNewManaged(Infected newManaged)
    {
        if (managed != null)
            managed.State = InfectedState.Auto;
        newManaged.State = InfectedState.Managed;
        managed = newManaged;
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

}
