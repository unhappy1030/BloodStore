using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawFamilyTreeLine : MonoBehaviour
{
    public List<GameObject> testChilds;
    public float testHalfYSize;

    [Space]
    public float lineWidth = 0.01f;
    public float distFromGroup;
    float halfY;
    float horLineSize;
    float verLineSize;

    [Space]
    public Gradient lindColor;

    GameObject lineParent;
    List<GameObject> lines = new();
    
    void Start(){
        testHalfYSize = (testChilds[0].transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y)/2.0f;
        DrawLine(testChilds);
    }

    void DrawLine(List<GameObject> childGroups){
        SetSize(testHalfYSize); // test
        Debug.Log(childGroups.Count);

        if( childGroups == null || childGroups.Count == 0){
            Debug.Log("There is no Child...");
            return;
        }

        CreateLineObj(childGroups.Count);
        for(int i=0; i<childGroups.Count; i++){
            SetLinePoints(lines[i], childGroups[i], this.gameObject);
        }

        MoveLineToParentNode(this.gameObject, childGroups.Count);
    }

    void SetSize(float halfYSize){
        halfY = halfYSize;
    }

    void CreateLineObj(int lineCount){
        for(int i=0; i<lineCount; i++){
            Debug.Log("Add lines...");
            lines.Add(new("line"+i));
        }

        foreach(GameObject line in lines){
            line.AddComponent<LineRenderer>();
            line.transform.position = new Vector3(0, 0, 0);
        }
    }

    // set Line Points for each Line
    void SetLinePoints(GameObject line, GameObject childGroup, GameObject parentGroup){
        Debug.Log("Setting Line Points...");
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        if(lineRenderer == null){
            Debug.Log("There is no LineRenderer...");
            return;
        }

        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.material.color = Color.black;
        Vector3[] linePoints = new Vector3[4];
        
        horLineSize = this.transform.position.x - childGroup.transform.position.x;
        verLineSize = (this.transform.position.y - childGroup.transform.position.y)/2.0f;
        if(verLineSize <= 0){
            Debug.Log("Vertical Line Size cannot be negative number...");
            return;
        }

        linePoints[0] = new Vector3(parentGroup.transform.position.x, parentGroup.transform.position.y, parentGroup.transform.position.z);
        linePoints[1] = new Vector3(parentGroup.transform.position.x, parentGroup.transform.position.y-verLineSize, parentGroup.transform.position.z);
        linePoints[2] = new Vector3(parentGroup.transform.position.x + horLineSize, parentGroup.transform.position.y-verLineSize, parentGroup.transform.position.z);
        linePoints[3] = new Vector3(parentGroup.transform.position.x + horLineSize, parentGroup.transform.position.y-(verLineSize * 2), parentGroup.transform.position.z);

        lineRenderer.positionCount = linePoints.Count();
        lineRenderer.SetPositions(linePoints);
    }

    void MoveLineToParentNode(GameObject parentGroup, int lineCount){
        lineParent = new("LineParent");

        for(int i=0; i<lineCount; i++){
            lines[i].transform.SetParent(lineParent.transform);
        }
        
        lineParent.transform.position = parentGroup.transform.position;
    }
}
