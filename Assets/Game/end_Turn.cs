using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_Turn : MonoBehaviour
{
    Info controller;
    List<int> remaining = new List<int> { 0,0,0,0 };
    List<float> posx;
    List<float> posy;
    List<float> posz;
    List<int> type_node;


    void OnMouseDown()
    {
        Info controller = transform.parent.parent.gameObject.GetComponent<Info>();
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
        
        
        /*
        for (int i = 0; i < controller.nodos.Count; i++)
        {
            remaining[controller.nodos[i].GetComponent<Seleccion_y_Union>().owner]++;
        }
        PrepareData();
        */
    }

    void PrepareData()
    {
        for (int i = 0; i < controller.nodos.Count; i++)
        {
            type_node.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().type);
        }
    }

    void Start()
    {
        controller = transform.parent.parent.gameObject.GetComponent<Info>();
        controller.turn=1;
        controller.player_turn=1;
        controller.players_number=4;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
