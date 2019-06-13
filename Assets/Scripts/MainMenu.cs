using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool ratingIsReaded = false;
    [SerializeField] GameObject ratingTemp;
    class record
    {
        public string date;
        public string name;
        public int points;
        public record(string date,string name, int points)
        {
            this.date = date;
            this.name = name;
            this.points = points;
        }
    };
    public void PlayGame()
    {
        SceneManager.LoadScene("Mrowisko");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void readRating()
    {
        if(!ratingIsReaded)
        { 
            List<record> records = new List<record>();
            string path = Application.persistentDataPath + "/rating.dat";
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] cells = line.Split('-');
                record rec = new record(cells[0],cells[1], System.Int32.Parse(cells[2]));
                records.Add(rec);
            }
            records.Sort((x, y) => -1*x.points.CompareTo(y.points));
            int n = 0;
            foreach(record r in records)
            {
                GameObject uiRecord = Instantiate(ratingTemp);
                uiRecord.transform.parent = GameObject.Find("Rating").transform;
                uiRecord.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, n*-32, 0);
                uiRecord.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 32);
                uiRecord.transform.Find("DateText").GetComponent<TextMeshProUGUI>().text = r.date;
                uiRecord.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = r.name;
                uiRecord.transform.Find("PointsText").GetComponent<TextMeshProUGUI>().text = r.points.ToString();
                n++;
            }
            ratingIsReaded = true;
        }
    }
    public void ChangeMusicVolume(float volume)
    {

    }
}
