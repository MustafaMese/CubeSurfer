using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance { get => instance; }

    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject defeatPanel;

    private int collectable;
    private bool touched;

    private void Awake()
    {
        instance = this;
        touched = false;
    }

    public void Initialize()
    {
        if(!touched)
        {
            touched = true;
            GameManager.Instance.canStart = true;
            startPanel.SetActive(false);
        }
    }

    public IEnumerator OpenDefeatPanel()
    {
        defeatPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
