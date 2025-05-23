using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject settingsPopup;
    public Slider illuminationSlider;

    [Header("Loading")]
    public GameObject loadingScreen;

    [Header("Togglers")]
    public GameObject toggleObject;
    public string[] scenesList;

    [Header("Mushroom Scene")]
    public Toggle mushroomModeToggle;

    private List<GameObject> togglers;

    static List<string> m_HistoryList = new List<string>();

    private void Start()
    {
        if (illuminationSlider) illuminationSlider.value = PrefsHandler.butterflyIlluminationStrength;
        
        if (toggleObject)
        {
            togglers = new List<GameObject>();
            toggleObject.GetComponent<MenuSceneDayNightToggleHandler>().Setup(scenesList[0]);
            togglers.Add(toggleObject);
            for (int i = 1; i < scenesList.Length; i++)
            {
                var obj = Instantiate(toggleObject, toggleObject.transform.parent) as GameObject;
                obj.GetComponent<MenuSceneDayNightToggleHandler>().Setup(scenesList[i]);
                togglers.Add(obj);
            }
        }

        mushroomModeToggle.isOn = PlayerPrefs.GetInt("IsMushroomModeDark", 1) == 1;
    }

    public void MushroomSceneToggleTap(bool val)
    {
        PlayerPrefs.SetInt("IsMushroomModeDark", mushroomModeToggle.isOn ? 1 : 0);
    }

    public void OpenSettings()
    {
        settingsPopup.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        m_HistoryList.Add(SceneManager.GetActiveScene().name);
        loadingScreen.SetActive(true);
        PrefsHandler.butterflyIlluminationStrength = Mathf.RoundToInt(illuminationSlider.value);
        SaveDayNightToggleData();
        SceneManager.LoadScene(sceneName);
    }

    private void SaveDayNightToggleData()
    {
        foreach (var item in togglers)
        {
            item.GetComponent<MenuSceneDayNightToggleHandler>().SaveData();
        }
    }

    public void Back()
    {
        // go back to the previous scene
        if (m_HistoryList.Count > 0)
        {
            string sceneName = m_HistoryList[m_HistoryList.Count - 1];
            m_HistoryList.RemoveAt(m_HistoryList.Count - 1);
            SceneManager.LoadScene(sceneName);
        }
    }
}