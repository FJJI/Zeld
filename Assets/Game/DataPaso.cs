using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPaso : MonoBehaviour
{
    public static DataPaso dataPaso;
    public static int Playing;
    // Este es el jugador que va a estar jugando (segun id en la partida se de si es el j1 o j2)
    public List<string> json;

    //datos para la partida
    public int players_number; // cuantos players hay en la partida 
    public int turn; // Cuantos Turnos LLevamos
    public int player_turn; // De Quien es el turno 
    public int IdSala;

    void Awake()
    {
        if (dataPaso == null)
        {
            dataPaso = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (dataPaso != this)
        {
            Destroy(gameObject);
        }
    }

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
}
