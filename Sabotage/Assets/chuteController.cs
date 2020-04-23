using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chuteController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if( CheckCollision() ) {
			// chute got hit
			Destroy(gameObject);
		} 
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
        // Check for falling troopers
        foreach( GameObject trooperObj in GameObject.FindGameObjectsWithTag("trooper")) {
        	// Make sure its not the trooper we are attached to
        	if( trooperObj.transform != transform.parent ) {
        	    SpriteRenderer trooperSR = trooperObj.GetComponent<SpriteRenderer>();
            	if( trooperSR.bounds.Intersects( mySR.bounds )) {
                	return true;
            	}	
        	}
        
        }
        return false;
    }
}
