using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{	
	public float shotAngle;
	public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3();
    	newPosition = transform.position;
    	newPosition.x += Mathf.Cos (shotAngle) * speed * Time.deltaTime;
		newPosition.y += Mathf.Sin (shotAngle) * speed * Time.deltaTime;
        transform.position = newPosition;

        // see if we are off the screen
        if( (transform.position.x < -9) ||  
        	(transform.position.x > 9) || 
        	(transform.position.y > 5) || 
        	(transform.position.y < -5))
        {
        	Destroy(gameObject);
        }   
    }

    public void FireShot( float angle , Vector3 position ) {
    	shotAngle = angle * Mathf.Deg2Rad;
    	transform.position = position;
    }
}
