using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAAAAA : MonoBehaviour
{
    Info controller;
    List<Nodo> nodos;

    public List<string> many_jsons;

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
        public Nodo(GameObject nodo, int identifier)
        {
            this.identifier = identifier;

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
        }
        public void addObj(int data)
        {
            objectives.Add(data);
        }
    }


    void Start()
    {
        controller = transform.parent.parent.gameObject.GetComponent<Info>();
        
    }

    void PrepareData()
    {
        for (int i = 0; i < controller.nodos.Count; i++)
        {
            Nodo nodo = new Nodo(controller.nodos[i], i);
            nodos.Add(nodo);
            /*
            type.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().type);
            points.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().points);
            total_nodes.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().total_nodes);
            used_nodes.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().used_nodes);
            owner.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().owner);
            healingFactor.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().healingFactor);
            dmgFactor.Add(controller.nodos[i].GetComponent<Seleccion_y_Union>().dmgFactor);

            posx.Add(controller.nodos[i].transform.position.x);
            posy.Add(controller.nodos[i].transform.position.y);
            posz.Add(controller.nodos[i].transform.position.z);
            */

        }
        //falta objectives
        //hacemos de nuevo el for para que ahora que estan creados, sacar los objetivos por referencia
        for (int i = 0; i < controller.nodos.Count; i++)
        {
            GameObject nodo_base = controller.nodos[i];
            for (int j = 0; j < controller.nodos[i].GetComponent<Seleccion_y_Union>().total_nodes; j++)
            {
                //necesito equiparar los nodos 
                GameObject nodo_Objetivo = controller.nodos[j];
                for (int k = 0; k < controller.nodos[j].GetComponent<Seleccion_y_Union>().total_nodes; k++)
                {
                    if (nodo_base.GetComponent<Seleccion_y_Union>().objectives[k] == nodo_Objetivo)
                    {
                        nodos[i].addObj(j);
                        break;
                    }
                }
            }
        }
        for (int i = 0; i< nodos.Count; i++)
        {
            string jsonStr = JsonUtility.ToJson(nodos[i]);
            many_jsons.Add(jsonStr);
        }
        Debug.Log(many_jsons);
        for (int i = 0; i < nodos.Count; i++)
        {
            Debug.Log(many_jsons[1]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
