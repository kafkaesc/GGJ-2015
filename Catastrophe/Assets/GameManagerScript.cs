using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour 
{
    public TextManagerScript textManager;

    private PlayerManagerScript Player1Manager;
    private PlayerManagerScript Player2Manager;

	// Use this for initialization
	void Start () 
    {
        if (!Player1Manager)
            Player1Manager = GameObject.Find("p1_Side").GetComponent<PlayerManagerScript>();

        if (!Player2Manager)
            Player2Manager = GameObject.Find("p2_Side").GetComponent<PlayerManagerScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // Phase 1: Resolve Environment Effects - Players choose effects of environments            -- Each player choose targets for their environment; if that player has one
        
        // Phase 2: Recruit Phase - Players choose to use Recruitment points for Recruiting         -- Each player given a minute to summon more creatures
        
        // Phase 3: Happenings - Players choose to use Recruitment points for Happenings            -- Players given 45 seconds to play any spells
        
        // Phase 4: Battle Phase - Players choose to Attack or Defend during this battle            -- Both players given 10 seconds to choose ATTACK or DEFEND; Then declare targets
        
        // Phase 5: Cleanup Phase - Resolve any "Till end of turn" effects.                         -- No player I/O
        	
    }

}
