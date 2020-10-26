using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Info : MonoBehaviour
{
    public List<GameObject> nodos;
    public List<Vector3> posiciones;
    public GameObject Normal;
    public GameObject Extra;
    public GameObject Ataque;
    public GameObject Defensa;
    public List<bool> defeated;
    public List<GameObject> tipoNodos = new List<GameObject>();
    public GameObject laData;

    //datos para la partida
    public int players_number; // cuantos players hay en la partida 
    public int turn; // Cuantos Turnos LLevamos
    public int player_turn; // De Quien es el turno 
    public int IdSala;

    public List<string> json;
    public List<Nodo> nodosIngresados = new List<Nodo>();

    void setup()
    {
        Debug.Log(json.Count);
        if (json.Count == 0)
        {
            defaultstart(); // si es la primera vez se abre la partida, creamos lugares por default
        }
        else
        {
            fixedStart();
        }
    }

    void defaultstart()
    {
        GameObject e = Instantiate(Normal, new Vector3(7, 3, -1), Quaternion.identity);
        GameObject n = Instantiate(Extra, new Vector3(7, -3, -1), Quaternion.identity);
        GameObject a = Instantiate(Ataque, new Vector3(-7, 3, -1), Quaternion.identity);
        GameObject d = Instantiate(Defensa, new Vector3(-7, -3, -1), Quaternion.identity);
        GameObject n2 = Instantiate(Normal, new Vector3(4, -2, -1), Quaternion.identity);
        GameObject n3 = Instantiate(Normal, new Vector3(4, 2, -1), Quaternion.identity);
        GameObject n4 = Instantiate(Normal, new Vector3(-4, -2, -1), Quaternion.identity);
        GameObject n5 = Instantiate(Normal, new Vector3(-4, 2, -1), Quaternion.identity);
        GameObject n6 = Instantiate(Normal, new Vector3(0, 3, -1), Quaternion.identity);
        GameObject n7 = Instantiate(Normal, new Vector3(0, -3, -1), Quaternion.identity);


        e.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        a.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        d.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n2.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n3.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n4.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n5.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n6.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n7.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;

        e.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        a.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        d.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n2.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n3.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n4.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n5.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n6.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n7.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;

        e.GetComponent<Seleccion_y_Union>().owner = 1;
        n.GetComponent<Seleccion_y_Union>().owner = 2;
        a.GetComponent<Seleccion_y_Union>().owner = 3;
        d.GetComponent<Seleccion_y_Union>().owner = 4;
        n2.GetComponent<Seleccion_y_Union>().owner = 0;
        n3.GetComponent<Seleccion_y_Union>().owner = 0;
        n4.GetComponent<Seleccion_y_Union>().owner = 0;
        n5.GetComponent<Seleccion_y_Union>().owner = 0;
        n6.GetComponent<Seleccion_y_Union>().owner = 0;
        n7.GetComponent<Seleccion_y_Union>().owner = 0;

        e.GetComponent<Seleccion_y_Union>().identifier = 0;
        n.GetComponent<Seleccion_y_Union>().identifier = 1;
        a.GetComponent<Seleccion_y_Union>().identifier = 2;
        d.GetComponent<Seleccion_y_Union>().identifier = 3;
        n2.GetComponent<Seleccion_y_Union>().identifier = 4;
        n3.GetComponent<Seleccion_y_Union>().identifier = 5;
        n4.GetComponent<Seleccion_y_Union>().identifier = 6;
        n5.GetComponent<Seleccion_y_Union>().identifier = 7;
        n6.GetComponent<Seleccion_y_Union>().identifier = 8;
        n7.GetComponent<Seleccion_y_Union>().identifier = 9;

        nodos.Add(e);
        nodos.Add(n);
        nodos.Add(a);
        nodos.Add(d);
        nodos.Add(n2);
        nodos.Add(n3);
        nodos.Add(n4);
        nodos.Add(n5);
        nodos.Add(n6);
        nodos.Add(n7);

        defeated = new List<bool> { false, false, false, false }; // si saber quien pierde
    }

    public void fixedStart()
    {
        for (int i = 0; i < json.Count; i++)
        {
            Nodo nodo = JsonUtility.FromJson<Nodo>(json[i]);
            nodosIngresados.Add(nodo);
            GameObject nodoActivo = Instantiate(tipoNodos[nodo.type], new Vector3(nodo.posx, nodo.posy, nodo.posz), Quaternion.identity);
            Seleccion_y_Union data = nodoActivo.GetComponent<Seleccion_y_Union>();
            data.points = nodo.points;
            data.total_nodes = nodo.total_nodes;
            data.used_nodes = nodo.used_nodes;
            data.owner = nodo.owner;
            data.healingFactor = nodo.healingFactor;
            data.dmgFactor = nodo.dmgFactor;
            data.identifier = nodo.identifier;
            data.turnController = this.gameObject;
            data.msgGameObject = this.gameObject;
            nodos.Add(nodoActivo);
            Debug.Log(nodo.points + " Nodo Points");
            Debug.Log(data.points + " Data Points");


            for (int j = 0; j < data.total_nodes; j++)
            {
                //hi
            }
        }

        for (int i = 0; i < json.Count; i++)
        {
            Seleccion_y_Union data = nodos[i].GetComponent<Seleccion_y_Union>();
            for (int j = 0; j < data.total_nodes; j++)
            {
                data.objectives.Add(null);
                data.unions.Add(null);
                try {
                    if (nodosIngresados[i].objectives[j] < j)
                    {
                        continue;
                    }
                    else
                    {
                        Debug.Log(data.objectives.Count + " i " + i + "aaa " + nodosIngresados[i].objectives[j]);
                        data.objectives[i] = nodos[nodosIngresados[i].objectives[j]];
                        Seleccion_y_Union.first = nodos[i];
                        data.objectives[i].GetComponent<Seleccion_y_Union>().ArrowCreation(j, data, Seleccion_y_Union.first);
                        Seleccion_y_Union.first = null;
                    }
                } catch { continue; }
            }
        }
    }

    public void refreshNodes()
    {
        defeated=new List<bool> {true,true,true,true};
        foreach (var item in nodos)
        {
            item.GetComponent<Seleccion_y_Union>().turnUpdate();
            if(item.GetComponent<Seleccion_y_Union>().owner!=0)
            {
                defeated[item.GetComponent<Seleccion_y_Union>().owner-1]=false;
            }
            
        }
    }

    private void Start()
    {
        GameObject laData = GameObject.Find("DataAGuardar");
        DataPaso info = laData.GetComponent<DataPaso>();
        players_number = info.players_number;
        turn = info.turn;
        player_turn = info.player_turn;
        IdSala = info.IdSala;
        if (laData.GetComponent<DataPaso>().json.Count > 0) 
        {
            json = laData.GetComponent<DataPaso>().json;

        }

        setup();
    }

}
