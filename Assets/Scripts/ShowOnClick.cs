using UnityEngine;
using System.Collections;

public class ShowOnClick : MonoBehaviour {

	GameObject builderLogicHolder;
	BuilderLogic builderLogic;

	public SpriteRenderer spriteRenderer;
	public bool isOccupied = false;
	public GameObject occupiedBy;
	public GameObject[,] cubeArray = null;
	public int posXInArray = 0;
	public int posYInArray = 0;
	
	// Use this for initialization
	void Start () {
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		builderLogicHolder = GameObject.FindGameObjectWithTag ("builderLogicHolder");
		builderLogic = builderLogicHolder.GetComponent<BuilderLogic>();
	}
	
	// Update is called once per frame

	void connectObjectsWithJoint(int xPos, int yPos, JointAngleLimits2D limits)
	{
		//xpos and yPos represent position in array of cubes
		float halfOfCubeSize = 0.5f;

		GameObject adjacentCube = cubeArray[posXInArray + xPos, posYInArray + yPos];
		ShowOnClick adjacentOnClick = adjacentCube.GetComponent<ShowOnClick>();
		if(adjacentOnClick.isOccupied)
		{
			HingeJoint2D joint = gameObject.AddComponent(typeof(HingeJoint2D)) as HingeJoint2D;
			joint.connectedBody = adjacentCube.GetComponent<Rigidbody2D>();
			joint.useLimits = true;
			joint.limits = limits;

			joint.anchor = transform.InverseTransformPoint(transform.position + new Vector3(halfOfCubeSize * xPos, halfOfCubeSize * yPos));
			joint.connectedAnchor = adjacentCube.transform.InverseTransformPoint(adjacentCube.transform.position + new Vector3(halfOfCubeSize * -xPos, halfOfCubeSize * -yPos));
		}
	}

	void OnMouseOver () {
		if(builderLogic.hasSelectedABlock)
		{
			if (Input.GetMouseButton(0) && !isOccupied) {
				occupiedBy = builderLogic.blockToAdd;
				spriteRenderer.sprite = builderLogic.blockSpriteRenderer.sprite;
				isOccupied = true;
				Color blocksColor = builderLogic.blockSpriteRenderer.color;
				Color tempColor = new Color(blocksColor.r, blocksColor.g, blocksColor.b, blocksColor.a);
				//tempColor.a = 1f;
				spriteRenderer.color = tempColor;
				builderLogic.isHovering = false;

				//add joints to adjacent cubes
				//will need to put checks in place to somehow determine if cubes are jointable at this angle
				JointAngleLimits2D limits = new JointAngleLimits2D();
				limits.max = 0f;
				if(posXInArray > 0)//left
				{
					connectObjectsWithJoint(-1, 0, limits);
				}
				if(posXInArray < 29)//right
				{
					connectObjectsWithJoint(1, 0, limits);
				}
				if(posYInArray > 0)//down
				{
					connectObjectsWithJoint(0, -1, limits);
				}
				if(posYInArray < 29)//up
				{
					connectObjectsWithJoint(0, 1, limits);
				}
			}
			else if (Input.GetMouseButton(1) && isOccupied) {
				occupiedBy = null;
				isOccupied = false;
				Color tempColor = spriteRenderer.color;
				tempColor.a = 0f;
				spriteRenderer.color = tempColor;
				builderLogic.isHovering = true;
			}
		}
	}

	void  OnMouseEnter()
	{
		if(builderLogic.hasSelectedABlock)
		{
			if(isOccupied)
			{
				//do nothing
			}
			else
			{
				builderLogic.isHovering = true;
				builderLogic.hoveringOver = gameObject;
				Color blocksColor = builderLogic.blockSpriteRenderer.color;
				spriteRenderer.color = new Color(blocksColor.r, blocksColor.g, blocksColor.b, 0.5f);
				spriteRenderer.sprite = builderLogic.blockSpriteRenderer.sprite;
				transform.rotation = Quaternion.Euler(builderLogic.facingRotation);
			}
		}
	}

	void OnMouseExit()
	{
		builderLogic.isHovering = false;
		if(!isOccupied)
		{
			Color tempColor = spriteRenderer.color;
			tempColor.a = 0f;
			spriteRenderer.color = tempColor;
		}
	}

	
	public GameObject buildObjectAt(Vector3 pos, GameObject obj)
	{
		return Instantiate(obj, pos, Quaternion.identity) as GameObject;
	}
}
