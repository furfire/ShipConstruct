using UnityEngine;
using System.Collections;

public class AddForce : MonoBehaviour {
	Rigidbody2D rigidBody;
	// Use this for initialization
	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		//force amount = 200;
		//calculate facing direction
		if(rigidBody)

		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(rigidBody == null)
			{
				rigidBody = gameObject.GetComponent<Rigidbody2D>();
			}
			rigidBody.AddForce(200 * transform.up);
		}
	}
}
