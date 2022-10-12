using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    public Button novaHraButton;
    public Button ulozHruButton;
    public Button nacitajHruButton;
    public Button napovedaButton;
    public Button editorLevelovButton;
    public Button ukonciHruButton;
    public GridManager gd;
    public GameObject menu;
    public InputField input;
    public GameObject saveName;


    // Start is called before the first frame update
    void Start()
    {
        Button novaHraB = novaHraButton.GetComponent<Button>();
        novaHraB.onClick.AddListener(NewGame);

        Button ulozHruB = ulozHruButton.GetComponent<Button>();
        ulozHruB.onClick.AddListener(Save);

        Button nacitajHruB = nacitajHruButton.GetComponent<Button>();
        nacitajHruB.onClick.AddListener(Napoveda);

        Button napovedaB = napovedaButton.GetComponent<Button>();
        napovedaB.onClick.AddListener(Napoveda);

        Button editorLevelovB = editorLevelovButton.GetComponent<Button>();
        editorLevelovB.onClick.AddListener(Continue);

        Button ukonciHruB = ukonciHruButton.GetComponent<Button>();
        ukonciHruB.onClick.AddListener(Napoveda);

        input.onSubmit.AddListener(saveGame);


    }

    public void Napoveda()
    {
        SceneManager.LoadScene("Hint");
    }

    public void NewGame()
    {
        menu.gameObject.SetActive(false);
        gd.NewGame();
    }

    public void Continue() {
        menu.SetActive(false);
    }

    public void Save() {
        saveName.SetActive(true);
    }

    public void saveGame(string text) {
        saveName.SetActive(false);
        gd.Save(text);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
