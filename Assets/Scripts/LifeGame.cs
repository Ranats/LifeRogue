using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;


public class LifeGame : MonoBehaviour
{

    public GameObject cellPrefab,
                      goalPrefab,
                      cellHolderPrefab;

    public Result resultScript;

    public Cell[,] cells { get; private set; }
    public GameObject cellObj;

    public float CELL_SIZE = 1f;
    public int GRID_SIZE = 100; //  1辺のセル数

    private int state = 0;

    public enum MapType
    {
        xp46,
        galaxy,
        toilet
    }

    Vector2 goal;
    public AudioClip goal_sound, over_sound;

    [SerializeField]
    float time;

    //    [SerializeField]    MapType map;// = MapType.A;
    [SerializeField]
public TextAsset map;// = MapType.A;

    [SerializeField]
    public TextAsset[] lights_map;

    [SerializeField]
    GameObject player_prefab;

    public bool save;

    public GameObject player;

    public int difficulity;

    [SerializeField]
    int lr;

    bool skip;
    int liveCount, step;
    public Text steps;
    public RectTransform[] buttons;

    public void InitCells(int selected)
    {
        Destroy(cellObj.gameObject);
        Destroy(player.gameObject);
        
        //        if (selected < 0)
        //        {
        //            foreach (Cell cell in cells)
        //            {
        //                Destroy(cell.gameObject);
        //            }
        //        }

        cellObj = GameObjectCommon.InstantiateChild(cellHolderPrefab, new Vector3(0, 0, 0), this.transform);
        cells = new Cell[GRID_SIZE, GRID_SIZE];

        for (int y = 0; y < GRID_SIZE; y++)
        {
            for (int x = 0; x < GRID_SIZE; x++)
            {
                float xPos = (GRID_SIZE - 1) - (x * CELL_SIZE);
                float yPos = (GRID_SIZE - 1) - (y * CELL_SIZE);
                Vector3 pos = new Vector3(-xPos, yPos, 0);
                GameObject obj = GameObjectCommon.InstantiateChild(cellPrefab, pos, cellObj.transform);
                obj.transform.localScale = Vector3.one * CELL_SIZE;

                cells[x, y] = obj.GetComponent<Cell>();

            }
        }

        if (lr == 0)
        {
            int x, y;
            float xPos, yPos;
            if (difficulity == 2)
            {
                x = 14;
                y = 14;
                xPos = (GRID_SIZE - 1) - (x * CELL_SIZE);
                yPos = (GRID_SIZE - 1) - (y * CELL_SIZE);
            }
            else
            {
                x = 15;
                y = 15;
                xPos = (GRID_SIZE - 1) - (x * CELL_SIZE);
                yPos = (GRID_SIZE - 1) - (y * CELL_SIZE);
            }

            Vector3 pos = new Vector3(-xPos, yPos, 0);
            player = GameObjectCommon.Instantiate(player_prefab, pos);
            player.GetComponent<Player>().Instantiate(x, y);

        }
        else
        {
            int x = 25,
                y = 0;

            float xPos = (GRID_SIZE - 1) - (x * CELL_SIZE);
            float yPos = (GRID_SIZE - 1) - (y * CELL_SIZE);
            Vector3 pos = new Vector3(-xPos, yPos, 0);
            player = GameObjectCommon.Instantiate(player_prefab, pos);
            player.GetComponent<Player>().Instantiate(x, y);

        }

        //        LoadMap(0, 0, mapAsset: lights_map[selected]);
        //        LoadMap(0, 0, mapAsset: map);
    }

    // Use this for initialization
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LifeOuts")
        {
            lr = 0;
            skip = false;
            step = 0;
            GRID_SIZE = 15;
        }
        else
        {
            lr = 1;
        }
        // 100/g
        //        CELL_SIZE = 100f / GRID_SIZE;

        //  セルを生成
        InitCells(1);
        // init player

        /*
         * while (true)
    {
        int x = Random.Range(0, GRID_SIZE);
        int y = Random.Range(0, GRID_SIZE);

        int count = checkCell(x, y);
//            print(count);
        if (cells[x, y].Living) { count++; }

        if (count == 0)
        {
            float xPos = (GRID_SIZE - 1) - (x * CELL_SIZE);
            float yPos = (GRID_SIZE - 1) - (y * CELL_SIZE);
            Vector3 pos = new Vector3(-xPos, yPos, 0);
            player = GameObjectCommon.Instantiate(player_prefab, pos);
            player.GetComponent<Player>().Instantiate(x, y);
            break;
        }
    }
        */

        state = 1; // ready

    }

    public void load()
    {
        if (lr == 0)
        {
            LoadMap(0, 0, mapAsset: lights_map[difficulity]);
            liveCount = 0;
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    if (cells[x, y].Alive) { liveCount++; }
                }
            }
        }
        else
        {
            LoadMap(0, 0, mapAsset: map);
            if (save) { SaveMap(); }
        }
    }

    public void GameStart()
    {
        GameObject.Find("BGM").GetComponent<AudioSource>().Play();
        //        if(lr == 1)
        //        {
        StartCoroutine("LifeGameCoroutine");
        //        }
        state = 2;
        time = 0;
    }

    public void LoadMap(int x, int y, MapType map = MapType.galaxy, TextAsset mapAsset = null)
    {
        print("load");
        /**        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath+"/Resources/Map");
                System.IO.FileInfo[] info = dir.GetFiles("*.csv");
                foreach(System.IO.FileInfo f in info)
                {
                    print(f.Name);
                }
        **/
        TextAsset csvFile;
        if (mapAsset != null)
        {
            csvFile = mapAsset;
        }
        else
        {
            csvFile = Resources.Load("Map/" + map.ToString()) as TextAsset;
        }
        StringReader reader = new StringReader(csvFile.text);
        List<string[]> csvDatas = new List<string[]>();

        int i = 0;
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            int j = 0;
            foreach (string cell in line.Split(','))
            {
                if (cell == "1")
                {
                    cells[y + j, x + i].Birth();
                    cells[y + j, x + i].isAlive = true;
                    cells[y + j, x + i].Living = true;

                } else
                {
                    print(x + i);
                    print(y + j);
                    cells[y + j, x + i].Die();
                    cells[y + j, x + i].isAlive = false;
                    cells[y + j, x + i].Living = false;

                }

                if (cell == "g")
                {
                    cells[y + j, x + i].isGoal = true;

                    float xPos = (GRID_SIZE - 1) - (x + i * CELL_SIZE) - 2f;
                    float yPos = (GRID_SIZE - 1) - (y + j * CELL_SIZE);
                    GameObject obj = GameObject.Instantiate(goalPrefab);
                    obj.transform.position = new Vector3(xPos, yPos, 0);

                    goal = new Vector2(i, j);
                }
                else
                {
                    cells[y + j, x + i].isGoal = false;
                }

                //                GameObject obj = GameObjectCommon.InstantiateChild(goalPrefab, pos, cells[y + j, x + i].transform);
                //obj.transform.localScale = Vector3.one * CELL_SIZE;

                j++;
            }
            i++;
            //            csvDatas.Add(line.Split(','));    
        }
    }

    void SaveMap()
    {
        StreamWriter writer;

        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath + "\\Resources\\Map\\Output");
        System.IO.FileInfo[] info = dir.GetFiles("*.csv");

        FileInfo fi = new FileInfo(Application.dataPath + "\\Resources\\Map\\Output\\" + "Map" + info.Length + ".csv");
        writer = fi.AppendText();
        for (int y = 0; y < GRID_SIZE; y++)
        {
            List<string> line = new List<string>();
            for (int x = 0; x < GRID_SIZE; x++)
            {
                line.Add((cells[x, y].isAlive) ? "1" : "0");
                //print(string.Format("{0},{1}:{2}", y, x, cells[y, x].isAlive));
            }
            writer.WriteLine(string.Join(",", (string[])line.ToArray()));
        }
        writer.Flush();
        writer.Close();
    }

    private Vector3 oldPos,
                    cameraZero = new Vector3(0, 0, -10);

    // Update is called once per frame
    void Update()
    {

        // DRY...DRY...
        //        switch (state)
        //        {
        //            case 1:
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Camera.main.orthographicSize -= scroll * 10;

            //  ズームアウト時にカメラの位置を中心に戻す
            if (Camera.main.orthographicSize > 40)
            {
                Vector3 diff = Camera.main.transform.position - cameraZero;
                Camera.main.transform.Translate(-diff / Mathf.Clamp((55 - Camera.main.orthographicSize), 1, 55));
            }

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 10, 55);

            // マウス位置にズーム
            //                    Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //                    Camera.main.transform.Translate(-(Camera.main.transform.position - target ) / 10f);
        }

        //  ホイールドラッグ
        if (Input.GetMouseButtonDown(2))
        {
            oldPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 diff = Input.mousePosition - oldPos;
            Camera.main.transform.Translate(-diff / 10f);
            oldPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D clickPoint = Physics2D.OverlapPoint(ray);
            //                    RaycastHit2D hit = new RaycastHit2D();
            if (clickPoint)
            {
                Cell cell = clickPoint.gameObject.GetComponent<Cell>();
                if (!cell.isAlive)
                {
                    cell.Birth();
                }
            }
            //if (Physics2D.Raycast(ray, -Vector2.up))
            //{
            //    Cell cell = hit.collider.gameObject.GetComponent<Cell>();
            //    if (!cell.isAlive)
            //    {
            //        cell.Birth();
            //    }
            //}
        }
        else if (Input.GetMouseButton(1))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D clickPoint = Physics2D.OverlapPoint(ray);
            //                    RaycastHit2D hit = new RaycastHit2D();
            if (clickPoint)
            {
                Cell cell = clickPoint.gameObject.GetComponent<Cell>();
                if (cell.isAlive)
                {
                    cell.Die();
                }
            }
        }

        //  一回だけ押せるようにしとかないと重複して呼び出されてﾀｲﾍﾝ
        //  UGUIのボタン
        if (Input.GetKeyDown(KeyCode.S))
        {
            switch (state)
            {
                case 1:
                    GameObject.Find("BGM").GetComponent<AudioSource>().Play();
                    StartCoroutine("LifeGameCoroutine");
                    if (save) { SaveMap(); }
                    state = 2;
                    time = 0;
                    break;
                case 2:
                    StopCoroutine("LifeGameCoroutine");
                    state = 1;
                    break;
            }
        }

        // player
        //c_diff.x = Input.GetAxis("Horizontal");
        //c_diff.y = Input.GetAxis("Vertical");

        c_diff = Vector2.zero;

        if (state == 2)
        {

            if (Input.GetAxisRaw("Horizontal") != 0)
                c_diff.x = Input.GetAxisRaw("Horizontal");
            else
                c_diff.y = -Input.GetAxisRaw("Vertical");
            /*
                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            c_diff.x = -1;
                        }
                        else if (Input.GetKey(KeyCode.RightArrow))
                        {
                            c_diff.x = 1;
                        }
                        else if (Input.GetKey(KeyCode.UpArrow))
                        {
                            c_diff.y = -1;
                        }
                        else if (Input.GetKey(KeyCode.DownArrow))
                        {
                            c_diff.y = 1;
                        }
                        */
        }

        if (player.GetComponent<Player>().coordinate == goal && lr == 1)
        {
            Goal();
        }

        //  time Over 170 2 2
        if (!Music.IsPlaying && state == 2)
        {
            player.GetComponent<Player>().audio_source.clip = over_sound;
            player.GetComponent<Player>().audio_source.Play();

            GameObject.Find("BGM").GetComponents<AudioSource>()[2].Play();
            c_diff = Vector2.zero;

            if (lr == 0)
            {
                resultScript.OpenL(-1, step, difficulity);
            }
            else
            {
                resultScript.OpenR(-1, player.GetComponent<Player>().damaged,difficulity);
            }
            state = 3;

            Invoke("ReturnTitle", 30f);

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReturnTitle();
        }

        if (state == 2)
        {
            time += Time.deltaTime;

            if (lr == 0)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton3))  //  △
                {
                    print("3");
                    skip = true;
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton2))  //  o
                {
                    player.GetComponent<Player>().state = 1;
                    print("2");
                    buttons[0].localScale = Vector3.one * 1.5f;
                    buttons[1].localScale = Vector3.one * 0.8f;
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton1))  //  x
                {
                    player.GetComponent<Player>().state = -1;
                    print("1");
                    buttons[0].localScale = Vector3.one * 0.8f;
                    buttons[1].localScale = Vector3.one * 1.5f;
                }

            }

        }
        //                break;
        //        }
    }

    void ReturnTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void Goal()
    {
        if (state < 3)
        {
            player.GetComponent<Player>().audio_source.clip = goal_sound;
            player.GetComponent<Player>().audio_source.Play();
            GameObject.Find("BGM").GetComponent<AudioSource>().Stop();
            GameObject.Find("BGM").GetComponents<AudioSource>()[2].Play();
            c_diff = Vector2.zero;
            if(lr == 0)
            {
                resultScript.OpenL(time,step,difficulity);
            }
            else
            {
                resultScript.OpenR(time, player.GetComponent<Player>().damaged,difficulity);
            }
        }
        print("goal");
        state = 3;
    }
    
    Vector2 c_diff = new Vector2(0, 0);

    [SerializeField]
    int ratio;

    public IEnumerator LifeGameCoroutine()
    {
        while (true)
        {
            // 全セルを更新
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LifeOuts")
            {
                if (Music.IsNearChanged && Music.Near.Unit % ratio == 0)
                {
                    if (state == 1)
                    {
                        for (int x = 0; x < GRID_SIZE; x++)
                        {
                            for (int y = 0; y < GRID_SIZE; y++)
                            {
                                UpdateCell(x, y, Vector2.zero);
                            }
                        }
                    }
                    else
                    {


                        Vector2 before = player.GetComponent<Player>().coordinate;
                        player.GetComponent<Player>().Move(c_diff);
                        if (c_diff != Vector2.zero || skip)
                        {
                            for (int x = 0; x < GRID_SIZE; x++)
                            {
                                for (int y = 0; y < GRID_SIZE; y++)
                                {
                                    UpdateCell(x, y, before);
                                }
                            }
                            skip = false;
                            step++;
                            steps.text = "Step : " + step.ToString();
                        }

                        liveCount = 0;
                        for (int x = 0; x < GRID_SIZE; x++)
                        {
                            for (int y = 0; y < GRID_SIZE; y++)
                            {
                                if (cells[x, y].Living) { liveCount++; }
                            }
                        }

                        print(liveCount);
                        if (liveCount == 0)
                        {
                            Goal();
                        }
                    }
                }
            }
            else
            {
                if (Music.IsNearChangedBeat() && Music.Near.Beat % difficulity == 0)
                {
                    for (int x = 0; x < GRID_SIZE; x++)
                    {
                        for (int y = 0; y < GRID_SIZE; y++)
                        {
                            UpdateCell(x, y,Vector2.zero);
                        }
                    }

                    //                Input.ResetInputAxes();
                }

                if (Music.IsNearChanged && Music.Near.Unit % ratio == 0)
                {
                    player.GetComponent<Player>().Move(c_diff);
                }
            }

            yield return new WaitForSeconds(0f);
        }
    }

    private int checkCell(int cellX, int cellY, Vector2 pCell)
    {
        int count = 0;
        int[] xsurround = { cellX - 1, cellX, cellX + 1 },
              ysurround = { cellY - 1, cellY, cellY + 1 };


        foreach (int x in xsurround)
        {
            foreach (int y in ysurround)
            {
                if (x == cellX && y == cellY)
                {
                    continue;
                }
                int g_x = x,
                    g_y = y;
                if (x < 0) { g_x = GRID_SIZE - 1; }
                if (x >= GRID_SIZE) { g_x = 0; }
                if (y < 0) { g_y = GRID_SIZE - 1; }
                if (y >= GRID_SIZE) { g_y = 0; }

                if(new Vector2(g_x,g_y) == pCell && player.GetComponent<Player>().state != 0)
                {
                    if(player.GetComponent<Player>().state == 1)
                        count++;
                }
                else if (cells[g_x, g_y].Living)
                {
                    count++;
                }
                //                print(string.Format("y:{0},x:{1}", y, x));
                //                print(string.Format("gy:{0},gx:{1}", g_y, g_x));
            }
        }
        return count;
    }

    private int checkCell(int cellX, int cellY)
    {
        int count = 0;
        int[] xsurround = { cellX - 1, cellX, cellX + 1 },
              ysurround = { cellY - 1, cellY, cellY + 1 };


        foreach (int x in xsurround)
        {
            foreach (int y in ysurround)
            {
                if (x == cellX && y == cellY)
                {
                    continue;
                }
                int g_x = x,
                    g_y = y;
                if (x < 0) { g_x = GRID_SIZE - 1; }
                if (x >= GRID_SIZE) { g_x = 0; }
                if (y < 0) { g_y = GRID_SIZE - 1; }
                if (y >= GRID_SIZE) { g_y = 0; }

                if (cells[g_x, g_y].Living)
                {
                    count++;
                }
                //                print(string.Format("y:{0},x:{1}", y, x));
                //                print(string.Format("gy:{0},gx:{1}", g_y, g_x));
            }
        }


        /**        for (int x_surround = cellX - 1; x_surround <= cellX + 1; x_surround++)
                {
                    for (int y_surround = cellY - 1; y_surround <= cellY + 1; y_surround++)
                    {
                        if ((x_surround == cellX && y_surround == cellY) || x_surround < 0 || y_surround < 0 || x_surround >= GRID_SIZE || y_surround >= GRID_SIZE)
                        {
                            continue;
                        }


                        //  左右上下をループ
                        //				if (cells [(x + gridSize) % gridSize, (z + gridSize) % gridSize].Living) {

                        //                if(cells[x, z].isAlive != cells[x,z].Living){
                        //                    print(cells[x, z].isAlive + " " + cells[x, z].Living);
                        //                }
                        //                if (cells[x, z].isAlive)
                        if (cells[x_surround, y_surround].Living)
                        {
                            //  if 相手のセル ... countEnemy++;
                            //  else count++;
                            count++;
                        }
                    }
                }
            **/
        return count;
    }

    /// セルの状態を更新
    private void UpdateCell(int cellX, int cellY, Vector2 pCell)
    {
        int count = checkCell(cellX, cellY, pCell);

        // 誕生/死滅
        Cell cell = cells[cellX, cellY];

        if (cell.Living)
        {
            // 周囲の生存セルが１以下、または４以上のとき死滅
            //  相手のセルによる過密で死滅させる
            //  count <= 1 || count+countEnemy >= 4)
            if (count <= 1 || count >= 4)
            {
                cell.Die();
            }
        }
        else
        {
            // 周囲の生存セルが３つのとき誕生
            //  相手のセルによる誕生はしない。
            if (count == 3)
            {
                cell.Birth();
            }
        }
    }
}
