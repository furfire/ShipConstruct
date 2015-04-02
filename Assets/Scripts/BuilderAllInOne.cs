using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderAllInOne : MonoBehaviour 
{
    private bool hasSelectedABlock { get; set; }
    private GameObject blockToAdd { get; set; }

    private GameObject placeHolderForBuilding { get; set; }

    private SpriteRenderer blockSpriteRenderer { get; set; }
    private Vector3 facingRotation = Vector3.zero;
    private GameObject hoveringOver { get; set; }

    //private Vector2 gridCoords { get; set; }

    private Dictionary<Vector2, GameObject> cubeArray = new Dictionary<Vector2, GameObject>();

    // Use this for initialization
    void Start()
    {
        hasSelectedABlock = false;
    }

    public void setSelectedBlock(GameObject obj)
    {
        blockToAdd = obj;
        if (placeHolderForBuilding != null)
        {
            Destroy(placeHolderForBuilding);
        }
        hasSelectedABlock = true;

        placeHolderForBuilding = Instantiate(obj, new Vector3(0, 0, -50), Quaternion.identity) as GameObject;
        blockSpriteRenderer = placeHolderForBuilding.GetComponent<SpriteRenderer>();
        facingRotation = Vector3.zero;
    }

    //on collision enter for the builder blocks array, 

    //2 layers, top layer just gets mouse position
    //bottom layer actually has all the cubes.  Need to seperate bottom layer so hovering over placed cubes doesnt block this script from working

    void OnMouseOver()
    {
        if (hasSelectedABlock)
        {
            Vector2 gridCoords = findCoordsFromLocalPosition();

            //handle movement of placeholder
            GameObject theCube;
            if (!cubeArray.TryGetValue(gridCoords, out theCube))
            {
                placeHolderForBuilding.transform.position = gridCoords;
                RotateOnMouseWheel(gridCoords);
                

                //eventually you'll need to something in the 'else' section for block destruction too
                AddBlockOnClick(gridCoords);
            }
            else
            {
                //hide it
                placeHolderForBuilding.transform.position = new Vector3(0, 0, -50);

                removeBlockOnRightClick(gridCoords);
            }
        }
    }

    void OnMouseExit()
    {
        if (hasSelectedABlock)
        {
            //hide it
            placeHolderForBuilding.transform.position = new Vector3(0, 0, -50);
        }

    }

    private void removeBlockOnRightClick(Vector2 gridCoords)
    {
        if (Input.GetMouseButton(1))
        {
            GameObject theCube;
            if(cubeArray.TryGetValue(gridCoords, out theCube))
            {
                cubeArray.Remove(gridCoords);
                Destroy(theCube);
            }
            
            //do cube destruction stuff!
            //destroy the joints too
            //iterate through the joints on adjacent blocks, destroy the right ones

            /*occupiedBy = null;
            isOccupied = false;
            Color tempColor = spriteRenderer.color;
            tempColor.a = 0f;
            spriteRenderer.color = tempColor;
            builderLogic.isHovering = true;*/
            
        }
    }

    private void AddBlockOnClick(Vector2 gridCoords)
    {
        if (Input.GetMouseButton(0))
        {
            cubeArray.Add(gridCoords, placeHolderForBuilding);
            //spriteRenderer.sprite = builderLogic.blockSpriteRenderer.sprite;
            //isOccupied = true;
            //Color blocksColor = builderLogic.blockSpriteRenderer.color;
            //Color tempColor = new Color(blocksColor.r, blocksColor.g, blocksColor.b, blocksColor.a);
            //tempColor.a = 1f;
            //spriteRenderer.color = tempColor;

            //add joints to adjacent cubes
            //will need to put checks in place to somehow determine if cubes are jointable at this angle
            JointAngleLimits2D limits = new JointAngleLimits2D();
            limits.max = 0f;
            //left
            connectObjectsWithJoint(placeHolderForBuilding, -1, 0, limits);
            //right
            connectObjectsWithJoint(placeHolderForBuilding, 1, 0, limits);
            //down
            connectObjectsWithJoint(placeHolderForBuilding, 0, -1, limits);
            //up
            connectObjectsWithJoint(placeHolderForBuilding, 0, 1, limits);

            //enables shadows on certain shaders!
            blockSpriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            //reinstantiate the placeholder block!
            placeHolderForBuilding = Instantiate(blockToAdd, new Vector3(0, 0, -50), Quaternion.identity) as GameObject;
            blockSpriteRenderer = placeHolderForBuilding.GetComponent<SpriteRenderer>();
        }
    }

    //TODO:  change this to use each individual blocks' script so we can check whether a block can be connected on that side!!!
    private void connectObjectsWithJoint(GameObject originalCube, int adjX, int adjY, JointAngleLimits2D limits)
    {
        Vector2 adjacentCubePos = new Vector2(originalCube.transform.position.x + adjX, originalCube.transform.position.y + adjY);

        //Vector2 gridCoords

        GameObject adjacentCube;
        if (cubeArray.TryGetValue(adjacentCubePos, out adjacentCube))
        {
            //adjX and adjY represent position in array of cubes relative to x,y of originalCube
            float halfOfCubeSize = 0.5f;

            HingeJoint2D joint = originalCube.AddComponent(typeof(HingeJoint2D)) as HingeJoint2D;
            joint.connectedBody = adjacentCube.GetComponent<Rigidbody2D>();
            joint.useLimits = true;
            joint.limits = limits;

            joint.anchor = originalCube.transform.InverseTransformPoint(originalCube.transform.position + new Vector3(halfOfCubeSize * adjX, halfOfCubeSize * adjY));
            joint.connectedAnchor = adjacentCube.transform.InverseTransformPoint(adjacentCube.transform.position + new Vector3(halfOfCubeSize * -adjX, halfOfCubeSize * -adjY));
        }
    }

    private Vector2 findCoordsFromLocalPosition()
    {
        //get the position we are hovering over:
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 posFromBottomLeft = mousePositionInWorld;

        return new Vector2(Mathf.RoundToInt(posFromBottomLeft.x), Mathf.RoundToInt(posFromBottomLeft.y)); 
    }

    private void RotateOnMouseWheel(Vector2 coords)
    {
        //if there's a blank spot
        if(!cubeArray.ContainsKey(coords))
        {
            float rotation = Input.GetAxisRaw("Mouse ScrollWheel");
            if (rotation > 0)
            {
                facingRotation += new Vector3(0, 0, 90f);
                placeHolderForBuilding.transform.Rotate(0, 0, 90f);
            }
            else if (rotation < 0)
            {
                facingRotation += new Vector3(0, 0, -90f);
                placeHolderForBuilding.transform.Rotate(0, 0, -90f);
            }
        }
    }
}
