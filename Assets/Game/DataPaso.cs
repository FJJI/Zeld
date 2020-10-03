using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPaso : MonoBehaviour
{
    public static DataPaso dataPaso;

    void Awake()
    {
        if (dataPaso == null)
        {
            dataPaso = this;
            DontDestroyOnLoad(Gameobject);
        }
        else if (dataPaso != this)
        {
            Destroy(gameObject);
        }
    }

    // Ahora Aca dejaremos todo el codigo que queramos ir pasando entre escenas, este solo estara 1 vez, asi que debiese ser mas facil de tratar 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
