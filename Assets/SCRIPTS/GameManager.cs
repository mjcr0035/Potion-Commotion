using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //matthew:
    //stores important player and scene information

    [Header("Level Select")]
    public bool levelOneSelected;
    public bool levelTwoSelected;
    public bool levelThreeSelected;
    public bool endlessSelected;
    public string sceneName;
    public GameObject levelsObject;

    [Header("Gold Collected")]
    public int totalGold;
    public int levelOneGold;
    public int levelTwoGold;
    public int levelThreeGold;
    public int endlessGold;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        levelOneSelected = false;
        levelTwoSelected = false;
        levelThreeSelected = false;
    }
    // Update is called once per frame
    void Update()
    {
        //update gold values from levels here
    }

    public void ShowLevels()
    {
        levelsObject.SetActive(true);
    }

    //my fault this kinda sux but it works
    public void levelOne()
    {
        levelOneSelected = true;
        SceneManager.LoadScene(sceneName);
    }

    public void levelTwo()
    {
        levelTwoSelected = true;
        SceneManager.LoadScene(sceneName);
    }
    public void levelThree()
    {
        levelThreeSelected = true;
        SceneManager.LoadScene(sceneName);
    }
    public void endless()
    {
        endlessSelected = true;
        SceneManager.LoadScene(sceneName);
    }
}
