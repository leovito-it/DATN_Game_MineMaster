using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject scrollView;
    public string[] scenes;
    public GameObject levelPrefab;
    public Transform content;

    public void ResetPlayPrefs()
    {
        PlayerPrefs.DeleteKey(DEFINE.KEY_UNLOCK_LEVEL);
        PlayerPrefs.DeleteKey(DEFINE.KEY_CLEAR_LEVEL);
    }

    private void Start()
    {
        for (int i=0; i < scenes.Length; i++)
        {
            int index = i;
            GameObject lv = Instantiate(levelPrefab, content);
            lv.transform.GetChild(0).GetComponent<Text>().text = scenes[index];

            Button btn = lv.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                DEFINE.LoadScene(scenes[index]);
            });
        }
    }

    private void OnGUI()
    {
        int unlock = PlayerPrefs.GetInt(DEFINE.KEY_UNLOCK_LEVEL, 0);
        int clear = PlayerPrefs.GetInt(DEFINE.KEY_CLEAR_LEVEL, -1);
        Debug.Log(unlock + "_" + clear);

        for (int i = 0; i < content.childCount; i++)
        {
            int index = i;
            Transform lv = content.GetChild(index);

            lv.GetChild(1).gameObject.SetActive(index > unlock);
            lv.GetChild(2).gameObject.SetActive(index < clear);

            Button btn = lv.GetComponent<Button>();
            btn.enabled = index <= unlock && index >= clear;
        }
    }

    public void Prev()
    {

    }

    public void Next()
    {

    }
}
