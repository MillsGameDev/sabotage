using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trooperController : MonoBehaviour
{
	public float gravity;
    public float fallSpeed;
    public float groundPos;
    public bool onGround;
    // Parachute Objects
    public GameObject chuteObj;
	private GameObject currentChute;
	private bool usedChute;
    // Bloody Bits
    public GameObject expObj;
    // ATC Object so we can update score
    public GameObject ATCObj;
    public bool destroyBuilding;
    // Fireworks object to destroy building
    public GameObject fireworkObj;
    // Sound Effects
    public GameObject hitSound;
    public GameObject squishSound;
    public GameObject screamSound; 


    // Start is called before the first frame update
    void Start()
    {
        // Locate the Air Traffic Controller
        ATCObj = GameObject.FindWithTag("ATC");
    	onGround = false;
    	usedChute = false;
        destroyBuilding = false;
    }

    // Update is called once per frame
    void Update()
    {
		if( !onGround ) {
            Vector3 newPosition;
            newPosition = transform.position;
            newPosition.y -= fallSpeed * Time.deltaTime;
            if( currentChute == null ) {
		    		fallSpeed += gravity * Time.deltaTime;	
		    }
            // see if we landed on the ground
            if( newPosition.y < groundPos ) {
                newPosition.y = groundPos;
                onGround = true;
                if( currentChute != null ) { Destroy( currentChute ); }
                // See if we were going too fast
                if( fallSpeed > 2f ) {
                    // we squished
                    ATCObj.GetComponent<AirTrafficController>().AddScore( 2 );
                    GameObject explode = Instantiate( expObj);
                    explode.transform.position = transform.position;
                    // Add squish sound
                    GameObject squish;
                    squish = Instantiate( squishSound );
                    squish.GetComponent<AudioSource>().panStereo = transform.position.x / 9f;
                    Destroy(gameObject);
                }
            }
            transform.position = newPosition;
            // If we are going to fast lets use the chute if we got one
            if( (!usedChute ) && ( fallSpeed > 3f )) {
		    	SpawnChute();
		    	fallSpeed = 1f;
		    	usedChute = true;
		    } else {
                if( usedChute && ( fallSpeed > 3f ) && !GetComponent<AudioSource>().isPlaying ) {
                    GetComponent<AudioSource>().Play();
                }
            }

            // See if we hit a bullet
            if( CheckCollision() ) {
                // we got hit
                ATCObj.GetComponent<AirTrafficController>().AddScore( 2 );
                GameObject explode = Instantiate( expObj);
                explode.transform.position = transform.position;
                // Add hit sound
                GameObject hit;
                hit = Instantiate( hitSound );
                hit.GetComponent<AudioSource>().panStereo = transform.position.x / 9f;
                Destroy(gameObject);
            }
        } else {
            // When destroying building run to center of screen
            if( destroyBuilding ) {
                Vector3 newPosition;
                newPosition = transform.position;
                if( newPosition.x < 0 ) {
                    newPosition.x += 1f * Time.deltaTime;
                    if( newPosition.x > 0 ) { 
                        Instantiate( fireworkObj );
                        Destroy(gameObject); 
                    }
                }else {
                    newPosition.x -= 1f * Time.deltaTime;
                    if( newPosition.x < 0 ) { 
                        Instantiate( fireworkObj );
                        Destroy(gameObject); 
                    }
                }
                transform.position = newPosition;
            } else {
                // On the ground, watch out for falling troopers
                if( CheckForFallingTroops() ) {
                    // we got hit
                    ATCObj.GetComponent<AirTrafficController>().AddScore( 2 );
                    GameObject explode = Instantiate( expObj);
                    explode.transform.position = transform.position;
                    // Add scream sound
                    GameObject scream;
                    scream = Instantiate( screamSound );
                    scream.GetComponent<AudioSource>().panStereo = transform.position.x / 9f + 0.5f;
                    Destroy(gameObject);
                }
            }

            
        }
    }

    // Create the parachute child game object
     void SpawnChute() {
    	currentChute = Instantiate( chuteObj );
    	currentChute.transform.position = transform.position;
    	currentChute.transform.parent = gameObject.transform;
    }

    // See if we hit any bullets
    bool CheckCollision() {
        SpriteRenderer mySR;
        mySR = gameObject.GetComponent<SpriteRenderer>();
        foreach( GameObject bullObj in GameObject.FindGameObjectsWithTag("bullet")) {
            SpriteRenderer bullSR = bullObj.GetComponent<SpriteRenderer>();
            if( bullSR.bounds.Intersects( mySR.bounds )) {
                // collides
                // destroy bullet
                Destroy(bullObj);
                return true;
            }
        }
        return false;
    }

    // See if a trooper fell on us
    bool CheckForFallingTroops() {
        SpriteRenderer mySR;
        mySR = gameObject.GetComponent<SpriteRenderer>();
        foreach( GameObject trooperObj in GameObject.FindGameObjectsWithTag("trooper")) {
            // Make sure its not ourselves and they are falling fast
            if( (trooperObj.transform != transform) &&
                (trooperObj.GetComponent<trooperController>().fallSpeed > 2f) ) {
                SpriteRenderer trooperSR = trooperObj.GetComponent<SpriteRenderer>();
                if( trooperSR.bounds.Intersects( mySR.bounds )) {
                    return true;
                } 
            }
        }
        return false;
    }

}
