using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;
using System;

public class GridManager : MonoBehaviour
{
    public int _width, _height;
    public Tile _prefab;
    public LineRenderer LineRenderer;
    public Transform _cam;
    public List<List<char>> mapa;
    private Dictionary<Vector2, Tile> _tiles;
    private int actualX, actualY;
    private Tile actualTile;
    private List<Pair> kroky;
    public Button backButton;
    public Button menuButton;
    public Button continueButton;
    public Button next;
    public Button odznova;
    public Button endNext;
    public Button previous;
    public GameObject ePanel;
    public GameObject menu;
    public GameObject menuPanel;
    public GameObject endMenu;
    private int levelNumber = 1;
    private List<Vector2> v0 = new List<Vector2>();
    private List<Vector2> v1 = new List<Vector2>();
    private List<Vector2> vT = new List<Vector2>();
    public bool saved = true;
    public GameObject hint;
    private string path;
    private int _serie;
    public Text actualLevel;
    public Text levelType;
    private bool solve;
    public bool editorGame = false;
    private bool gameOver = false;
    public Button treeButton;
    public Button playerButton;
    public Button tileButton;
    public Button riesenieButton;
    private char editorTileChoosen = '0';
    private bool wizi;
    private bool klik = true;

    public Button ukazRiesenieButton;
    public GameObject sada1level1;
    public GameObject sada1level2;
    public GameObject sada1level3;
    public GameObject sada2level3;

    public GameObject editorPanel;


    // Start is called before the first frame update
    void Start()
    {
        Tile.OnSelectedEvent += SelectAction;
        mapa = new List<List<char>>();
        kroky = new List<Pair>();

        Button bckBtn = backButton.GetComponent<Button>();
        bckBtn.onClick.AddListener(back);
        Button menuBtn = menuButton.GetComponent<Button>();
        menuBtn.onClick.AddListener(showMenu);
        Button nextBtn = next.GetComponent<Button>();
        nextBtn.onClick.AddListener(nextGame);
        Button previousBtn = previous.GetComponent<Button>();
        previousBtn.onClick.AddListener(previousGame);
        Button odznovaBtn = odznova.GetComponent<Button>();
        odznovaBtn.onClick.AddListener(() => NewGame(_serie, levelNumber));
        Button endNextBtn = endNext.GetComponent<Button>();
        endNextBtn.onClick.AddListener(nextGame);

        Button treeBtn = treeButton.GetComponent<Button>();
        treeBtn.onClick.AddListener(() => editorChooseTile('s'));
        Button playerBtn = playerButton.GetComponent<Button>();
        playerBtn.onClick.AddListener(() => editorChooseTile('z'));
        Button tileBtn = tileButton.GetComponent<Button>();
        tileBtn.onClick.AddListener(() => editorChooseTile('.'));

        Button riesenieBtn = riesenieButton.GetComponent<Button>();
        riesenieBtn.onClick.AddListener(RiesenieCheck);

        Button ukazRiesenieBtn = ukazRiesenieButton.GetComponent<Button>();
        ukazRiesenieBtn.onClick.AddListener(UkazRiesenie);

        


        _tiles = new Dictionary<Vector2, Tile>();
    }

    private void UkazRiesenie()
    {
        if (klik) { 
            if (_serie == 1)
            {
                if (levelNumber == 1)
                {
                    sada1level1.SetActive(true);
                }
                if (levelNumber == 2)
                {
                    sada1level2.SetActive(true);
                }
                if (levelNumber == 3)
                {
                    sada1level3.SetActive(true);
                }

            }
            else
            {
                if (levelNumber == 1)
                {
                    EditorUtility.DisplayDialog("⁄loha nem· rieöenie", "T·to ˙loha nem· rieöenie", "dobre");
                }
                if (levelNumber == 2)
                {
                    EditorUtility.DisplayDialog("⁄loha nem· rieöenie", "T·to ˙loha nem· rieöenie", "dobre");
                }
                if (levelNumber == 3)
                {
                    sada2level3.SetActive(true);
                }
            }
            klik = false;
        }
        else
        {
            sada1level1.SetActive(false);
            sada1level2.SetActive(false);
            sada1level3.SetActive(false);
            sada2level3.SetActive(false);
            klik = true;
        }
    }

    private void RiesenieCheck()
    {
        if (!solve)
        {
            gameOver = true;
            endMenu.SetActive(true);
        }
        else
        {
            EditorUtility.DisplayDialog("Nem·ö pravdu", "T·to ˙loha m· rieöenie", "dobre...");
        }
    }

    // Update is called once per frame

    public void clear() {
        foreach (Tile t in _tiles.Values) {
            Destroy(t._Player);
            Destroy(t._Tree);
            Destroy(t._renderer);
            Destroy(t._HighLight);
            Destroy(t);
        }
        _tiles.Clear();
        mapa.Clear();
        foreach (var krok in kroky) {
            Destroy(krok.getLine());
        }
        kroky.Clear();
        saved = true;
        actualTile = null;
    }

    private void editorChooseTile(char ch) {
        editorTileChoosen = ch;
    }

    private void previousGame()
    {

        if (levelNumber - 1 > 0)
        {
            NewGame(_serie, levelNumber -= 1);
            endMenu.SetActive(false);
            sada1level1.SetActive(false);
            sada1level2.SetActive(false);
            sada1level3.SetActive(false);
            sada2level3.SetActive(false);
            klik = true;
        }
    }

    private void nextGame() {

        if (levelNumber + 1 < 4)
        {
            NewGame(_serie, levelNumber += 1);
            endMenu.SetActive(false);
            sada1level1.SetActive(false);
            sada1level2.SetActive(false);
            sada1level3.SetActive(false);
            sada2level3.SetActive(false);
            klik = true;
        }
    }

    void Update()
    {
        if (!editorGame & !gameOver) {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (actualX + 1 < _width)
                {
                    if ((GetTileAtPosition(new Vector2(actualX + 1, actualY))._znak != 's') & (GetTileAtPosition(new Vector2(actualX + 1, actualY))._znak != '1'))
                    {
                        actualTile._znak = '1';
                        actualTile._Player.SetActive(false);
                        mapa[_height - actualY - 1][actualX] = '1';

                        var oldTile = actualTile;

                        actualTile = GetTileAtPosition(new Vector2(actualX + 1, actualY));
                        actualTile._znak = 'z';
                        actualTile._Player.SetActive(true);



                        var line = CreateLine(1, 0);

                        kroky.Add(new Pair(oldTile, line));

                        actualX += 1;

                        saved = false;
                        if (Check())
                        {
                            endMenu.SetActive(true);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!editorGame & !gameOver)
            {
                if (actualY - 1 >= 0)
                {
                    if ((GetTileAtPosition(new Vector2(actualX, actualY - 1))._znak != 's') & (GetTileAtPosition(new Vector2(actualX, actualY - 1))._znak != '1'))
                    {

                        actualTile._znak = '1';
                        actualTile._Player.SetActive(false);
                        mapa[_height - actualY - 1][actualX] = '1';

                        var oldTile = actualTile;

                        actualTile = GetTileAtPosition(new Vector2(actualX, actualY - 1));
                        actualTile._znak = 'z';
                        actualTile._Player.SetActive(true);

                        var line = CreateLine(0, -1);

                        kroky.Add(new Pair(oldTile, line));
                        actualY -= 1;

                        saved = false;
                        if (Check())
                        {
                            endMenu.SetActive(true);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (!editorGame & !gameOver)
            {
                if (actualY + 1 < _height)
                {
                    if ((GetTileAtPosition(new Vector2(actualX, actualY + 1))._znak != 's') & (GetTileAtPosition(new Vector2(actualX, actualY + 1))._znak != '1'))
                    {

                        actualTile._znak = '1';
                        actualTile._Player.SetActive(false);
                        mapa[_height - actualY - 1][actualX] = '1';

                        var oldTile = actualTile;

                        actualTile = GetTileAtPosition(new Vector2(actualX, actualY + 1));
                        actualTile._znak = 'z';
                        actualTile._Player.SetActive(true);

                        var line = CreateLine(0, 1);

                        kroky.Add(new Pair(oldTile, line));
                        actualY += 1;

                        saved = false;
                        if (Check())
                        {
                            endMenu.SetActive(true);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!editorGame & !gameOver)
            {
                if (actualX - 1 >= 0)
                {
                    if ((GetTileAtPosition(new Vector2(actualX - 1, actualY))._znak != 's') & (GetTileAtPosition(new Vector2(actualX - 1, actualY))._znak != '1'))
                    {
                        actualTile._znak = '1';
                        actualTile._Player.SetActive(false);
                        mapa[_height - actualY - 1][actualX] = '1';

                        var oldTile = actualTile;

                        actualTile = GetTileAtPosition(new Vector2(actualX - 1, actualY));
                        actualTile._znak = 'z';
                        actualTile._Player.SetActive(true);

                        var line = CreateLine(-1, 0);

                        kroky.Add(new Pair(oldTile, line));
                        actualX -= 1;

                        saved = false;
                        if (Check())
                        {
                            endMenu.SetActive(true);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            back();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            menu.SetActive(true);
        }
    }

    void NacitajLevel()
    {

        var file = (TextAsset)Resources.Load(path + name);
        string[] strings = file.ToString().Split('\n');

        if (strings[0][0] == 'S') {
            var item = strings[0];
            var saveLine = strings[0].Remove(0, 1).Split('|');
            foreach (var sL in saveLine)
            {
                var coordinations = sL.Split('-');
                v0.Add(new Vector2(float.Parse(coordinations[0].Replace('.', ',')), float.Parse(coordinations[1].Replace('.', ','))));
                v1.Add(new Vector2(float.Parse(coordinations[2].Replace('.', ',')), float.Parse(coordinations[3].Replace('.', ','))));
                vT.Add(new Vector2(float.Parse(coordinations[4].Replace('.', ',')), float.Parse(coordinations[5].Replace('.', ','))));
            }
            strings = Array.FindAll(strings, i => i != item).ToArray();
        }

        int i = 0;
        int j = 0;

        if (strings[strings.Length - 1][0] == 'D')
        {
            if (strings[strings.Length - 1][1] == 'R')
            {
                solve = true;
                if (strings[strings.Length - 1][2] == 'K')
                {
                    levelType.text = "Kruûnica";
                }
                else
                {
                    levelType.text = "çah";
                }
            }
            else
            {
                solve = false;
            }
        }

        strings = strings.Take(strings.Length - 1).ToArray();

        foreach (string s in strings)
        {
            if (s != "")
            {
                j = 0;
                mapa.Add(new List<char>());
                var str = s.Replace("\n", "").Replace("\r", "");
                foreach (char ch in str)
                {
                    mapa[i].Add(ch);
                    j += 1;
                }
                i += 1;
            }
        }
        _width = j;
        _height = i;
    }

    void VytvorGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var tile = Instantiate(_prefab, new Vector3(x + 0.5F, y - 0.5F), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);

                if (mapa[_height - y - 1][x] == 'z')
                {
                    actualX = x;
                    actualY = y;
                    actualTile = tile;
                }

                tile.Init(isOffset, x, y, mapa[_height - y - 1][x], _serie);

                _tiles[new Vector2(x, y)] = tile;
            }

        }
        if (v0.Count > 0) {
            for (int l = 0; l <= v0.Count - 1; l++) {
                actualX = (int)vT[l].x;
                actualY = (int)vT[l].y;
                actualTile = GetTileAtPosition(vT[l]);
                kroky.Add(new Pair(actualTile, CreateLine((int)(v1[l].x - v0[l].x), (int)(v1[l].y - v0[l].y))));
            }
            actualX += (int)(v1[v1.Count - 1].x - v0[v0.Count - 1].x);
            actualY += (int)(v1[v1.Count - 1].y - v0[v0.Count - 1].y);

            actualTile = GetTileAtPosition(new Vector2(actualX, actualY));
            v0.Clear();
            v1.Clear();
            vT.Clear();

        }
        if (actualTile != null)
        {
            actualTile._Player.SetActive(true);
        }
        _cam.transform.position = new Vector3((float)_width / 2, (float)_height / 2 - 0.2F, -10);
    }

    public void NacitajEditor(int row, int column)
    {
        if ((row > 10) | (column > 15)){
            EditorUtility.DisplayDialog("Veæke pole", "Maxim·lny poËet riadkov je 15 a stÂpcov 10", "OK");
            return;
        }
        if ((mapa.Count == 0) & (editorGame == false)){
            clear();
        }
        if (mapa.Count == 0)
        {
            editorGame = true;
            _width = column;
            _height = row;
            _serie = 1;
            for (int x = 0; x < _height; x++)
            {
                mapa.Add(new List<char>());
                for (int y = 0; y < _width; y++)
                {
                    mapa[x].Add('.');
                }
            }
        }
        else {
            modifeMap(row - mapa.Count, column - mapa[1].Count);
        }
        VytvorGrid();
        editorGame = true;
    }

    public void modifeMap(int row, int column)
    {
        for (int r = 0; r < row; r++)
        {
            if (row > 0)
                mapa.Add(new string('.', mapa[0].Count).ToCharArray().ToList());
            if (row < 0)
                mapa.RemoveAt(mapa.Count - 1);
        }
        foreach (List<char> lists in mapa)
            {
            for (int c = 0; c < column; c++)
            {
                if (column > 0)
                {
                    lists.Add('.');
                }
                else {
                    lists.RemoveAt(lists.Count - 1);
                }
            }
        }
    
        _height += row;
        _width += column;
        _cam.transform.position = new Vector3((float)_width / 2, (float)_height / 2 - 0.2F, -10); 
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            return tile;
        return null;
    }

    void SelectAction(Tile target)
    {
        if (editorGame & editorTileChoosen != '0') {
            if (editorTileChoosen == 'z' & wizi) {
                return;
            }
            if (editorTileChoosen == 'z')
            {
                wizi = true;
            }
            else {
                if (target._znak == 'z') {
                    wizi = false;
                }
            }
            target._znak = editorTileChoosen;
            target.refresh();
            mapa[_height - target._y - 1][target._x] = editorTileChoosen;
        }
    }
    LineRenderer CreateLine(int x, int y)
    {
        var Line = Instantiate(LineRenderer, new Vector3(0, 0, 1), Quaternion.identity);

        Color c1 = Color.white;

        Line.material = new Material(Shader.Find("Sprites/Default"));
        Line.SetColors(c1, c1);

        // set width of the renderer
        Line.startWidth = 0.1f;
        Line.endWidth = 0.1f;

        // set the position
        Line.SetPosition(0, new Vector3(actualX + 0.5F, actualY - 0.5F , -2));
        Line.SetPosition(1, new Vector3(actualX + 0.5F + x, actualY - 0.5F + y, -2));
        Line.useWorldSpace = true;
        return Line;

    }


    bool Check()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (GetTileAtPosition(new Vector2(x, y))._znak == '.') return false;
            }
        }
        gameOver = true;
        return true;
    }


    public void back()
    {
        if (kroky.Count > 0)
        {
            var pair = kroky[kroky.Count-1];
            kroky.RemoveAt(kroky.Count - 1);

            actualTile._znak = '.';
            actualTile._Player.SetActive(false);

            actualTile = pair.getTile();
            actualTile._Player.SetActive(true);
            actualX = actualTile._x;
            actualY = actualTile._y;
            Destroy(pair.getLine());
            saved = false;
        }

        if (kroky.Count == 0)
            saved = true;
    }

    public void showMenu()
    {
        hint.SetActive(false);
        if (mapa.Count != 0)
        {
            continueButton.gameObject.SetActive(true);
        }
        menu.gameObject.SetActive(true);
    }

    public void Save(string name, bool kruznica)
    {
        //var str = saveLine();

        var str = "";
        foreach (var i in mapa)
        {

            foreach (var ch in i)
            {
                str += ch;
            }
            str += "\n";
        }
        if (kruznica)
        {
            str += "CRK";
        }
        else
        {
            str += "CRT";
        } 
       
        File.WriteAllText(Application.dataPath + "/Resources/Created/" + name + ".txt", str);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        saved = true;

    }

    public void NewGame(int serie, int level = 1)
    {
        menuPanel.SetActive(true);
        editorPanel.SetActive(false);
        endMenu.SetActive(false);
        levelNumber = level;
        editorGame = false;
        gameOver = false;
        actualLevel.text = "Level: " + levelNumber.ToString() + "/3";
        _serie = serie;
        path = "Sada" + serie.ToString() + "/";
        name = "map" + level.ToString();
        if (actualTile != null)
            actualTile._Player.gameObject.SetActive(false);
        clear();
        mapa.Clear();
        NacitajLevel();
        VytvorGrid();
    }

    private string saveLine()
    {
        string line = "S";

        foreach (Pair p in kroky)
        {
            Vector3 v0 = p.getLine().GetPosition(0);
            Vector3 v1 = p.getLine().GetPosition(1);
            Tile t = p.getTile();

            line += v0.x.ToString().Replace(',', '.') + "-" + v0.y.ToString().Replace(',', '.') + "-" + v1.x.ToString().Replace(',', '.') + "-" + v1.y.ToString().Replace(',', '.') + "-" + t._x.ToString().Replace(',', '.') + "-" + t._y.ToString().Replace(',', '.') + "|";
        }
        line = line.Remove(line.Length - 1);
        line += "\n";
        return line;
    }


    public void loadSave()
    {
        clear();
        string fileName = dialogWindow();
        var spl = fileName.Split('/');
        name = spl[spl.Length - 1].Split('.')[0];
        _serie = 1;
        path = "Created/";
        NacitajLevel();
        VytvorGrid();
    }

    private string dialogWindow()
    {
        var path = EditorUtility.OpenFilePanel("Vyber si level", Application.dataPath + "/Resources/", "txt");
        if (string.IsNullOrEmpty(path))
            return "";
        return path;
    }
}
