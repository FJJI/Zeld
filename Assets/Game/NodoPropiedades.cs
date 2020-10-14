using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoPropiedades : MonoBehaviour
{

    public int activeLinks;
    public int power;
    public int hability; // 0 if normal, 1 to 3 to the powers
    public int owner; // 0 if neutral, else to playerN

    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    void ChangeColor()
    {
        if (owner == 0)
        {
            sprite.color = new Color(0.50f, 0.50f, 0.50f, 1);
        }
        else if (owner == 1)
        {
            sprite.color = new Color(0.10f, 0.67f, 0.14f, 1);
        }
        else if (owner == 2)
        {
            sprite.color = new Color(0.52f, 0.14f, 0.67f, 1);
        }
    }
}
