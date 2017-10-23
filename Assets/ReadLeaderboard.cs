using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ReadLeaderboard : MonoBehaviour {

    public GameObject boxL,boxR;
    public LifeGame lifegame;

    // Use this for initialization
    void Start () {
        dirL = "";
        dirR = "";
//        lifegame.StartCoroutine("LifeGameCoroutine");


    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            boxL.SetActive(true);
            boxR.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine("readCoroutine");
        }
    }

    public Text[] ranks;
    string dirL,dirR;

    public void ReadR()
    {
        dirR = boxR.GetComponent<InputField>().text;
        string strange1 = dirR + "/LifeStrange1.dat";
        string strange2 = dirR + "/LifeStrange2.dat";

        GetRanking(strange1, ranks[3], 3);
        GetRanking(strange2, ranks[4], 3);

        boxR.SetActive(false);
    }

    public void ReadL()
    {
        dirL = boxL.GetComponent<InputField>().text;
        string lifeOuts0 = dirL + "/LifeOuts0.dat";
        string lifeOuts1 = dirL + "/LifeOuts1.dat";
        string lifeOuts2 = dirL + "/LifeOuts2.dat";
        string strange1 = dirR + "/LifeStrange1.dat";
        string strange2 = dirR + "/LifeStrange2.dat";

        GetRanking(lifeOuts0, ranks[0],0);
        GetRanking(lifeOuts1, ranks[1],1);
        GetRanking(lifeOuts2, ranks[2], 2);

        boxL.SetActive(false);
    }

    IEnumerator readCoroutine()
    {
        while (true)
        {
            string lifeOuts0 = dirL + "/LifeOuts0.dat";
            string lifeOuts1 = dirL + "/LifeOuts1.dat";
            string lifeOuts2 = dirL + "/LifeOuts2.dat";
            string strange1 = dirR + "/LifeStrange1.dat";
            string strange2 = dirR + "/LifeStrange2.dat";

            GetRanking(lifeOuts0, ranks[0], 0);
            GetRanking(lifeOuts1, ranks[1], 1);
            GetRanking(lifeOuts2, ranks[2], 2);
            GetRanking(strange1, ranks[3], 3);
            GetRanking(strange2, ranks[4], 3);

            yield return new WaitForSeconds(1f);
        }
    }
    
    [SerializeField]
    float output;

    string[] datas;
    void GetRanking(string dir, Text rank, int no)
    {
        FileInfo fi = new FileInfo(dir);
        StreamReader reader = new StreamReader(fi.OpenRead());
        string data = "";
        string[] datas = new string[101];
        int i = 0;
        
        while (reader.Peek() > -1)
        {
            datas[i] = reader.ReadLine();
            string[] arr = datas[i].Split(',');
            if(no != 3)
            {
                float score = float.Parse(arr[2]);
                switch (i)
                {
                    case 0: 
                        data += string.Format("1st:{0}\n{1}steps  {2:00}:{3}\n\n", arr[0],arr[1], score / 60f, (score % 60f).ToString("F2"));
                        break;
                    case 1:
                        data += string.Format("2nd:{0}\n{1}steps  {2:00}:{3}\n\n", arr[0], arr[1], score / 60f, (score % 60f).ToString("F2"));
                        break;
                    case 2:
                        data += string.Format("3rd:{0}\n{1}steps  {2:00}:{3}\n\n", arr[0], arr[1], score / 60f, (score % 60f).ToString("F2"));
                        break;
                    case 3:
                        data += string.Format("4th:{0}\n{1}steps  {2:00}:{3}\n\n", arr[0], arr[1], score / 60, (score % 60f).ToString("F2"));
                        break;
                    case 4:
                        data += string.Format("5th:{0}\n{1}steps  {2:00}:{3}\n\n", arr[0], arr[1], score / 60, (score % 60f).ToString("F2"));
                        break;
                }
            }
            else
            {
                float score = float.Parse(arr[1]);

                print(score);
                output = score % 60f;
                print(string.Format("{0}", output.ToString("F2")));

                switch (i)
                {
                    case 0:
                        data += string.Format("1st:{0}\n{1:00}:{2}  {3} damaged\n\n", arr[0], score / 60, (score % 60f).ToString("F2"),arr[2]);
                        break;
                    case 1:
                        data += string.Format("2nd:{0}\n{1:00}:{2}  {3} damaged\n\n", arr[0], score / 60, (score % 60f).ToString("F2"), arr[2]);
                        break;
                    case 2:
                        data += string.Format("3rd:{0}\n{1:00}:{2}  {3} damaged\n\n", arr[0], score / 60, (score % 60f).ToString("F2"), arr[2]);
                        break;
                    case 3:
                        data += string.Format("4th:{0}\n{1:00}:{2}  {3} damaged\n\n", arr[0], score / 60, (score % 60f).ToString("F2"), arr[2]);
                        break;
                    case 4:
                        data += string.Format("5th:{0}\n{1:00}:{2}  {3} damaged\n\n", arr[0], score / 60, (score % 60f).ToString("F2"), arr[2]);
                        break;
                }
            }
            //            print(data.Split(',')[1]);
            i++;
        }

        rank.text = data;

        reader.Close();


    }
}
