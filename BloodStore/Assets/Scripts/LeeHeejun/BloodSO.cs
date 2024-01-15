using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person
{
    public string name;
    public string gender;
    public bool isRHNegative;
    public string bloodType;
    public int age;
}

[CreateAssetMenu(fileName = "BloodSO", menuName = "Scriptable Object/PersonSO")]
public class PersonSO : ScriptableObject
{
    public List<Person> people;
}