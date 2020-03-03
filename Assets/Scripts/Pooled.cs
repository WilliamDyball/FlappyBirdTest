using System.Collections;
using Unity;
using UnityEngine;
//Class to trigger a function when spawning a pooled object and reset them all when the player is hit
public class Pooled : MonoBehaviour {
    public static bool bReset;
    public virtual void OnObjectSpawn() { }
}