using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Seleccion_y_Union : MonoBehaviour
{
    GameObject textObject;
    public int points = 50;
    public int counter = 0; //fines demostrativos
    public int total_nodes = 2;
    public int used_nodes = 0;
    public static GameObject first;
    public int type;
    public CircleCollider2D collider;
    // Tipo 0 es normal, 1 es warrior, 2 es defensa, 3 es nodo extra
    public List<GameObject> objectives;
    public List<GameObject> unions;
    public GameObject arrow;
    public Component halo;
    public SpriteRenderer sprite;
    public int owner; // 0 if neutral, else to playerN

    void CheckType()
    {
        if (type == 3) // aumentamos la cantidad de nodos usables
        {
            total_nodes += 1;
        }
        // Para crear los objetivos de cada uno
        for (int i = 0; i < total_nodes; i++)
        {
            objectives.Add(null);
            unions.Add(null);
        }
    }

    void OnMouseDown()
    {
        if (!first)
        {
            Debug.Log("Seleccionado" + this.gameObject);
            first = this.transform.gameObject;
        }
        else if (first == this.transform.gameObject)
        {
            Debug.Log("Ya no hay first" + this.gameObject);
            Debug.Log("Todos los nodos eliminados" + this.gameObject);
            for (int i = 0; i < total_nodes; i++)
            {
                used_nodes -= 1;
                objectives[i] = null;
                try { Destroy(unions[i].gameObject); } catch { }
                unions[i] = null;
            }
            first = null;
        }
        else if (first != this.transform.gameObject)
        {
            Seleccion_y_Union first_code = first.GetComponent<Seleccion_y_Union>();
            //con este if, considero que quede al menos un "objective" en null
            if (first_code.used_nodes + 1 <= first_code.total_nodes)
            {
                //veo la union, si ya existe, la destruyo
                for (int i = 0; i < first_code.total_nodes; i++)
                {
                    if (first_code.objectives[i] == this.gameObject)
                    {
                        //ya existe, elimino la flecha y libero el cupo
                        first_code.used_nodes -= 1;
                        first_code.objectives[i] = null;
                        Destroy(first_code.unions[i].gameObject);
                        first_code.unions[i] = null;
                        //Debug.Log("nodo sacado" + this.gameObject);
                        first = null;
                        return;
                    }
                }
                int index_to_use = 0;
                //no existe, selecciono el espacio para agregar la union
                for (int i = 0; i < first_code.total_nodes; i++)
                {
                    // cuando encontremos uno vacio lo usaremos
                    if (first_code.objectives[i] == null)
                    {
                        first_code.objectives[i] = this.gameObject;
                        index_to_use = i;
                        first_code.used_nodes += 1;
                        break;
                    }
                }
                //finalmente, casteo la linea
                Debug.Log(first.transform.position);
                float distTotal = Vector2.Distance(first.transform.position, transform.position);
                float distX = Math.Abs(first.transform.position.x - transform.position.x);
                float distY = Math.Abs(first.transform.position.y - transform.position.y);
                float middleX;
                float middleY;
                Vector2 colliderClosest1 = collider.ClosestPoint(first.transform.position);
                Vector2 colliderClosest2 = first.GetComponent<CircleCollider2D>().ClosestPoint(transform.position);
                float colliderDist = Vector2.Distance(colliderClosest1, colliderClosest2);
                //calculos de puntos medios
                #region
                middleX = (first.transform.position.x + transform.position.x) / 2f;
                middleY = (first.transform.position.y + transform.position.y) / 2f;
                #endregion

                int angle;
                angle = (int)(Math.Atan(distY / distX) * 180 / Math.PI);
                //fixing angle depending on direction
                if (first.transform.position.x < transform.position.x && first.transform.position.y >= transform.position.y)
                {
                    angle *= -1;

                }
                else if (first.transform.position.x >= transform.position.x && first.transform.position.y >= transform.position.y)
                {
                    angle += 180;
                }
                else if (first.transform.position.x >= transform.position.x && first.transform.position.y < transform.position.y)
                {
                    angle += (90 - angle) * 2;
                }
                Debug.Log("angulo: " + angle);
                GameObject g = Instantiate(arrow, new Vector3(middleX, middleY, transform.position.z), Quaternion.identity);
                /*
                //find the vector pointing from our position to the target
                Vector3 _direction = (Target.position - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
                */
                g.transform.Rotate(0, 0, angle - 90);
                g.transform.localScale = new Vector3(0.3f, 0.15f * distTotal - 0.3f, 1); //the minus parameter avoid the arrow to enter into the circle
                first_code.unions[index_to_use] = g;
                Debug.Log("Union entre" + first + "and" + this.gameObject);
                first = null;


            }
            else
            {
                for (int i = 0; i < first_code.total_nodes; i++)
                {
                    if (first_code.objectives[i] == this.gameObject)
                    {
                        //ya existe, elimino la flecha y libero el cupo
                        first_code.used_nodes -= 1;
                        first_code.objectives[i] = null;
                        Destroy(first_code.unions[i].gameObject);
                        first_code.unions[i] = null;
                        Debug.Log("nodo sacado" + this.gameObject);
                        first = null;
                        return;
                    }
                    else
                    {
                        Debug.Log("This node can´t have more nodes");
                    }
                }
            }
        }
    }

    void ChangeColor()
    {
        #region Colores y halo
        if (owner == 0)
        {
            sprite.color = new Color(1f, 1f, 1f, 1);
        }
        else if (owner == 1)
        {
            sprite.color = new Color(0.2002492f, 9433962f, 0.5556294f, 1);
        }
        else if (owner == 2)
        {
            sprite.color = new Color(0.8867924f, 0.0f, 0.788344f, 1);
        }
        else if (owner == 3)
        {
            sprite.color = new Color(0.9677409f, 1f, 0.495283f, 1);
        }
        else if (owner == 4)
        {
            sprite.color = new Color(0f, 0f, 0f, 1);
        }
        if (first == this.gameObject)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
        }
        else
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
        }
        #endregion
    }

    private void ChangeHP()
    {
        //demostrative only of modifying the points
        textObject.GetComponent<TextMeshProUGUI>().text = points.ToString();
        counter++;
        if (objectives[0] != null && counter > 400)
        {
            objectives[0].GetComponent<Seleccion_y_Union>().points--;
            counter = 0;
        }
    }

    void Start()
    {
        CheckType();
        textObject = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        textObject.GetComponent<TextMeshProUGUI>().text = points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
        ChangeHP();
    }
}
