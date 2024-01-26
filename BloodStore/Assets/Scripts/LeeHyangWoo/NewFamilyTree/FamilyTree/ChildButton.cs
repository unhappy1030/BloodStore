using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildButton : MonoBehaviour
{
    public Group group;
    private void Start() {
        group = gameObject.GetComponentInParent<Group>();
    }
}
