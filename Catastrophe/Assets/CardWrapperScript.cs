using UnityEngine;
using System.Collections;

public enum CardPlace { HAND, PLAY, GRAVEYARD, DECK };


public class CardWrapperScript : MonoBehaviour {
    public CardPlace place = CardPlace.DECK;
    public int placeIndex;
    public GameObject cardPlaceHolder;

	// Use this for initialization
	void Start () 
    {
        placeIndex = -1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public bool placeCard(CardPlace cp, int index, Vector3 pos, GameObject par)
    {
        placeIndex = index;
        place = cp;
        cardPlaceHolder = par;

        gameObject.transform.position = pos;
        transform.parent = cardPlaceHolder.transform;

        return true;
    }
}
