using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonController : MonoBehaviour
{
	public float maxAngle;
	private float gunAngle;
    private bool fireFlag;
    public  GameObject bulletObj;
    public float fireDelay;
    private float fireTimer;
    // Need ATCObj to access score
    public GameObject ATCObj;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // check for end game condition
        if( ATCObj.GetComponent<AirTrafficController>().gameEnd ) {
            // disable cannon and move off screen
            Vector3 newPosition;
            newPosition = transform.position;
            if( newPosition.y > -4.5f ) {
                newPosition.y -= .4f * Time.deltaTime;
            }
            transform.position = newPosition;
        } else {
        	// Sent angle of cannon
            Vector3 mousePos = Input.mousePosition;
            float mouseSwing = (mousePos.x * 2) / Screen.width - 1;
            gunAngle = mouseSwing * -maxAngle;
            transform.rotation = Quaternion.AngleAxis( gunAngle , Vector3.forward);
            
            // Check for fire button
            fireFlag = Input.GetButton ("Fire1");
            fireTimer -= Time.deltaTime;
            if( fireFlag && (fireTimer < 0)) {
                ShootBullet();
                fireTimer = fireDelay;
            }
        }

    }

    void ShootBullet(){
        GameObject newBullet = Instantiate( bulletObj );
        newBullet.GetComponent<bulletController>().FireShot( gunAngle + 90 , transform.position );
        ATCObj.GetComponent<AirTrafficController>().AddScore(-1);
    }
}
