using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour {

    AudioSource src;
    [SerializeField]
    int currentTiming;

	// Use this for initialization
	void Start () {
        src = this.GetComponents<AudioSource>()[1];
        currentTiming = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if ( Music.IsNearChangedBeat() && Music.Just.Beat != currentTiming )
        {
            Music.QuantizePlay(src);
//            src.PlayOneShot(src.clip);
            currentTiming = Music.Just.Beat;
        }
    }

    //  25,78
    //  player : 15x15
}
