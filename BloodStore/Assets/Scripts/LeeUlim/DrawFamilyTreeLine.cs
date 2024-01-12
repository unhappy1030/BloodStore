using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawFamilyTreeLine : MonoBehaviour
{
    public List<GameObject> testChilds;
    public float testSize;

    [Space]
    public float lineWidth = 0.01f;
    public float distFromGroup;
    float horLineSize;
    float verLineSize;
    float groupDist;

    [Space]
    public Gradient lindColor;

    GameObject lineParent;
    List<GameObject> lines = new();
    
    void Start(){
        DrawLine(testChilds);
    }

    void DrawLine(List<GameObject> childGroups){
        SetSize(testSize); // test
        Debug.Log(childGroups.Count);
        CreateLineObj(childGroups.Count);
        for(int i=0; i<childGroups.Count; i++){
            SetLinePoints(lines[i], childGroups[i]);
        }

        MoveLineToParentNode(this.gameObject);
    }

    void SetSize(float size){
        groupDist = size;
    }

    void CreateLineObj(int lineCount){
        lineParent = new("LineParent");
        lineParent.transform.position = new Vector3(0, 0, 0);

        for(int i=0; i<lineCount; i++){
            Debug.Log("Add lines...");
            lines.Add(new("line"+i));
        }

        foreach(GameObject line in lines){
            line.AddComponent<LineRenderer>();
            line.transform.position = new Vector3(0, 0, 0);
            line.transform.SetParent(lineParent.transform);
        }
    }

    // set Line Points for each Line
    void SetLinePoints(GameObject line, GameObject childGroup){
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
        verLineSize = groupDist/2.0f - distFromGroup;
        if(verLineSize <= 0){
            Debug.Log("Vertical Line Size cannot be negative number...");
            return;
        }

        linePoints[0] = new Vector3(0, 0, 0);
        linePoints[1] = new Vector3(0, -verLineSize, 0);
        linePoints[2] = new Vector3(horLineSize, -verLineSize, 0);
        linePoints[3] = new Vector3(horLineSize, -verLineSize * 2, 0);

        lineRenderer.positionCount = linePoints.Count();
        lineRenderer.SetPositions(linePoints);
    }

    void MoveLineToParentNode(GameObject parentNode){
        lineParent.transform.position = parentNode.transform.position;
        
    }
}
