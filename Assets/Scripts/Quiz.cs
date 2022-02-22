using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colours")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    Color32 correctColour = new Color32 (0,255,0,255);
    Color32 incorrectColour = new Color32 (255,0,0,255);

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;

    [Header("Audio Clips")]
    AudioSource correctAudio;
    AudioSource incorrectAudio;

    public bool isComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        AudioSource[] audios = GetComponents<AudioSource>();
        correctAudio = audios[1];
        incorrectAudio = audios[2];
        progressBar.maxValue = questions.Count;     
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuesion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayCorrectAnswer(-1); //-1 common practice for out of range exception
            SetButtonState(false);
        }
    }

    void DisplayCorrectAnswer(int index)
    {
        Image correctButtonImage;
        Image selectedButtonImage;

        if(index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            correctButtonImage = answerButtons[index].GetComponent<Image>();
            // buttonImage.sprite = correctAnswerSprite;
            correctButtonImage.color = correctColour;
            scoreKeeper.IncrementCorrectAnswers();
            correctAudio.Play();
        }
        else
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, The correct answer is:\n " + correctAnswer;

            correctButtonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            // buttonImage.sprite = correctAnswerSprite;
            correctButtonImage.color = correctColour;
            incorrectAudio.Play(); 

            if (index > -1)
            {
                selectedButtonImage = answerButtons[index].GetComponent<Image>();
                selectedButtonImage.color = incorrectColour; 
            }
            
        }
        scoreText.text = "Score: " + scoreKeeper.CalculateScore();
        
    }

    public void OnAnswerSelected(int index) 
    {
        hasAnsweredEarly = true;
        DisplayCorrectAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        
    }

    void GetNextQuesion()
    {
        if(questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        
        if(questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }

    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        
        for(int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
        scoreKeeper.IncrementQuestionsSeen();
    }

    void SetDefaultButtonSprites()
    {
        for(int i = 0; i < answerButtons.Length;i++)
        {
            Color32 defaulColour = new Color32 (255,255,255,255);
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.color = defaulColour;
        }
    }

    void SetButtonState(bool state)
    {
       for(int i = 0; i < answerButtons.Length;i++)
       {
           Button button = answerButtons[i].GetComponent<Button>();
           button.interactable = state;
       } 
    }
}