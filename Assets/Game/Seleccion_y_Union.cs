using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccion_y_Union : MonoBehaviour
{
    public int total_nodes = 2;
    public int used_nodes = 0;
    public static GameObject first;
    public int type;
    public CircleCollider2D collider;
    // Tipo 0 es normal, 1 es warrior, 2 es defensa, 3 es nodo extra
    public List<GameObject> objectives;
    public List<GameObject> unions;
    public GameObject arrow;
    void CheckType()
    {
        if (type == 3) // aumentamos la cantidad de nodos usables
        {
            total_nodes += 1;
        }
        // Para crear los objetivos de cada uno
        for(int i = 0; i<total_nodes; i++)
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
                        Destroy(first_code.unions[i]);
                        first_code.unions[i] = null;
                        Debug.Log("nodo sacado" + this.gameObject);
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
                        Debug.Log("AAA");
                        first_code.used_nodes += 1;
                        break;
                    }
                }
                //finalmente, casteo la linea
                Debug.Log(first.transform.position);
                float distX = Math.Abs(first.transform.position.x-transform.position.x);
                float distY = Math.Abs(first.transform.position.y-transform.position.y);
                float middleX;
                float middleY;
                //calculos de puntos medios
                #region
                middleX = (first.transform.position.x + transform.position.x) / 2f;
                middleY = (first.transform.position.y + transform.position.y) / 2f;
                #endregion

                int angle;
                angle =(int)(Math.Atan(distY/distX)*180/Math.PI);
                //fixing angle depending on direction
                if(first.transform.position.x<transform.position.x && first.transform.position.y>=transform.position.y)
                {
                    angle*=-1;
                    
                }
                else if(first.transform.position.x>=transform.position.x && first.transform.position.y>=transform.position.y)
                {
                    angle+=180;
                }
                else if(first.transform.position.x>=transform.position.x && first.transform.position.y<transform.position.y)
                {
                    angle+=(90-angle)*2;
                }
                Debug.Log("angulo: " + angle);
                GameObject g = Instantiate(arrow, new Vector3(middleX,middleY, transform.position.z), Quaternion.identity);
                /*
                //find the vector pointing from our position to the target
                Vector3 _direction = (Target.position - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
                */
                g.transform.Rotate(0, 0, angle-90);
                Debug.Log("Union entre"+ first+ "and"+ this.gameObject);
                first = null;

            }
            else
            {
                Debug.Log("This node can´t have more nodes");
            }
        }
    }

    Vector2 point(GameObject game)
    {
        Vector2 vect = collider.ClosestPoint(game.transform.position);
        return vect;
    }

    void Start()
    {
        CheckType();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
