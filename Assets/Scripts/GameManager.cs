using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TMP_Text scoreTxt;

    public int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        scoreTxt.text = score.ToString();
    }

    public void AddScore(int addition)
    {
        score += addition;
        scoreTxt.text = score.ToString();
    }
}