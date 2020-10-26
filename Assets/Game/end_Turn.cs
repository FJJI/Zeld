using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_Turn : MonoBehaviour
{
    // Start is called before the first frame update

    void OnMouseDown()
    {
        Info controller = transform.parent.parent.gameObject.GetComponent<Info>();
        controller.turn++;
        controller.player_turn++;
        if(controller.player_turn>controller.players_number)
        {
            controller.player_turn=1;
        }
        controller.refreshNodes();
    }


    void Start()
    {
        Info controller = transform.parent.parent.gameObject.GetComponent<Info>();
        controller.turn=1;
        controller.player_turn=1;
        controller.players_number=4;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
