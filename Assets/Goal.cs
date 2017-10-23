using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    Animation anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update () {
        if (Music.IsNearChanged && Music.Near.Unit % 4 == 0)// && Music.Near.Beat %2 == 0)
        {
            print("change");
            anim.Play();
        }


    }
}
