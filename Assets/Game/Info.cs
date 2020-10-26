using System.Collections;
using System.Collections.Generic;
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

    //datos para la partida
    public int players_number; // cuantos players hay en la partida 
    public int turn; // Cuantos Turnos LLevamos
    public int player_turn; // De Quien es el turno 

    void setup()
    {
        Debug.Log(nodos.Count);
        if (nodos.Count == 0)
        {
            defaultstart(); // si es la primera vez se abre la partida, creamos lugares por default
        }
        else
        {

        }
    }

    void defaultstart()
    {
        GameObject e = Instantiate(Normal, new Vector3(7, 3, -1), Quaternion.identity);
        GameObject n = Instantiate(Extra, new Vector3(7, -3, -1), Quaternion.identity);
        GameObject a = Instantiate(Ataque, new Vector3(-7, 3, -1), Quaternion.identity);
        GameObject d = Instantiate(Defensa, new Vector3(-7, -3, -1), Quaternion.identity);
        e.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        n.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        a.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;
        d.GetComponent<Seleccion_y_Union>().turnController = this.gameObject;

        e.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        n.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        a.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;
        d.GetComponent<Seleccion_y_Union>().msgGameObject = this.gameObject;

        e.GetComponent<Seleccion_y_Union>().owner = 1;
        n.GetComponent<Seleccion_y_Union>().owner = 2;
        a.GetComponent<Seleccion_y_Union>().owner = 3;
        d.GetComponent<Seleccion_y_Union>().owner = 4;

        nodos.Add(e);
        nodos.Add(n);
        nodos.Add(a);
        nodos.Add(d);

        defeated = new List<bool> { false, false, false, false }; // si saber quien pierde
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
        setup();
    }

}
