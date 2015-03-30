using UnityEngine;
using System.Collections;

public class InitGrid : MonoBehaviour {

	//if you change this, also change code in the script on the cubes in cube array
	static int arraySize = 30;


	public GameObject cube;
	public GameObject[,] cubes = new GameObject[arraySize,arraySize];

	// Use this for initialization
	void Start () {
		int xOffset = 4;
		for(int x = 0; x < arraySize; x++)
		{
			for(int y = 0; y < arraySize; y++) 
		    {
				GameObject cubeObj = Instantiate(cube, new Vector3(x + xOffset, y, 0), Quaternion.identity) as GameObject;

				SpriteRenderer spriteRenderer = cubeObj.gameObject.GetComponent<SpriteRenderer>();
				Color tempColor = spriteRenderer.color;
				tempColor.a = 0f;
				spriteRenderer.color = tempColor;
				cubes[x,y] = cubeObj;
				ShowOnClick cubeOnClick = cubeObj.GetComponent<ShowOnClick>();

				//init the cube's scripts  - later on I can just use world position to cut down on code if need be.
				cubeOnClick.cubeArray = cubes;
				cubeOnClick.posXInArray = x;
				cubeOnClick.posYInArray = y;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
