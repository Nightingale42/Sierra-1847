using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    public int count;
    public WinLose winLoseScript;

    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    void Start() => UpdateCount();

    void OnEnable() => Collectible.OnCollected += OnCollectibleCollected;
    void onDisable()=> Collectible.OnCollected -= OnCollectibleCollected;

    void OnCollectibleCollected()
    {
        ++count;
        UpdateCount();

        if (count > 6)
        {
            Debug.Log("you winny");
            SceneManager.LoadScene("GameWin");

        }


    }

    void UpdateCount()
    {
        text.text = $"{count} / {Collectible.total}";
    }
}

