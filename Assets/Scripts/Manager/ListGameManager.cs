using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct GameInfo
{
    public string sceneName;
    public string displayNameVn, displayNameEn;
    public Sprite icon;
}

public class ListGameManager : Singleton<ListGameManager>
{
    public GameInfo[] games;

    public GameObject pagePrefab;
    public GameObject gamePrefab;

    int installed = 0;
    [SerializeField] int numGamePerPage = 2;

    private void Start()
    {
        installed = 0;
        PageManager.Instance.numPage = games.Length / numGamePerPage;

        if (games.Length % numGamePerPage > 0)
            PageManager.Instance.numPage++;

        PageManager.Instance.InitStep();

        for (int i = 0; i < PageManager.Instance.numPage; i++)
        {
            InitPage();
        }
    }

    void InitPage()
    {
        if (games.Length <= installed)
            return;

        GameObject page =  Instantiate(pagePrefab, PageManager.Instance.ScrollContent);

        for ( int i = 0; i < numGamePerPage; i++)
        {
            GameObject newGame = Instantiate(gamePrefab, page.transform);
            newGame.name = games[installed].sceneName;

            newGame.transform.Find("icon").GetComponent<Image>().sprite = games[installed].icon;
            LanguageManager languageManager = newGame.transform.Find("name").GetComponent<LanguageManager>();

            languageManager.vietnamese = games[installed].displayNameVn;
            languageManager.english = games[installed].displayNameEn;
            newGame.GetComponent<Button>().onClick.AddListener ( () => { MySceneManager.LoadScene(newGame.name); });

            installed++;
        }

        page.SetActive(true);
    }
}
