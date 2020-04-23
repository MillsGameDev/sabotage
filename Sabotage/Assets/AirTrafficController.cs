using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTrafficController : MonoBehaviour
{
	public GameObject heliObj;
   	public float launchDelayMin;
   	public float launchDelayMax;
	private float launchTimer;

	// Used to manage helicopter traffic
	public bool[] levelDirection;
	public int[] levelNum;
	public int maxOccupancy;

	// Score Board
    public GameObject scoreObj;
    private int score;

    // Game End Flag
    public bool gameEnd;

    // Start is called before the first frame update
    void Start()
    {
       	// Setup the empty levels
      	levelDirection = new bool[4];
        levelNum = new int[4];
        levelNum[0] = 0;
        levelNum[1] = 0;
        levelNum[2] = 0;
        levelNum[3] = 0;  

        // start with zero points
        score = 0;

        // We just started a game
        gameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
    	// After short delay spawn a helicopter
        launchTimer -= Time.deltaTime;
        if( launchTimer < 0 ) {
        	CheckForGameEnd();
        	if( !gameEnd ) {
        		Instantiate( heliObj );	
        	}
			launchTimer = Random.Range( launchDelayMin , launchDelayMax );
        }
    }

    // Find an available level for the helicopter
	public int GetAvailableLevel( bool forwardFlag ) {
		int i = 0;
		while( i < 4) {
			// If level is empty, claim it for this helicopter
			if( levelNum[i] == 0 ) {
				levelDirection[i] = forwardFlag;
				levelNum[i]++;
				return i;
			} else {
				// If the level is the same direction and not already crowded,
				// add helicopter to it
				if( ( levelDirection[i] == forwardFlag ) && (levelNum[i] < maxOccupancy )) {
					levelNum[i]++;
					return i;
				}
			}
			// lets try the next level
			i = i + 1;
		}
    	return -1; // no levels available
    }

    //  Helicopter left the level
    public void LeftLevel( int level ) {
    	levelNum[level]--;
    }

    // Add points to the score and update the display
    public void AddScore( int n ) {
    	// yes they can add negative score too
    	score += n;
    	if( score < 0 ) score = 0;
    	scoreObj.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
    }

    // Check for the end game condition
    void CheckForGameEnd() {
    	int leftCount = 0;
    	int rightCount = 0;

    	foreach( GameObject troopObj in GameObject.FindGameObjectsWithTag("trooper") ) {
    		if( troopObj.GetComponent<trooperController>().onGround ) {
    			if( troopObj.transform.position.x < 0 ) { 
    				leftCount++;
    			} else {
    				rightCount++;
    			}
    		}
    	}
    	if(( leftCount > 3 ) || ( rightCount > 3 )) {
    		gameEnd = true;
    		// tell troopers to destroy building
    		foreach( GameObject troopObj in GameObject.FindGameObjectsWithTag("trooper") ) {
    			if( troopObj.GetComponent<trooperController>().onGround ) {
    				if( troopObj.transform.position.x < 0 ) {
    					if( leftCount > 3 ) { troopObj.GetComponent<trooperController>().destroyBuilding = true; }
    				} else {
    					if( rightCount > 3 ) { troopObj.GetComponent<trooperController>().destroyBuilding = true; }
    				}
    			}
    		}
    	}


    }
}
