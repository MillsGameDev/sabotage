using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heliController : MonoBehaviour
{
	public float speed;
	private bool forwardDirection;
	private int flightLevel;
    public GameObject ATCObj;
    public GameObject shrapnelObj;
    public GameObject boomObj;
    // Trooper Information
    public GameObject trooperObj;
    private bool payloadFlag;
    private float payloadPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Locate the Air Traffic Controller
        ATCObj = GameObject.FindWithTag("ATC");

        Vector3 newPosition = new Vector3();
    	newPosition = transform.position;
    	// Pick a direction
        if( Random.value < 0.5 ) { 
        	forwardDirection = true; 
        	newPosition.x = -9;
        	// need to flip sprite
        	 GetComponent<SpriteRenderer>().flipX = true;
        } else { 
        	forwardDirection = false;
        	newPosition.x = 9;
        	speed = -speed;
        }
        // Pick a level
        flightLevel = ATCObj.GetComponent<AirTrafficController>().GetAvailableLevel( forwardDirection );
        if( flightLevel == -1 ) { Destroy( gameObject ); } // no level available. Cancel flight
        newPosition.y = 4.5f - flightLevel * 0.8f;
        transform.position = newPosition;
        // Setup our Trooper
        payloadFlag = true;
        payloadPosition = Random.Range( -8f , 8f ); // if you want floats, pass in floats
        // Avoid the building
        if( payloadPosition > 0 ) { payloadPosition = Mathf.Max( 0.8f , payloadPosition ); }
        if( payloadPosition <= 0 ) { payloadPosition = Mathf.Min( -0.8f , payloadPosition ); }

    }

    // Update is called once per frame
    void Update()
    {
		Vector3 newPosition = new Vector3();
    	newPosition = transform.position;
        newPosition.x += speed * Time.deltaTime;
        transform.position = newPosition;
        // Pan the audio to our position
        GetComponent<AudioSource>().panStereo = newPosition.x / 9f;

         // see if we are off screen
        if( (newPosition.x < -9) || (newPosition.x > 9)) {
            ATCObj.GetComponent<AirTrafficController>().LeftLevel(flightLevel);
        	Destroy(gameObject);
        }

        // Is it time to dump our payload?
        if( payloadFlag == true ) {
            if( forwardDirection && ( newPosition.x > payloadPosition )  ||
                !forwardDirection && ( newPosition.x < payloadPosition) ) {
                // dump the trooper
                GameObject trooper = Instantiate( trooperObj );
                trooper.transform.position = transform.position;
                payloadFlag = false;
            } 
        }


		// see if we collided with bullet
        if( CheckCollision() ) {
            ATCObj.GetComponent<AirTrafficController>().LeftLevel(flightLevel);
            ATCObj.GetComponent<AirTrafficController>().AddScore( 5 );
            shrapnelSpawn();
        	Destroy(gameObject);
        }
    }

    // See if we intersect any bullet objects
     bool CheckCollision() {
    	// see if we hit any bullets
    	SpriteRenderer mySR;
    	mySR = gameObject.GetComponent<SpriteRenderer>();
    	foreach( GameObject bullObj in GameObject.FindGameObjectsWithTag("bullet")) {
    		SpriteRenderer bullSR = bullObj.GetComponent<SpriteRenderer>();
    		if( bullSR.bounds.Intersects( mySR.bounds )) {
    			// We collided with bullet. Destroy bullet
    			Destroy(bullObj);
    			return true;
    		}
    	}
    	return false;
    }

    // Create Shrapnel when helicopter blows up
    void shrapnelSpawn() {
        Vector3 newPosition;
        newPosition = transform.position;
        GameObject newShrapnel;
        // Shrapnel 1
        newShrapnel = Instantiate( shrapnelObj  );
        newShrapnel.GetComponent<shrapnelController>().CreateShrapnel( Random.Range( speed * 0.8f , speed ) ,
             newPosition );
        // Shrapnet 2
        newPosition.x = transform.position.x - 0.2f;
        newShrapnel = Instantiate( shrapnelObj  );
        newShrapnel.GetComponent<shrapnelController>().CreateShrapnel( Random.Range( speed * 0.8f , speed ), 
            newPosition );
        // Shrapnel 3
        newPosition.x = transform.position.x + 0.2f;
        newShrapnel = Instantiate( shrapnelObj  );
        newShrapnel.GetComponent<shrapnelController>().CreateShrapnel( Random.Range( speed * 0.8f , speed ),
            newPosition );
        // Add the boom sound
        GameObject boom;
        boom = Instantiate( boomObj);
        boom.GetComponent<AudioSource>().panStereo = newPosition.x / 9f;
        boom.GetComponent<AudioSource>().pitch = Random.Range( 0.8f , 1.2f );
    }
}
