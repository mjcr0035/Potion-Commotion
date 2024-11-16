using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public bool tutorialSelected;

    public GameObject levelsObject;

    [Header("Gold Collected")]
    public int totalGold;
    public int levelOneGold;
    public int levelTwoGold;
    public int levelThreeGold;
    public int endlessGold;

  

    public static GameObject sampleInstance;



    private void Awake()
    {
        if (sampleInstance != null)
        {
            Destroy(sampleInstance);
        }
        sampleInstance = gameObject;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        levelOneSelected = false;
        levelTwoSelected = false;
        levelThreeSelected = false;
        endlessSelected = false;
        tutorialSelected = false;
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
        levelTwoSelected = false;
        levelThreeSelected = false;
        endlessSelected = false;
        tutorialSelected = false;
        SceneManager.LoadScene(1);
    }
    public void levelTwo()
    {
        levelOneSelected = false;
        levelTwoSelected = true;
        levelThreeSelected = false;
        endlessSelected = false;
        tutorialSelected = false;
        SceneManager.LoadScene(1);
    }
    public void levelThree()
    {
        levelOneSelected = false;
        levelTwoSelected = false;
        levelThreeSelected = true;
        endlessSelected = false;
        tutorialSelected = false;
        SceneManager.LoadScene(1);
    }
    public void endless()
    {
        levelOneSelected = false;
        levelTwoSelected = false;
        levelThreeSelected = false;
        endlessSelected = true;
        tutorialSelected = false;
        SceneManager.LoadScene(1);
    }
    public void tutorial()
    {
        levelOneSelected = false;
        levelTwoSelected = false;
        levelThreeSelected = false;
        endlessSelected = false;
        tutorialSelected = true;
        SceneManager.LoadScene(1);
    }
    public void mainMenu()
    {
        levelOneSelected = false;
        levelTwoSelected = false;
        levelThreeSelected = false;
        endlessSelected = false;
        tutorialSelected = false;
        SceneManager.LoadScene(0);
    }
}
