using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalBoxs;
    public int finishedBoxs;
    public static GameManager instance;
    private Stack<ICommand> commandStack = new Stack<ICommand>();

    public GameObject difficultyPanel;
    public Button goodButton;
    public Button badButton;
    private int boxCount = 1; 
    private int wallCount = 10;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }

    void Start()
    {
        goodButton.onClick.AddListener(IncreaseDifficulty);
        badButton.onClick.AddListener(DecreaseDifficulty);
        difficultyPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            difficultyPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
            ResetStage();
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.instance.Undo();
        }

    }
    void IncreaseDifficulty()
    {
        boxCount++;
        wallCount += 5;
        ReloadScene();
    }

    void DecreaseDifficulty()
    {
        if (boxCount > 1)
        {
            boxCount--;
        }
        if (wallCount > 10)
        {
            wallCount -= 5;
        }
        ReloadScene();
    }
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandStack.Push(command);
    }

    public void Undo()
    {
        if (commandStack.Count > 0)
        {
            var command = commandStack.Pop();
            command.Undo();
        }
    }


    public void CheckFinish()
    {
        if(finishedBoxs == totalBoxs)
        {
            print("Next LeveL!");
            StartCoroutine(LoadNextStage());
        }
    }

    void ResetStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LoadNextStage()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("BoxCount", boxCount);
        PlayerPrefs.SetInt("WallCount", wallCount);
    }  

}
