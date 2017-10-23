using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

//    public Texture Alive, Dead;
    public Sprite Alive, Dead;
    public bool isAlive;

    public bool isGoal;

    public bool Living;// { get; private set; }	// このセルが生存状態か


    private SpriteRenderer renderer;
    private Renderer ren;

    void Awake()
    {
        renderer = this.GetComponent<SpriteRenderer>();
        ren = this.GetComponent<Renderer>();
//        renderer = this.gameObject.GetComponent<Renderer>();
//        if (Random.Range(0f, 1f) > 0.95f)
//        {
//            print("death");
//            isAlive = false;
//        }
//        else
//        {
//            print("alive");
//            isAlive = true;
//        }
//        renderer.material.mainTexture = (isAlive) ? Alive : Dead;
    }
	// Use this for initialization
	void Start () {
//        StartCoroutine("blink");
//        renderer.material.mainTexture = (isAlive) ? Alive : Dead;

	}

    void Update()
    {
        if (Music.IsNearChanged && isGoal)
        {
            Living = false;
        }
        else
        {
            Living = isAlive;
        }
    }

//    public void state_trans()
//    {
//        isAlive = !isAlive;
//        renderer.material.mainTexture = (isAlive) ? Alive : Dead;
//    }


    //  if 8近傍
    public void Birth()
    {
        isAlive = true;
        renderer.sprite = Alive;
//        ren.material.mainTexture = Alive;
//          ren.material.mainTextureOffset = new Vector2(0.5f,0);
//        renderer.material.mainTexture = Alive;
    }

    public void Die()
    {
        isAlive = false;
        renderer.sprite = Dead;
//.        ren.material.mainTexture = Dead;
//       ren.material.mainTextureOffset = new Vector2(0, 0);

//        renderer.material.mainTexture = Dead;
    }

    IEnumerator blink()
    {
        while (true)
        {
            isAlive = !isAlive;
//            renderer.material.mainTexture = (isAlive) ? Alive : Dead;
            yield return new WaitForSeconds(Random.Range(0.1f,1f));
        }
    }
}
