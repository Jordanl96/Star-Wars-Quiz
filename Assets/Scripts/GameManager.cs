using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioClip replayAudio;

    StartScreen startScreen;
    Quiz quiz;
    EndScreen endScreen;
    
    void Awake()
    {
        startScreen = FindObjectOfType<StartScreen>();
        quiz = FindObjectOfType<Quiz>();
        endScreen = FindObjectOfType<EndScreen>();
    }

    void Start()
    {
        startScreen.gameObject.SetActive(true);
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        if(quiz.isComplete)
        {
            quiz.gameObject.SetActive(false);
            endScreen.gameObject.SetActive(true);
            endScreen.DisplayFinalScore();
        }
    }

    public void OnReplayLevel()
    {
        StartCoroutine(Waiter());
    }

    public void OnStartGame()
    {
        startScreen.hasStarted = true;
        startScreen.gameObject.SetActive(false);
        quiz.gameObject.SetActive(true);
    }

    IEnumerator Waiter()
    {
        GetComponent<AudioSource>().PlayOneShot(replayAudio);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
