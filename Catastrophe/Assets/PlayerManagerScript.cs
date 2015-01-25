using UnityEngine;
using System.Collections;

public class PlayerManagerScript : MonoBehaviour 
{
    /* Constants */
    const int STARTING_HAND_SIZE = 4;
    const int MAX_HAND_SIZE = 6;
    const int MAX_SWARM_SIZE = 10;


    int currentHandSize;
    public GameObject card;    

    /* Variables for Player Mouse I/O */
    bool selectFlag;
    GameObject selectTarget;

    /* Board Positions */
    Vector3[] spawn_position;
    Vector3[] swarm_creatures;
    Vector2 playmatBounds;




	// Use this for initialization
	void Start ()
    {
        currentHandSize = 0;
        spawn_position = new Vector3[MAX_HAND_SIZE];
        swarm_creatures = new Vector3[MAX_SWARM_SIZE];

        if (!card)
        {
            Debug.Log("Attempting to instantiate card prefab!");
            //card = (GameObject)Resources.Load("../Card.prefab");
        }

        setConstants();

        while (true)
        {
            drawCard(STARTING_HAND_SIZE);

            if (currentHandSize > 0)
                break;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        handlePlayerInput();
	}


    void handlePlayerInput ()
    {
        /* 
         *  1. Viewing a card's text
         *  2. Picking up a card
         *  3. Leaving card view mode
         *  4. Select target
         *  5. Press End Phase Button
         * 
        */

        // Begin Click -- select card or pass turn
        if (Input.GetMouseButtonDown(0))
        {
            /* Attempt to Select a Card */
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                if (rayHit.transform.gameObject != null && rayHit.transform.gameObject.tag == "Card")
                {
                    selectTarget = rayHit.transform.gameObject;
                    
                    Debug.Log(string.Format("Selected Item = {0}", selectTarget));
                    
                    selectFlag = true;
                }
            }

            /* Check Button Press */

        }
        
        // Drag Click
        if (selectFlag && Input.GetMouseButton(0))
        {
            /* Update Position of Selected Card */
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

            selectTarget.transform.position = new Vector3(curPosition.x, curPosition.y, 0.0f);
        }

        // End click
        if (selectFlag && Input.GetMouseButtonUp(0))
        {

            if (selectTarget)
            {
                viewOrPlay();

                selectTarget = null;
            }

            selectFlag = false;
        }
    }

    void viewOrPlay()
    {
        if (selectTarget.tag != "Card")
            return;

        Vector3 cardDropPosition = selectTarget.transform.position;

        /* View */

        /* Play */
        if (cardDropPosition.y - (selectTarget.transform.lossyScale.y / 3.0f) >= playmatBounds.x
                && cardDropPosition.y + (selectTarget.transform.lossyScale.y / 3.0f) <= playmatBounds.y)
        {
            Debug.Log("Played a card to the Battlefield!");
        }
    }

    void setConstants()
    {
        GameObject playmat = GameObject.Find("Playmat");

        playmatBounds = new Vector2(playmat.transform.position.y - (playmat.transform.lossyScale.y / 2.0f), 
                                        playmat.transform.position.y + (playmat.transform.lossyScale.y / 2.0f));

        spawn_position[0] = GameObject.Find("p1_Card1Zone").transform.position;
        spawn_position[1] = GameObject.Find("p1_Card2Zone").transform.position;
        spawn_position[2] = GameObject.Find("p1_Card3Zone").transform.position;
        spawn_position[3] = GameObject.Find("p1_Card4Zone").transform.position;
        spawn_position[4] = GameObject.Find("p1_Card5Zone").transform.position;
        spawn_position[5] = GameObject.Find("p1_Card6Zone").transform.position;

        for (int i = 0; i < MAX_HAND_SIZE; ++i)
            spawn_position[i].z = 4.8f;
    }

    void drawCard (int x)
    {
        int newHandSize = currentHandSize + x;

        for (int i = currentHandSize; i < newHandSize; i++)
        {
            Debug.Log(string.Format("Current Hand Size = {0}", currentHandSize));
            GameObject localCard;
            
            localCard = Instantiate(card, spawn_position[i], Quaternion.identity) as GameObject;

            localCard.name = "Card" + i;
        }

        currentHandSize = newHandSize;
    }
}
