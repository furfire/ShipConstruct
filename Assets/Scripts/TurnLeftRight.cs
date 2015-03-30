using UnityEngine;
using System.Collections;

public class TurnLeftRight : MonoBehaviour {
	Rigidbody2D rigidBody;
	// Use this for initialization
	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			rigidBody.AddForce(new Vector2(-200,0));
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			rigidBody.AddForce(new Vector2(200,0));
		}
	}
}
