using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CollectibleCount : MonoBehaviour
{
    TMP_Text text;
    public WinLose winLoseScript;

    public int totalLogs = 20; // 👈 fixed total set to 20

    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    void Start()
    {
        UpdateCount();
    }

    void OnEnable()  => Collectible.OnCollected += OnCollectibleCollected;
    void OnDisable() => Collectible.OnCollected -= OnCollectibleCollected;

    void OnCollectibleCollected()
    {
        Caminteract.count++;  
        UpdateCount();

        if (Caminteract.count >= totalLogs) // 👈 match the goal
        {
            SceneManager.LoadScene("GameWin");
        }
    }

    void UpdateCount()
    {
        text.text = $"{Caminteract.count} / {totalLogs}";
    }
}