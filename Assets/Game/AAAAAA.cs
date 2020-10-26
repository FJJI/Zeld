using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AAAAAA : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("ZeldGame");
    }

    private void Start()
    {
        GameObject laData = GameObject.Find("DataAGuardar");
        Debug.Log(laData.GetComponent<DataPaso>().json);
        Debug.Log(laData.GetComponent<DataPaso>().json[0]);
    }

}
