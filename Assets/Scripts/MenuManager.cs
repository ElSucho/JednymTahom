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
    public GameObject ePanel;
    public Button chooseButton;
    public GameObject choosePanel;
    public InputField rowInput;
    public InputField columnInput;
    public GameObject endMenu;
    private bool editorKruznica = true;
    public GameObject playPanel;

    public Button stopPlayButton;
    public Button playButton;
    public Button kruznicaButton;


   

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
        editorLevelovB.onClick.AddListener(Editor);
        Button ukonciHruB = ukonciHruButton.GetComponent<Button>();
        ukonciHruB.onClick.AddListener(Quit);
        Button pokracujB = pokracujButton.GetComponent<Button>();
        pokracujB.onClick.AddListener(Continue);
        Button serie1B = serie1.GetComponent<Button>();
        serie1B.onClick.AddListener(Serie1);
        Button serie2B = serie2.GetComponent<Button>();
        serie2B.onClick.AddListener(Serie2);
        Button chooseB = chooseButton.GetComponent<Button>();
        chooseB.onClick.AddListener(Choose);

        Button playBtn = playButton.GetComponent<Button>();
        playBtn.onClick.AddListener(playKlik);

        Button stopBtn = stopPlayButton.GetComponent<Button>();
        stopBtn.onClick.AddListener(StopPlayKlik);

        Button kruznicaBtn = kruznicaButton.GetComponent<Button>();
        kruznicaBtn.onClick.AddListener(KruznicaKlik);


        input.onSubmit.AddListener(saveGame);
        rowInput.onSubmit.AddListener(ChooseInput);
        columnInput.onSubmit.AddListener(ChooseInput);


    }

    private void StopPlayKlik()
    {
        playPanel.SetActive(false);
    }

    private void playKlik()
    {
        
        
        playPanel.SetActive(true);
        
        
    }

    private void KruznicaKlik()
    {
        if (editorKruznica)
        {
            editorKruznica = false;
            kruznicaButton.GetComponentInChildren<Text>().text = "�ah";
        }
        else
        {
            editorKruznica = true;
            kruznicaButton.GetComponentInChildren<Text>().text = "Kruznica";
        }
    }

    public void Editor() {

        if (gd.mapa.Count < 1)
        {
            playPanel.SetActive(false);
            menu.SetActive(false);
            gameMenu.SetActive(false);
            ePanel.SetActive(true);
            gd.clear();


        }
        else
        {
            if (EditorUtility.DisplayDialog("Neulo�en� hra", "Naozaj chcete zahodi� neulo�en� mapu?", "Zaho�i�", "Zru�i�"))
            {
                playPanel.SetActive(false);
                menu.SetActive(false);
                gameMenu.SetActive(false);
                ePanel.SetActive(true);
                gd.clear();
            }
            else
            {
                
            }
        }

        


    }

    public void Choose() {
        gd.menuButton.interactable = false;
        ePanel.SetActive(false);
        choosePanel.SetActive(true);
    }

    public void ChooseInput(string text) {
        if ((rowInput.text != "") & (columnInput.text != "")) {
            gd.NacitajEditor(int.Parse(rowInput.text), int.Parse(columnInput.text));
            choosePanel.SetActive(false);
            ePanel.SetActive(true);
            gd.menuButton.interactable = true;
        }
    }

    public void Napoveda()
    {
        menu.SetActive(false);
        napoveda.SetActive(true);
    }

    public void Serie1() {

        gameMenu.GetComponent<Image>().color = c1;
        
        pickSerie.SetActive(false);
        endMenu.GetComponent<Image>().color = c1;
        NewGame(1);
    }

    public void Serie2() {
        gameMenu.GetComponent<Image>().color = c2;
        endMenu.GetComponent<Image>().color = c2;

        pickSerie.SetActive(false);

        NewGame(2);
    }

    public void SelectSerie() {

        if (!gd.saved)
        {
            if (EditorUtility.DisplayDialog("Neulo�en� hra", "Naozaj chcete zahodi� neulo�en� mapu?", "Zaho�i�", "Zru�i�"))
            {
                pickSerie.SetActive(true);
            }
        }
        else
        {
            pickSerie.SetActive(true);
        }

        if (gd.mapa.Count > 0)
        {
            if (EditorUtility.DisplayDialog("Neulo�en� hra", "Naozaj chcete zahodi� neulo�en� mapu?", "Zaho�i�", "Zru�i�"))
            {
                pickSerie.SetActive(true);
            }
            else
            {
                pickSerie.SetActive(false);
            }
        }

    }
    public void NewGame(int sada)
    {
        if (!gd.saved)
        {
            if (EditorUtility.DisplayDialog("Neulo�en� hra", "Naozaj chcete zahodi� neulo�en� hru?", "Zaho�i�", "Zru�i�"))
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
        if (gd.mapa.Count != 0 & gd.editorGame)
        {
            saveName.SetActive(true);
        }
    }

    public void Load() {
        if (!gd.saved)
        {
            if (EditorUtility.DisplayDialog("Neulo�en� hra", "Naozaj chcete zahodi� neulo�en� hru?", "Zaho�i�", "Zru�i�"))
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
            gd.Save(text, editorKruznica);
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
