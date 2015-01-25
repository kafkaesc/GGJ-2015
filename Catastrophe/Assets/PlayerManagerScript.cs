using UnityEngine;
using System.Collections;

public class PlayerManagerScript : MonoBehaviour 
{
    const int MAX_HAND_SIZE = 6;
    const int STARTING_HAND_SIZE = 4;

    int currentHandSize;

    public GameObject card;
    public bool moveRequiredFlag;

    bool draggingCardFlag;
    bool environmentActive;

    Vector3[] spawn_position;

	// Use this for initialization
	void Start ()
    {
        moveRequiredFlag = false;
        draggingCardFlag = false;
        environmentActive = false;
        currentHandSize = 0;
        spawn_position = new Vector3[MAX_HAND_SIZE];

        if (!card)
        {
            Debug.Log("Attempting to instantiate card prefab!");
            //card = (GameObject)Resources.Load("../Card.prefab");
        }

        setSpawnPositions();

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
        // Begin Click
        /* 
         *  1. Viewing a card's text
         *  2. Picking up a card
         *  3. Leaving card view mode
         *  4. Select target
         *  5. Press End Phase Button
         * 
        */
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed left click.");

            if (moveRequiredFlag)
            {
                draggingCardFlag = true;
            }

        }

        // Translate Object
        if (Input.GetMouseButton(0) && draggingCardFlag)
        {

        }

        // End click
        if (Input.GetMouseButtonUp(0))
        {
            // Raycast to card in order to STOP click; if board found then valid click
            Debug.Log("Pressed left click.");
        }

        if (currentHandSize < MAX_HAND_SIZE && Input.GetMouseButtonUp(1))
        {
            Debug.Log("DRAW!");

            drawCard(1);
        }
    }

    public bool isEnvironmentActive()
    {
        return environmentActive;
    }

    void setSpawnPositions()
    {
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

            Instantiate(card, spawn_position[i], Quaternion.identity);
        }

        currentHandSize = newHandSize;
    }
}
