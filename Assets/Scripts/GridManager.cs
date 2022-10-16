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
    public GameObject win;
    public LineRenderer LineRenderer;
    public Transform _cam;
    private List<List<char>> mapa;
    private Dictionary<Vector2, Tile> _tiles;
    private int actualX, actualY;
    private Tile actualTile;
    private List<Pair> kroky;
    public Button backButton;
    public Button menuButton;
    public Button continueButton;
    public GameObject menu;
    private string levelName = "map1";
    private List<Vector2> v0 = new List<Vector2>();
    private List<Vector2> v1 = new List<Vector2>();
    private List<Vector2> vT = new List<Vector2>();
    public bool saved = true;




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
    }

    // Update is called once per frame

    private void clear() {
        mapa.Clear();
        foreach (var krok in kroky) {
            Destroy(krok.getTile());
            Destroy(krok.getLine());
        }
        kroky.Clear();
        saved = true;
    }
    
    void Update()
    {
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
                        win.SetActive(true);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
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
                        win.SetActive(true);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
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
                        win.SetActive(true);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
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
                        win.SetActive(true);
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

    void NacitajLevel(string game)
    {
        var file = (TextAsset)Resources.Load(game);
        string[] strings = file.ToString().Split('\n');

        if (strings[0][0] == 'S') {
            var item = strings[0];
            var saveLine = strings[0].Remove(0, 1).Split('|');
            foreach(var sL in saveLine)
            {
                var coordinations = sL.Split('-');
                v0.Add(new Vector2(float.Parse(coordinations[0].Replace('.',',')), float.Parse(coordinations[1].Replace('.', ','))));
                v1.Add(new Vector2(float.Parse(coordinations[2].Replace('.', ',')), float.Parse(coordinations[3].Replace('.', ','))));
                vT.Add(new Vector2(float.Parse(coordinations[4].Replace('.', ',')), float.Parse(coordinations[5].Replace('.', ','))));
            }
            strings = Array.FindAll(strings, i => i != item).ToArray();
        }

        int i = 0;
        int j = 0;
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
        _tiles = new Dictionary<Vector2, Tile>();
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

                tile.Init(isOffset, x, y, mapa[_height - y - 1][x]);

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
        actualTile._Player.SetActive(true);
        _cam.transform.position = new Vector3((float)_width / 2 , (float)_height / 2 - 0.2F, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            return tile;
        return null;
    }

    void SelectAction(Tile target)
    {

    }
    LineRenderer CreateLine(int x, int y)
    {
        var Line = Instantiate(LineRenderer, new Vector3(0, 0, 1), Quaternion.identity);

        Line.startColor = Color.red;
        Line.endColor = Color.red;

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
        continueButton.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
    }

    public void Save(string name)
    {
        var str = saveLine();
        foreach (var i in mapa)
        {

            foreach (var ch in i)
            {
                str += ch;
            }
            str += "\n";
        }

        File.WriteAllText(Application.dataPath + "/Resources/" + name + ".txt", str);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        saved = true;
    }

    public void NewGame(string level = "map1")
    {
        if (actualTile != null)
            actualTile._Player.gameObject.SetActive(false);
        if (!saved)
            clear();
        mapa.Clear();
        NacitajLevel(level);
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
        string fileName = dialogWindow();
        var spl = fileName.Split('/');
        name = spl[spl.Length - 1].Split('.')[0];
        NewGame(name);

    }

    private string dialogWindow()
    {
        var path = EditorUtility.OpenFilePanel("Vyber si level", Application.dataPath + "/Resources/", "txt");
        if (string.IsNullOrEmpty(path))
            return "";
        return path;
    }
}
