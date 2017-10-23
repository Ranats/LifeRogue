using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Vector2 coordinate;// { get; private set; }// = new Vector2(0, 0);

    Vector2 default_position;
    Vector3 default_scale;

    int GRID_SIZE;
    float CELL_SIZE;

    Animation anim;
    Animator animator;
    public AudioSource audio_source;


    LifeGame lifeGame;

    public int damaged;

    public int state;



    public void Instantiate(int x, int y)
    {
        state = 0;
        coordinate.Set(x, y);
        lifeGame = GameObject.Find("GameBoad").GetComponent<LifeGame>();
        GRID_SIZE = lifeGame.GRID_SIZE;
        CELL_SIZE = lifeGame.CELL_SIZE;

        animator = GetComponent<Animator>();
        anim = GetComponent<Animation>();
        audio_source = GetComponent<AudioSource>();

        default_position = new Vector2(x, y);
        default_scale = this.transform.localScale;
        damaged = 0;
    }


    public void Move(Vector2 diff)
    {
        print("move");
//        animator.SetBool("move", true);
        anim.Play();
        coordinate += diff;

        if (diff == Vector2.zero) { return; }

        coordinate.Set(Mathf.Min(GRID_SIZE-1, Mathf.Max(0, coordinate.x)),
                       Mathf.Min(GRID_SIZE-1, Mathf.Max(0, coordinate.y)));

        float xPos = (GRID_SIZE - 1) - (coordinate.x * CELL_SIZE);
        float yPos = (GRID_SIZE - 1) - (coordinate.y * CELL_SIZE);
        Vector3 pos = new Vector3(-xPos, yPos, 0);
        this.transform.position = pos;
        
    }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
//        animator.SetBool("move",false);

        if (Music.IsNearChanged)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
            {
                //  respawn
                if (lifeGame.cells[(int)coordinate.x, (int)coordinate.y].Living)
                {
                    print("hit");
                    //                Instantiate((int)default_position.x, (int)default_position.y);
                    Music.QuantizePlay(audio_source);
                    this.transform.localScale = default_scale;
                    Move(new Vector2((default_position.x - coordinate.x), (default_position.y - coordinate.y)));
                    damaged++;
                }
            }
        }
	}
}
