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

    public GameObject neulozenaPanel;
    public GameObject neulozenaHraPanel;

    public Button neulozenaPokracovatButton;
    public Button neulozenaSpatButton;
    private List<List<char>> mapaCopy;
    public Button neulozenaHraPokracovatButton;
    public Button neulozenaHraSpatButton;

    public bool newGameBool = false;

    public Button stopPlayButton;
    public Button playButton;
    public Button kruznicaButton;

    public GameObject loadPanel;
    public Button potvrditButton;

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

        Button neulozenaPokracovatBtn = neulozenaPokracovatButton.GetComponent<Button>();
        neulozenaPokracovatBtn.onClick.AddListener(PB);

        Button neulozenaSpatBtn = neulozenaSpatButton.GetComponent<Button>();
        neulozenaSpatBtn.onClick.AddListener(SB);


        Button neulozenaHraPokracovatBtn = neulozenaHraPokracovatButton.GetComponent<Button>();
        neulozenaHraPokracovatBtn.onClick.AddListener(HraPokracovatButton);

        Button neulozenaHraSpatBtn = neulozenaHraSpatButton.GetComponent<Button>();
        neulozenaHraSpatBtn.onClick.AddListener(HraSpatButton);

        Button potvrditBtn = potvrditButton.GetComponent<Button>();
        potvrditBtn.onClick.AddListener(LoadGo);

        input.onSubmit.AddListener(saveGame);
        rowInput.onSubmit.AddListener(ChooseInput);
        columnInput.onSubmit.AddListener(ChooseInput);


        }

        

    private void HraSpatButton()
    {
        neulozenaHraPanel.SetActive(false);

    }

    private void HraPokracovatButton()
    {
        if (newGameBool)
        {
            pickSerie.SetActive(true);
            neulozenaHraPanel.SetActive(false);
        }
        else
        {
            playPanel.SetActive(false);
            menu.SetActive(false);
            gameMenu.SetActive(false);
            ePanel.SetActive(true);
            gd.clear();
            neulozenaHraPanel.SetActive(false);
        }
        
    }

    private void SB()
    {
        pickSerie.SetActive(false);
        neulozenaPanel.SetActive(false);
    
    }
    
    private void PB()
    {
        if (newGameBool)
        {
            pickSerie.SetActive(true);
            neulozenaPanel.SetActive(false);
        }
        else
        {
            playPanel.SetActive(false);
            menu.SetActive(false);
            gameMenu.SetActive(false);
            ePanel.SetActive(true);
            gd.clear();
            neulozenaPanel.SetActive(false);
        }
        
    }

    private void SpatButton()
    {
        neulozenaPanel.SetActive(false);
        pickSerie.SetActive(false);

    }

    private void PokracovatButton()
    {
        playPanel.SetActive(false);
        menu.SetActive(false);
        gameMenu.SetActive(false);
        ePanel.SetActive(true);
        gd.clear();
        neulozenaPanel.SetActive(false);
    }

    private void StopPlayKlik()
    {
        playPanel.SetActive(false);
        gd.editorGame = true;
        ePanel.SetActive(true);
        gd.clear();
        gd.mapa = copyList(mapaCopy);
        gd.VytvorGrid();
        gd.pauza = false;
    }

    private void playKlik()
    {
        if (!gd.wizi) { return; }
        playPanel.SetActive(true);
        gd.editorGame = false;
        mapaCopy = copyList(gd.mapa);
        gd.clearTiles();
        gd.VytvorGrid();
        gd.pauza = true;
    }

    private List<List<char>> copyList(List<List<char>> list) {
        List<List<char>> ret = new List<List<char>>();

        foreach (List<char> lch in list) {
            var l = new List<char>();
            foreach (char ch in lch) {
                l.Add(ch);
            }
            ret.Add(l);
        }
        return ret;
    }

    private void KruznicaKlik()
    {
        if (editorKruznica)
        {
            editorKruznica = false;
            kruznicaButton.GetComponentInChildren<Text>().text = "çah";
        }
        else
        {
            editorKruznica = true;
            kruznicaButton.GetComponentInChildren<Text>().text = "Kruznica";
        }
    }

    public void Editor()
    {

        if (gd.mapa.Count < 1)
        {
            playPanel.SetActive(false);
            menu.SetActive(false);
            gameMenu.SetActive(false);
            ePanel.SetActive(true);
            gd.clear();
            gd.wizi = false;
            gd.pauza = false;

        }
        else
        {
            if (gd.editorGame)
            {
                neulozenaPanel.SetActive(true); 
            }
            else
            {
                neulozenaHraPanel.SetActive(true);
            } 

        }
        newGameBool = false;
    }
    public void Choose() {
        gd.menuButton.interactable = false;
        ePanel.SetActive(false);
        gd.menuButton.interactable = false;
        ePanel.SetActive(false);
        gd.pauza = true;
        choosePanel.SetActive(true);
    }

    public void ChooseInput(string text) {
        if ((rowInput.text != "") & (columnInput.text != "")) {
            gd.NacitajEditor(int.Parse(rowInput.text), int.Parse(columnInput.text));
            choosePanel.SetActive(false);
            ePanel.SetActive(true);
            gd.pauza = false;
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
        gd.nemaRieseniePanel.GetComponent<Image>().color = c1;
        gd.nemasPravduPanel.GetComponent<Image>().color = c1;

        pickSerie.SetActive(false);
        endMenu.GetComponent<Image>().color = c1;
        NewGame(1);
    }

    public void Serie2() {
        gameMenu.GetComponent<Image>().color = c2;
        endMenu.GetComponent<Image>().color = c2;
        gd.nemaRieseniePanel.GetComponent<Image>().color = c2;
        gd.nemasPravduPanel.GetComponent<Image>().color = c2;

        pickSerie.SetActive(false);

        NewGame(2);
    }

    public void SelectSerie() {


        /*   if (!gd.saved)
           {
               if (EditorUtility.DisplayDialog("Neuloûen· hra", "Naozaj chcete zahodiù neuloûen˙ mapu?", "ZahoÔiù", "Zruöiù"))
               {
                   pickSerie.SetActive(true);
               }
           }
           else
           {
               pickSerie.SetActive(true);
           }*/

        if (gd.mapa.Count < 1)
        {
            pickSerie.SetActive(true);
        }
        else
        {
            if (gd.editorGame)
            {
                neulozenaPanel.SetActive(true);              
            }
            else
            {
                neulozenaHraPanel.SetActive(true);
               
            }
        }
        newGameBool = true;


    }
    public void NewGame(int sada)
    {
        if (gd.saved)
        {
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
            if (!gd.wizi) { return; }
            saveName.SetActive(true);
        }
    }

    public void Load() {
        
            loadPanel.SetActive(true);
            gd.dialogWindow();
    }

    private void LoadGo() {
        menu.gameObject.SetActive(false);
        loadPanel.SetActive(false);
        gd.loadSave();
        mapaCopy = copyList(gd.mapa);
        playPanel.SetActive(true);
        gd.editorGame = false;

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
