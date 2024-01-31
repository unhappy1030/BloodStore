using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPackUITest : MonoBehaviour
{
    public string sex;
    public string[] bloodType;

    public void SetSex(string sex){
        this.sex = sex;
    }

    public void SetBloodType(string rh, string bloodType){
        this.bloodType[0] = bloodType;
        this.bloodType[1] = rh;
    }
}
