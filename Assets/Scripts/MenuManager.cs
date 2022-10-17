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
    public Button pokracujButton;
    public GridManager gd;
    public GameObject menu;
    public InputField input;
    public GameObject saveName;
    public GameObject napoveda;
    public GameObject pickSerie;
    public Button serie1;
    public Button serie2;
    public GameObject gameMenu;
    public Color c1, c2;

    // Start is called before the first frame update
    void Start()
    {
        Button novaHraB = novaHraButton.GetComponent<Button>();
        novaHraB.onClick.AddListener(SelectSerie);

        Button ulozHruB = ulozHruButton.GetComponent<Button>();
        ulozHruB.onClick.AddListener(SaveMenu);

        Button nacitajHruB = nacitajHruButton.GetComponent<Button>();
        nacitajHruB.onClick.AddListener(Load);

        Button napovedaB = napovedaButton.GetComponent<Button>();
        napovedaB.onClick.AddListener(Napoveda);

        Button editorLevelovB = editorLevelovButton.GetComponent<Button>();
        editorLevelovB.onClick.AddListener(Continue);

        Button ukonciHruB = ukonciHruButton.GetComponent<Button>();
        ukonciHruB.onClick.AddListener(Quit);

        Button pokracujB = pokracujButton.GetComponent<Button>();
        pokracujB.onClick.AddListener(Continue);

        Button serie1B = serie1.GetComponent<Button>();
        serie1B.onClick.AddListener(Serie1);

        Button serie2B = serie2.GetComponent<Button>();
        serie2B.onClick.AddListener(Serie2);

        input.onSubmit.AddListener(saveGame);


    }

    public void Napoveda()
    {
        menu.SetActive(false);
        napoveda.SetActive(true);
    }

    public void Serie1() {
        gameMenu.GetComponent<Image>().color = c1;
        
        pickSerie.SetActive(false);
        NewGame(1);
    }

    public void Serie2() {
        gameMenu.GetComponent<Image>().color = c2;

        pickSerie.SetActive(false);
        NewGame(2);
    }

    public void SelectSerie() {
        pickSerie.SetActive(true);
    }
    public void NewGame(int sada)
    {
        if (!gd.saved)
        {
            if (EditorUtility.DisplayDialog("Neuloûen· hra", "Naozaj chcete zahodiù neuloûen˙ hru?", "ZahoÔiù", "Zruöiù"))
            {
                menu.gameObject.SetActive(false);
                gd.NewGame(sada);
            }
        }
        else {
            menu.gameObject.SetActive(false);
            gd.NewGame(sada);
        }
    }

    public void Continue() {

            menu.SetActive(false);

    }

    public void SaveMenu() {
        if (gd.mapa.Count != 0)
        {
            saveName.SetActive(true);
        }
    }

    public void Load() {
        if (!gd.saved)
        {
            if (EditorUtility.DisplayDialog("Neuloûen· hra", "Naozaj chcete zahodiù neuloûen˙ hru?", "ZahoÔiù", "Zruöiù"))
            {
                menu.gameObject.SetActive(false);
                gd.loadSave();
            }
        }
        else {
            menu.gameObject.SetActive(false);
            gd.loadSave();
        }
    }
    public void saveGame(string text) {
        if (gd.mapa.Count != 0)
        {
            saveName.SetActive(false);
            gd.Save(text);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Quit() {
        Application.Quit();
    }
}
