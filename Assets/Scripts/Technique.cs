using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technique", menuName = "Moves/Technique")]
public class Technique : ScriptableObject 
{
	//public string name = "";
    public int damage = 0;
    public int stun = 0;
    public float force = 0.0f;
    public bool launch = false;
    public bool juggle = false;
    public bool dizzy = false;
}
