using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrapnelController : MonoBehaviour
{
	private float Speed;
    private float Spin;
    public float gravityAmount;
    private float fallSpeed;
    public float lifeTime;
    private float currentLifeTime;

    // Start is called before the first frame update
    void Start()
    {
		// Pick a random spin speed between 2 to 5 times a second
        Spin = Random.Range( 360 * 2 , 360 * 5 );
        currentLifeTime = lifeTime;
 
    }

    // Update is called once per frame
    void Update()
    {
        // Move shrapnel to new location
        Vector3 newPosition = new Vector3();
        newPosition = transform.position;
        newPosition.x += Speed * Time.deltaTime;
        newPosition.y -= fallSpeed * Time.deltaTime;
        transform.position = newPosition;
        fallSpeed += gravityAmount * Time.deltaTime; 
        transform.Rotate ( Vector3.forward * ( Time.deltaTime * Spin ) );

        // check if we are done
        currentLifeTime -= Time.deltaTime;
        if(currentLifeTime  < 0) {
            Destroy( gameObject );
        } else {
            // fade out over lifetime
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = currentLifeTime / lifeTime;
            GetComponent<SpriteRenderer>().color = color;
        }
    }

     // Called by helicopter to set the position and speed of shrapnel
    public void CreateShrapnel( float initSpeed , Vector3 initPos ) {
        transform.position = initPos;
        Speed = initSpeed;
        fallSpeed = Random.Range( - 1f , 0 );
    }
}
