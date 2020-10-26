using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Nodo
{
    public float posx;
    public float posy;
    public float posz;

    public int type;
    public int points;
    public int total_nodes;
    public int used_nodes;
    public int owner;
    public int healingFactor;
    public int dmgFactor;
    public int identifier;
    public List<int> objectives;
    public Nodo(GameObject nodo)
    {
        this.posx = nodo.transform.position.x;
        this.posy = nodo.transform.position.y;
        this.posz = nodo.transform.position.z;

        Seleccion_y_Union data = nodo.GetComponent<Seleccion_y_Union>();
        this.type = data.type;
        this.points = data.points;
        this.total_nodes = data.total_nodes;
        this.used_nodes = data.used_nodes;
        this.owner = data.owner;
        this.healingFactor = data.healingFactor;
        this.dmgFactor = data.dmgFactor;
        this.identifier = data.identifier;


        objectives = new List<int>();
    }
    public void addObj(int data)
    {
        objectives.Add(data);
    }
}

public class end_Turn : MonoBehaviour
{
    Info controller;
    List<Nodo> nodos = new List<Nodo>();
    public GameObject laData;
    public List<string> many_jsons;

    void OnMouseDown()
    {
        controller = transform.parent.parent.gameObject.GetComponent<Info>();
        controller.refreshNodes();
        controller.turn++;
        int winCalculator=0;
        int winner=0;
        for(int i =0;i<controller.players_number;i++)
        {
            if(controller.defeated[i]==false)
            {
                winCalculator++;
                winner=i+1;
            }
        }
        if(winCalculator!=1)
        {
            while(true)
            {
                controller.player_turn++;
                if(controller.player_turn>controller.players_number)
                {
                    controller.player_turn=1;
                }
                if(controller.defeated[controller.player_turn-1]==false)
                {
                    break;
                }
            }
        }
        else
        {
            //terminar juego y hacer ganador a: winner
        }

        Debug.Log(controller.nodos);
        PrepareData();
    }

    void PrepareData()
    {
        for (int i = 0; i < controller.nodos.Count; i++)
        {
            Nodo nodo = new Nodo(controller.nodos[i]);
            nodos.Add(nodo);

        }
        //falta objectives
        //hacemos de nuevo el for para que ahora que estan creados, sacar los objetivos por referencia
        for (int i = 0; i < controller.nodos.Count; i++)
        {
            GameObject nodo_base = controller.nodos[i];
            for (int j = 0; j < controller.nodos[i].GetComponent<Seleccion_y_Union>().total_nodes; j++)
            {
                //necesito equiparar los nodos 
                for (int k = 0; k < controller.nodos.Count; k++)
                {
                    GameObject nodo_Objetivo = controller.nodos[k];
                    if (nodo_base.GetComponent<Seleccion_y_Union>().objectives[j] == nodo_Objetivo)
                    {
                        nodos[i].addObj(k);
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < nodos.Count; i++)
        {
            string jsonStr = JsonUtility.ToJson(nodos[i]);
            many_jsons.Add(jsonStr);
        }
        Debug.Log(many_jsons);
        for (int i = 0; i < nodos.Count; i++)
        {
            Debug.Log(many_jsons[i]);
        }
        laData = GameObject.Find("DataAGuardar");
        laData.GetComponent<DataPaso>().json = many_jsons.Select(x => x).ToList();
        laData.GetComponent<DataPaso>().turn = controller.turn;
        laData.GetComponent<DataPaso>().player_turn = controller.player_turn;
        laData.GetComponent<DataPaso>().players_number = controller.players_number;
        laData.GetComponent<DataPaso>().defeated = controller.defeated;
        SceneManager.LoadScene("ZeldGame");
    }

    void Start()
    {

        controller = transform.parent.parent.gameObject.GetComponent<Info>();
        GameObject laData = GameObject.Find("DataAGuardar");
        if (laData.GetComponent<DataPaso>().json.Count == 0)
        {
            controller.turn = 1;
            controller.player_turn = 1;
            controller.players_number = 4;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
