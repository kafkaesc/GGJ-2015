using UnityEngine;
using System.Collections;

public class PlayerManagerScript : MonoBehaviour 
{
    /* Constants */
    const int STARTING_HAND_SIZE = 4;
    const int MAX_HAND_SIZE = 6;
    const int MAX_SWARM_SIZE = 10;
    const float CARD_DEPTH_Z = 3.0f;


    int currentHandSize;
    public GameObject card;    

    /* Variables for Player Mouse I/O */
    bool clickFlag;
    GameObject clickTarget;

    /* Board Positions */
    Vector3[] spawn_position;
    Vector3[] swarm_positions;
    GameObject[] swarm_creatures;
    //Vector2 playmatBounds;


    void setConstants()
    {
        //GameObject playmat = GameObject.Find("p1_Battlefield");

        //playmatBounds = new Vector2(playmat.transform.position.y - (playmat.transform.lossyScale.y / 2.0f),
          //                              playmat.transform.position.y + (playmat.transform.lossyScale.y / 2.0f));

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
            spawn_position[i].z = CARD_DEPTH_Z;

        for (int i = 0; i < MAX_SWARM_SIZE; ++i)
        {
            swarm_positions[i].z = CARD_DEPTH_Z;
            swarm_creatures[i] = null;
        }
    }

	// Use this for initialization
	void Start ()
    {
        currentHandSize = 0;

        spawn_position = new Vector3[MAX_HAND_SIZE];
        swarm_positions = new Vector3[MAX_SWARM_SIZE];

        swarm_creatures = new GameObject[MAX_SWARM_SIZE];

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
   
        // Left Mouse Button Clicked
        if (Input.GetMouseButtonDown(0))
        {
            /* Raycast to see what GameObject was clicked */
            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                if (rayHit.transform.gameObject != null)
                {
                    // Clicked a Card
                    if (rayHit.transform.gameObject.tag == "Card")
                    {
                        clickTarget = rayHit.transform.gameObject;

                        clickFlag = true;
                    }
                }
            }
        }

        // Left Mouse Button Held
        if (clickFlag && Input.GetMouseButton(0))
        {
            /* Update Position of Selected Card */
            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                if (rayHit.collider != null)
                {
                    clickTarget.transform.position = rayHit.point;
                }
            }
        }

        // End click
        if (clickFlag && Input.GetMouseButtonUp(0))
        {
            /* Raycast to see what GameObject was clicked */
            if (Physics.Raycast(new Ray(clickTarget.transform.position, new Vector3(0.0f, 0.0f, 1.0f)) , out rayHit, Mathf.Infinity))
            {
                if (rayHit.transform.gameObject != null)
                {
                    // Placed Card on Swarm position
                    if (rayHit.transform.gameObject.tag == "Swarm" && rayHit.transform.gameObject.GetComponent<SwarmLocationScript>().creature == null)
                    {
                        // CURRENT: NEVER REACHING HERE

                        Debug.Log(string.Format("Placed card at swarm position: {0}", rayHit.transform.gameObject.name));

                        Vector3 placePosition = rayHit.transform.gameObject.transform.position;
                        placePosition = new Vector3(placePosition.x, placePosition.y, CARD_DEPTH_Z);
                        
                        rayHit.transform.gameObject.GetComponent<SwarmLocationScript>().creature = clickTarget;

                        clickTarget.GetComponent<CardWrapperScript>().placeCard(
                                                CardPlace.PLAY, 
                                                rayHit.transform.gameObject.GetComponent<SwarmLocationScript>().index,
                                                placePosition, rayHit.transform.gameObject);
                    }

                    Debug.Log(string.Format("Game Object Hit on End Click: {0}", rayHit.transform.gameObject.name));

                }
            }

            clickTarget = null;
            clickFlag = false;
        }
    }


    void drawCard (int x)
    {
        int newHandSize = currentHandSize + x;

        for (int i = currentHandSize; i < newHandSize; i++)
        {
            GameObject localCard;
            
            localCard = Instantiate(card, spawn_position[i], Quaternion.identity) as GameObject;

            localCard.name = "Card_" + i;
        }

        currentHandSize = newHandSize;
    }
}
