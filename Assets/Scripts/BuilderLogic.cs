using UnityEngine;
using System.Collections;

public class BuilderLogic : MonoBehaviour {

	public bool hasSelectedABlock {get; set;}
	public GameObject blockToAdd {get; set;}
	public SpriteRenderer blockSpriteRenderer {get; set;}
	public Vector3 facingRotation = Vector3.zero;
	public bool isHovering {get; set;}
	public GameObject hoveringOver {get; set;}

	// Use this for initialization
	void Start () {
		hasSelectedABlock = false;
	}

	public void setSelectedBlock(GameObject obj)
	{
		hasSelectedABlock = true;
		blockToAdd = obj;
		blockSpriteRenderer = obj.GetComponent<SpriteRenderer>();
		facingRotation = Vector3.zero;
	}

	void Update () {
		if(isHovering)// && Input.GetAxisRaw("Mouse ScrollWheel") != 0)
		{
			float rotation = Input.GetAxisRaw("Mouse ScrollWheel");
			if(rotation > 0)
			{
				facingRotation += new Vector3(0, 0, 90f);
				if(isHovering)
				{
					hoveringOver.transform.Rotate(0, 0, 90f);
				}
			}
			else if(rotation < 0)
			{
				facingRotation += new Vector3(0, 0, -90f);
				if(isHovering)
				{
					hoveringOver.transform.Rotate(0, 0, -90f);
				}
			}
		}
	}
}
