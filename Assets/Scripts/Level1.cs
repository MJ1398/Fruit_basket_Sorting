using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1 : MonoBehaviour
{
    public GameObject apple, RedBasket, GameComplete, GameOver, Gameplay;

    Vector2 appleInitialPos;

    public AudioSource source;
    public AudioClip[] correct;
    public AudioClip incorrect;

    bool appleCorrect = false;

    // Start is called before the first frame update
    void Start()
    {
        appleInitialPos = apple.transform.position;
        ScoreManager.scoreCount = 0;
        GameComplete.SetActive(false);
        GameOver.SetActive(false);
        Gameplay.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //if (appleCorrect && bananaCorrect && ScoreManager.scoreCount >= 20)
        //{
        //    //debug.log("you win");
        //    GameComplete.SetActive(true);
        //    Invoke("LevelComplete", 3f);

        //}
        //else
        //{
        //    GameComplete.SetActive(false);
        //}
    }

    public void DragApple()
    {
        if (Input.GetMouseButton(0))
        {
            apple.transform.position = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            // Touch input
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                apple.transform.position = touch.position;
            }
        }
    }    

    //Function for apple
    public void DropApple()
    {
        float Distance = Vector3.Distance(apple.transform.position, RedBasket.transform.position);
        if (Distance < 50)
        {
            apple.transform.position = RedBasket.transform.position;
            source.clip = correct[Random.Range(0, correct.Length)];
            source.Play();
            appleCorrect = true;
            ScoreManager.scoreCount += 10;
        }
        else
        {
            if (Distance < 60)
            {
                apple.transform.position = appleInitialPos;
                source.clip = incorrect;
                source.Play();
                ScoreManager.scoreCount -= 5;
            }
            else
            {
                apple.transform.position = appleInitialPos;
                source.clip = incorrect;
                source.Play();
                ScoreManager.scoreCount = 0;
            }
            
        }

        CheckLevelCompletion();
    }    

    void CheckLevelCompletion()
    {
        if (appleCorrect && ScoreManager.scoreCount >= 10)
        {
            GameComplete.SetActive(true);
            Invoke("LevelComplete", 3f);
        }
    }

    //When level completes this function sends the player to next level
    void LevelComplete()
    {         
        UnlockNewLevel();

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; //loads the next build scene
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            GameOver.SetActive(true);
            Gameplay.SetActive(false);
            GameComplete.SetActive(false);
            // Handle the case when there are no more scenes to load
            //Debug.Log("No more levels to load. You have completed all levels!");
            Invoke("HomeScreen", 4f);
        }
    }    

    //unlocks new level
    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel",  1)+ 1);
            PlayerPrefs.Save();
        }
    }

    //sends player to home screen
    public void HomeScreen()
    {
        SceneManager.LoadScene(0);
    }
}
