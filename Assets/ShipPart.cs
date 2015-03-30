using UnityEngine;
using System.Collections;

public class ShipPart : MonoBehaviour {
	//Idea will be to have things like generators/components extending ShipPart

	//things like mass/density are on the rigidbody
	//texture/color is on the sprite renderer
	public string Name;

	//use this in conjunction with oddly shaped parts, like triangle bits
	public bool canConnectLeft = true;
	public bool canConnectRight = true;
	public bool canConnectUp = true;
	public bool canConnectDown = true;

	/*
	public GameObject leftCube {get; set;}
	public GameObject rightCube {get; set; }
	public GameObject upCube {get; set; }
	public GameObject downCube {get; set; }

	//whether or not it holds the joint is determined by if these are null or not
	public HingeJoint2D leftJoint {get; set; }
	public HingeJoint2D rightJoint {get; set; }
	public HingeJoint2D upJoint {get; set; }
	public HingeJoint2D downJoint {get; set; }
	*/

	public ArrayList joints {get; set;}

	//will need to build in support for components greater than size one here...

	public int maxHP;
	public int currentHP;

	public int Armor;//reduces dmg to HP from collisions

	public int maxSafeHeat;
	public int currentHeat;
	public int passiveCooling;
	public int passiveHeatAtOptimumPower;

	public int moneyCost;

	//power related things here-
	public bool hasPowerConnection;
	public int optimumPower;
	public int maxPower;//max power draw capable
	public int minPower;//min power needed to barely function

	public enum TypeOfCube
	{
		Door,
		Hull,
		Engine,
		WarpEngine,
		Floor,
		Weapon
		//etc, maybe make this more general
	}

	//Radar/stealth things?  not feeling it...

	private Rigidbody2D rigBody;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		rigBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	/*
	void connectObjectsWithJoint(GameObject adjacentCube)
	{
		//xpos and yPos represent position in relation to this cube
		// x = -1 means adj to left.  x = 1 means adj to right
		xPos = adjacentCube.transform.position.x - transform.position.x;
		yPos = adjacentCube.transform.position.y - transform.position.y;

		float halfOfCubeSize = 0.5f;

		HingeJoint2D joint = gameObject.AddComponent(typeof(HingeJoint2D)) as HingeJoint2D;
		joint.connectedBody = adjacentCube.GetComponent<Rigidbody2D>();
		joint.useLimits = true;
		JointAngleLimits2D limits = new JointAngleLimits2D(){max = 0f;}
		joint.limits = limits;
		
		joint.anchor = transform.InverseTransformPoint(transform.position + new Vector3(halfOfCubeSize * xPos, halfOfCubeSize * yPos));
		joint.connectedAnchor = adjacentCube.transform.InverseTransformPoint(adjacentCube.transform.position + new Vector3(halfOfCubeSize * -xPos, halfOfCubeSize * -yPos));
	}*/

	void connectObjectsWithJoint(GameObject adjacentCube)
	{
		HingeJoint2D joint = gameObject.AddComponent(typeof(HingeJoint2D)) as HingeJoint2D;
		joint.connectedBody = adjacentCube.GetComponent<Rigidbody2D>();
		joint.useLimits = true;
		JointAngleLimits2D limits = new JointAngleLimits2D(){max = 0f};
		joint.limits = limits;

		// x == -1 means adj to left.  x == 1 means adj to right
		Vector3 relativePosition = adjacentCube.transform.position - transform.position;
		float halfOfCubeSize = 0.5f;
		joint.anchor = transform.InverseTransformPoint(transform.position + (relativePosition * halfOfCubeSize) );
		joint.connectedAnchor = adjacentCube.transform.InverseTransformPoint(adjacentCube.transform.position - (relativePosition * halfOfCubeSize));

		joints.Add(joint);
		/*
		if(relativePosition == Vector3.right)
		{
			rightCube = adjacentCube;
		}
		else if(relativePosition == -Vector3.right)
		{
			leftCube = adjacentCube;
		}
		else if(relativePosition == Vector3.up)
		{
			upCube = adjacentCube;
		}
		else if(relativePosition == -Vector3.up)
		{
			downCube = adjacentCube;
		}*/
	}

	void destroyAllJointsStoredHere()
	{
		foreach(HingeJoint2D joint in joints)
		{
			Destroy(joint);
		}
		joints.Clear();
	}

	//returns true if it found the joint to the rigidBody
	bool destroyJointToCubeStoredHere(Rigidbody2D adjacentRigidBody)
	{
		foreach(HingeJoint2D joint in joints)
		{
			if(joint.connectedBody == adjacentRigidBody)
			{
				joints.Remove(joint);
				Destroy(joint);
				return true;
			}
		}
		return false;
	}

	//returns true if it found the joint to the rigidBody
	bool destroyJointToCubeStoredHere(GameObject adjacentCube)
	{
		foreach(HingeJoint2D joint in joints)
		{
			if(joint.connectedBody == adjacentCube.GetComponent<Rigidbody2D>())
			{
				joints.Remove(joint);
				Destroy(joint);
				return true;
			}
		}

		return false;
	}
}
