using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class D2CollectibleCount : MonoBehaviour
{
    TMP_Text text;
    public WinLose winLoseScript;

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
        D2Caminteract.count++;   // update the REAL count
        UpdateCount();

        if (D2Caminteract.count > 6)
        {
            SceneManager.LoadScene("GameWin");
        }
    }

    void UpdateCount()
    {
        text.text = $"{D2Caminteract.count} / {Collectible.total}";
    }
}
