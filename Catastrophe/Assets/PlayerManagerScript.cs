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
    Vector3[] swarm_positions;
    GameObject[] swarm_creatures;
    Vector2 playmatBounds;




	// Use this for initialization
	void Start ()
    {
        currentHandSize = 0;

        spawn_position = new Vector3[MAX_HAND_SIZE];
        swarm_positions = new Vector3[MAX_SWARM_SIZE];

        swarm_creatures = new GameObject[MAX_SWARM_SIZE];

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
                    CardWrapperScript selectedCardWrapperScript;
                    selectTarget = rayHit.transform.gameObject;

                    selectedCardWrapperScript = selectTarget.GetComponent<CardWrapperScript>();

                    if (selectedCardWrapperScript.swarmIndex == -1)
                    {
                        selectFlag = true;

                        Debug.Log(string.Format("Selected Item = {0}", selectTarget));
                    }
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
        CardWrapperScript droppedCardWrapperScript;

        if (selectTarget.tag != "Card")
            return;

        Vector3 cardDropPosition = selectTarget.transform.position;
        droppedCardWrapperScript = selectTarget.GetComponent<CardWrapperScript>();

        /* View */

        /* Play */

        // Is the dropped position between the y bounds of the battlfield
        if (cardDropPosition.y >= playmatBounds.x
                && cardDropPosition.y <= playmatBounds.y)
        {
            int closestSwarmPosition = -1;
            Debug.Log("Played a card to the Battlefield!");

            for (int i = 0; i < MAX_SWARM_SIZE; i++)
            {
                if (swarm_creatures[i] == null)
                {
                    Vector2 swarmPositionBounds = new Vector2(swarm_positions[i].x - (card.transform.lossyScale.x / 2.0f),
                                            swarm_positions[i].x + (card.transform.lossyScale.x / 2.0f));

                    if (swarmPositionBounds.x - 0.2f <= selectTarget.transform.position.x
                        && swarmPositionBounds.y + 0.2f >= selectTarget.transform.position.x)
                    {
                        closestSwarmPosition = i;
                        break;
                    }
                }
            }

            if (closestSwarmPosition >= 0)
            {
                swarm_creatures[closestSwarmPosition] = selectTarget;
                selectTarget.transform.position = swarm_positions[closestSwarmPosition];

                droppedCardWrapperScript.swarmIndex = closestSwarmPosition;
            }
        }
        else
        {
            string respawn_location = "p1_Hand_" + selectTarget.name;

            Debug.Log(string.Format("Name of respawn location game object = {0}", respawn_location));

            Vector3 newCardLocation = GameObject.Find(respawn_location).transform.position;

            selectTarget.transform.position = new Vector3(newCardLocation.x, newCardLocation.y, 4.8f);

            if (selectTarget.GetComponent<CardWrapperScript>())
            {
                if (droppedCardWrapperScript.swarmIndex >= 0)
                {
                    droppedCardWrapperScript.swarmIndex = -1;
                    swarm_creatures[droppedCardWrapperScript.swarmIndex] = null;
                }
            }
        }
    }

    void setConstants()
    {
        GameObject playmat = GameObject.Find("p1_Battlefield");

        playmatBounds = new Vector2(playmat.transform.position.y - (playmat.transform.lossyScale.y / 2.0f), 
                                        playmat.transform.position.y + (playmat.transform.lossyScale.y / 2.0f));

        spawn_position[0] = GameObject.Find("p1_Hand_Card_0").transform.position;
        spawn_position[1] = GameObject.Find("p1_Hand_Card_1").transform.position;
        spawn_position[2] = GameObject.Find("p1_Hand_Card_2").transform.position;
        spawn_position[3] = GameObject.Find("p1_Hand_Card_3").transform.position;
        spawn_position[4] = GameObject.Find("p1_Hand_Card_4").transform.position;
        spawn_position[5] = GameObject.Find("p1_Hand_Card_5").transform.position;

        swarm_positions[0] = GameObject.Find("p1_Battlefield_Card_0").transform.position;
        swarm_positions[1] = GameObject.Find("p1_Battlefield_Card_1").transform.position;
        swarm_positions[2] = GameObject.Find("p1_Battlefield_Card_2").transform.position;
        swarm_positions[3] = GameObject.Find("p1_Battlefield_Card_3").transform.position;
        swarm_positions[4] = GameObject.Find("p1_Battlefield_Card_4").transform.position;
        swarm_positions[5] = GameObject.Find("p1_Battlefield_Card_5").transform.position;
        swarm_positions[6] = GameObject.Find("p1_Battlefield_Card_6").transform.position;
        swarm_positions[7] = GameObject.Find("p1_Battlefield_Card_7").transform.position;
        swarm_positions[8] = GameObject.Find("p1_Battlefield_Card_8").transform.position;
        swarm_positions[9] = GameObject.Find("p1_Battlefield_Card_9").transform.position;

        for (int i = 0; i < MAX_HAND_SIZE; ++i)
            spawn_position[i].z = 4.8f;

        for (int i = 0; i < MAX_SWARM_SIZE; ++i)
        {
            swarm_positions[i].z = 4.8f;
            swarm_creatures[i] = null;
        }
    }

    void drawCard (int x)
    {
        int newHandSize = currentHandSize + x;

        for (int i = currentHandSize; i < newHandSize; i++)
        {
            Debug.Log(string.Format("Current Hand Size = {0}", currentHandSize));
            GameObject localCard;
            
            localCard = Instantiate(card, spawn_position[i], Quaternion.identity) as GameObject;

            localCard.name = "Card_" + i;
        }

        currentHandSize = newHandSize;
    }
}
