using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Result : MonoBehaviour {

    public GameObject panel,fade,thank;
    public GameObject[] texts;
    float[] timing = { 1.5f, 2.5f, 3.0f, 4.0f, 4.5f, 5.5f, 6.0f, 7.0f, 8f };
   

    string name;

    [SerializeField]
    int state;

    int lr;
    int rank,
        damaged,
        step,
        difficality;
    float score;

    public void OpenL(float _time, int _step, int _difficality)
    {
        score = _time;
        step = _step;
        difficality = _difficality;

        if (score < 0)
        {
            texts[0].GetComponent<Text>().text = "TIME UP!";
            texts[4].GetComponent<Text>().text = "--:--.--";
        }
        else
        {
            texts[4].GetComponent<Text>().text = string.Format("{0:00}:{1}", score / 60, (score % 60f).ToString("F2"));
        }

        texts[2].GetComponent<Text>().text = step.ToString();

        rank = GetRanking(step, score, difficality);

        string rank_s = "th";
        switch (rank)
        {
            case 0: rank_s = "st"; break;
            case 1: rank_s = "nd"; break;
            case 2: rank_s = "rd"; break;
        }

        texts[6].GetComponent<Text>().text = (rank + 1).ToString() + rank_s;

        print(rank);

        // 上位5
        if (rank < 5)
        {
            foreach (GameObject holder in nameHolder)
            {
                holder.SetActive(true);
            }
            state = 2;
            StartCoroutine("InputName");

            // name input visible
            // rank+1;
        }
        else
        {
            StartCoroutine(PlayAnimation(10f, thank));
            SetRanking(rank, step, score, "NIL", difficality);
            state = 1;
        }

        panel.SetActive(true);
        //        StartCoroutine(de(2f));
        //        StartCoroutine(PlayAnimation(0.5f, panel));
        int t = 0;
        foreach (GameObject text in texts)
        {
            StartCoroutine(PlayAnimation(timing[t++] - 1f, text));
        }
    }

    public void OpenR(float time,int _damaged,int difficality)
    {
        score = time;
        damaged = _damaged;

        if(score < 0)
        {
            texts[0].GetComponent<Text>().text = "TIME UP!";
            texts[2].GetComponent<Text>().text = "--:--.--";
        }
        else
        {
            texts[2].GetComponent<Text>().text = string.Format("{0:00}:{1}", score / 60, (score % 60f).ToString("F2"));
        }

        texts[4].GetComponent<Text>().text = damaged.ToString();

        rank = GetRanking(score,damaged,difficality);

        string rank_s = "th";
        switch (rank)
        {
            case 0: rank_s = "st"; break;
            case 1: rank_s = "nd"; break;
            case 2: rank_s = "rd"; break;
        }

        texts[6].GetComponent<Text>().text = (rank+1).ToString() + rank_s;

        print(rank);

        // 上位5
        if(rank < 5)
        {
            foreach (GameObject holder in nameHolder)
            {
                holder.SetActive(true);
            }
            state = 2;
            StartCoroutine("InputName");
            // name input visible
            // rank+1;
        }
        else
        {
            StartCoroutine(PlayAnimation(10f, thank));
            SetRanking(rank, score, damaged, "NIL", difficality);
            state = 1;
        }

        panel.SetActive(true);
        //        StartCoroutine(de(2f));
        //        StartCoroutine(PlayAnimation(0.5f, panel));
        int t = 0;
        foreach (GameObject text in texts)
        {
            StartCoroutine(PlayAnimation(timing[t++] - 1f, text));
        }

        //        panel.transform.position = new Vector3(panel.transform.localScale.x, 0);
    }

    [SerializeField]
    string data;

    int GetRanking(int step, float score, int difficality)
    {
        string dir = Application.dataPath + "/LifeOuts" + difficality.ToString() + ".dat";
        FileInfo fi = new FileInfo(dir);
        StreamReader reader = new StreamReader(fi.OpenRead());
        //string data;
        int i = 0;
        while (reader.Peek() > -1)
        {
            data = reader.ReadLine();
            //            print(data.Split(',')[1]);

            if (score < 0)
            {
                if (float.Parse(data.Split(',')[2]) < 0)
                {
                    if (int.Parse(data.Split(',')[1]) > step)
                    {
                        reader.Close();
                        return i;
                    }
                }
            }
            else
            {
                if (float.Parse(data.Split(',')[1]) > step)
                {
                    reader.Close();
                    return i;
                }
            }

            i++;
        }

        reader.Close();
        return i;
    }
    int GetRanking(float score,int damage,int difficulity)
    {
        string dir = Application.dataPath + "/LifeStrange" + difficulity.ToString() + ".dat";
        FileInfo fi = new FileInfo(dir);
        StreamReader reader = new StreamReader(fi.OpenRead());
        //string data;
        int i = 0;
        while(reader.Peek() > -1)
        {
            data = reader.ReadLine();
            //            print(data.Split(',')[1]);

            float s = float.Parse(data.Split(',')[1]);
            if (score < 0)
            {
                if (s < 0)
                {
                    if (int.Parse(data.Split(',')[2]) > damage)
                    {
                        reader.Close();
                        return i;
                    }
                }
            }
            else
            {
                if (s > score)
                {

                    print(score);
                    reader.Close();
                    return i;
                }
            }

            i++;
        }

        reader.Close();
        return i;
    }

    void SetState()
    {
        state = 1;
    }

    void SetRanking(int rank, int step, float score, string name, int difficality)
    {
        string dir = Application.dataPath + "/LifeOuts" + difficality.ToString() + ".dat";
        FileInfo fi = new FileInfo(dir);
        StreamReader reader = new StreamReader(fi.OpenRead());
        string[] datas = new string[101];
        int i = 0;
        datas[rank] = string.Format("{0},{1},{2}", name, step, score);

        while (reader.Peek() > -1)
        {
            if (i == rank)
            {
                i++;
            }
            datas[i] = reader.ReadLine();
            i++;
        }

        reader.Close();

        StreamWriter writer = new StreamWriter(fi.OpenWrite());
        foreach (string line in datas)
        {
            if (line == null) { break; }
            print(line);
            writer.WriteLine(line);
        }
        writer.Close();
    }
    void SetRanking(int rank, float score,int damaged, string name, int difficulty)
    {
        string dir = Application.dataPath + "/LifeStrange" + difficality.ToString() + ".dat";
        FileInfo fi = new FileInfo(dir);
        StreamReader reader = new StreamReader(fi.OpenRead());
        string[] datas = new string[101];
        int i = 0;
        datas[rank] = string.Format("{0},{1},{2}", name, score, damaged);

        while (reader.Peek() > -1)
        {
            if(i == rank)
            {
                i++;
            }
            datas[i] = reader.ReadLine();
            i++;
        }

        reader.Close();

        StreamWriter writer = new StreamWriter(fi.OpenWrite());
        foreach (string line in datas)
        {
            if(line == null) { break; }
            print(line);
            writer.WriteLine(line);
        }
        writer.Close();
    }


    public GameObject[] nameHolder;
    public GameObject[] arrow;
    public int cursor = 0;
    public bool pushH = false,pushV=false;
    public AudioSource audioDecide;
    string[] alphabet = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z, ".Split(',');

    IEnumerator InputName()
    {
        int count = alphabet.Length;
        while (true)
        {
            if (Input.GetAxisRaw("Horizontal") > 0 && cursor < 5 && !pushH)
            {
                audioDecide.Play();
                nameHolder[cursor].GetComponent<Animation>().Stop();
                nameHolder[cursor].GetComponent<Text>().color = new Color(255,255,255,255);
                arrow[cursor].GetComponent<Text>().color = new Color(255, 255, 255, 0);
                cursor++;
                nameHolder[cursor].GetComponent<Animation>().Play();
                pushH = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0 && cursor > 0 && !pushH)
            {
                audioDecide.Play();
                nameHolder[cursor].GetComponent<Animation>().Stop();
                nameHolder[cursor].GetComponent<Text>().color = new Color(255, 255, 255, 255);
                if (cursor < 5)
                {
                    arrow[cursor].GetComponent<Text>().color = new Color(255, 255, 255, 0);
                }
                cursor--;
                nameHolder[cursor].GetComponent<Animation>().Play();
                pushH = true;
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                pushH = false;
            }

            if (cursor < 5)
            {
                if (Input.GetAxisRaw("Vertical") > 0 && !pushV)
                {
                    audioDecide.Play();
                    int index = System.Array.IndexOf(alphabet, nameHolder[cursor].GetComponent<Text>().text);
                    print(index);
                    if (index == alphabet.Length-1) { index = 0; }else
                    {
                        index++;
                    }
                    nameHolder[cursor].GetComponent<Text>().text = alphabet[index];
                    print(alphabet[index]);
                    pushV = true;
                }
                else if (Input.GetAxisRaw("Vertical") < 0 && !pushV)
                {
                    audioDecide.Play();
                    int index = System.Array.IndexOf(alphabet, nameHolder[cursor].GetComponent<Text>().text);
                    print(index);
                    if (index == 0) { index = alphabet.Length-1; }else
                    {
                        index--;
                    }
                    nameHolder[cursor].GetComponent<Text>().text = alphabet[index];
                    print(alphabet[index]);
                    pushV = true;
                }
                else if (Input.GetAxisRaw("Vertical") == 0)
                {
                    pushV = false;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Submit") > 0 && state == 2)
                {
                    audioDecide.Play();
                       name = "";
                    for(int i=0;i<5; i++)
                    {
                        name += nameHolder[i].GetComponent<Text>().text;
                    }
                    Invoke("SetState", 3f);
                    if(lr == 0)
                    {
                        SetRanking(rank, step, score, name ,difficality);
                    }
                    else
                    {
                        SetRanking(rank, score, damaged, name,difficality);
                    }
                    
                    StartCoroutine(PlayAnimation(3f, thank));

                    break;
                }
            }

            yield return null;
        }
    }
    

    // Use this for initialization
    void Start () {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LifeOuts")
        {
            lr = 0;
        }else
        {
            lr = 1;
        }
        state = 0;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetAxisRaw("Submit") > 0 && state == 1)
        {
            fade.SetActive(true);
            Invoke("ReturnTitle", 2.5f);
        }
    }

    void ReturnTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    

    IEnumerator PlayAnimation(float waitTime,GameObject text)
    {
        yield return new WaitForSeconds(waitTime);
        text.GetComponent<Animation>().Play();
    }


    IEnumerator de(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        panel.SetActive(true);
    }


}
