using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;


public class Title : MonoBehaviour {

    public LifeGame lifegame;
    public AudioSource audio;
    public Animation panel;
    public GameObject BGM;


    public Text titleText;

    string str;

    [SerializeField]
    int selected, state = 0,lr;

	// Use this for initialization
	void Start () {
//        GameObject.Find("BGM").GetComponent<AudioSource>().Play();
        lifegame.StartCoroutine("LifeGameCoroutine");
        str = titleText.text;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LifeOuts")
        {
            lr = 0;
            selected = 1;
            lifegame.GRID_SIZE = 16;
            lifegame.difficulity = selected;
            lifegame.InitCells(selected);
            lifegame.LoadMap(0, 0, mapAsset: lifegame.lights_map[selected]);
        }
        else
        {
            lr = 1;
            selected = 2;
            lifegame.GRID_SIZE = 50;
            lifegame.difficulity = selected;

            lifegame.InitCells(-1);
            lifegame.LoadMap(0, 0, mapAsset: lifegame.map);
        }
        push = false;




    }

    bool push;

    // Update is called once per frame
    void Update() {
        if (state == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }

            if (Input.GetAxisRaw("Vertical") > 0 && selected >= 1 && !push)
            {
                audio.Play();
                if(lr == 0)
                {
                    if(selected == 1)
                    {
                        lifegame.GRID_SIZE = 16;
                        str = "Easy  ◀\nNormal\nHard  ";
                    }
                    if (selected == 2)
                    {
                        lifegame.GRID_SIZE = 16;
                                                str = "Easy  \nNormal◀\nHard  ";
                        push = true;
                    }
                    selected--;
                    lifegame.difficulity = selected;
                    lifegame.InitCells(selected);
                    lifegame.LoadMap(0, 0, mapAsset: lifegame.lights_map[selected]);
                }
                else
                {
                    str = "▶Easy\n　Normal";
                    selected = 0;
                    lifegame.difficulity = 2;
                 //   lifegame.GRID_SIZE = 50;
                 //   lifegame.InitCells(-1);
                 // lifegame.LoadMap(0, 0, mapAsset: lifegame.map);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0 && selected < 2 && !push)
            {
                audio.Play();
                if (lr == 0)
                {
                    if(selected == 0)
                    {
                        lifegame.GRID_SIZE = 16;
                                                str = "Easy  \nNormal◀\nHard  ";
                        push = true;
                    }
                    if(selected == 1)
                    {
                        lifegame.GRID_SIZE = 15;
                                                str = "Easy  \nNormal\nHard  ◀";
                    }
                    selected++;
                    lifegame.difficulity = selected;
                    lifegame.InitCells(selected);
                    lifegame.LoadMap(0, 0, mapAsset: lifegame.lights_map[selected]);
                }
                else
                {
                    str = "　Easy\n▶Normal";
                    selected = 2;
                    lifegame.difficulity = 1;
                    lifegame.GRID_SIZE = 50;
                 //   lifegame.InitCells(-1);
                 //   lifegame.LoadMap(0, 0, mapAsset: lifegame.map);
                }
            }
            else if(Input.GetAxisRaw("Vertical") == 0)
            {
                push = false;
            }

            titleText.text = str;

            if (Input.GetAxisRaw("Submit") > 0 && state == 0)
            {
//                lifegame.difficulity = selected;
                if(lr == 0)
                {
                    panel.Play("panel_out_L");
                }
                else
                {
                    StartCoroutine(GameStart());
                    panel.Play("panel_out");
                }
                lifegame.StopAllCoroutines();

                print("submit");
                state = 1;
                Invoke("loadGame", 5f);
                BGM.GetComponent<Animation>().Play();   // fade
                Invoke("StopBGM", 1f);
            }
        }
    }

    private Vector3 oldPos,
               cameraZero = new Vector3(0, 0, -10);
    
    public GameObject panelR;

    void StopBGM()
    {
        if (lr == 0)
        {
            Camera.main.transform.position = new Vector3(0, 0, -10);
            panelR.SetActive(true);
        }
        BGM.GetComponent<AudioSource>().Stop();
        lifegame.load();
      
    }

    void loadGame()
    {
        BGM.GetComponent<AudioSource>().volume = 1;
        BGM.GetComponent<AudioSource>().loop = false;
        lifegame.GameStart();
    }

    IEnumerator GameStart()
    {
        while (true)
        {
            Camera.main.orthographicSize += 1f;

            if (Camera.main.orthographicSize > 40)
            {
                Vector3 diff = Camera.main.transform.position - cameraZero;
                Camera.main.transform.Translate(-diff / Mathf.Clamp((55 - Camera.main.orthographicSize), 1, 55));
            }

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 10, 55);

            
            if(Camera.main.orthographicSize == 55)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
