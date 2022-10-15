using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

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

    private Stack<Pair> kroky;

    public Button backButton;

    public Button menuButton;

    public Button continueButton;

    public GameObject menu;




    // Start is called before the first frame update
    void Start()
    {
        Tile.OnSelectedEvent += SelectAction;
        mapa = new List<List<char>>();
        kroky = new Stack<Pair>();

        Button bckBtn = backButton.GetComponent<Button>();
        bckBtn.onClick.AddListener(back);
        Button menuBtn = menuButton.GetComponent<Button>();
        menuBtn.onClick.AddListener(showMenu);
    }

    // Update is called once per frame
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

                    kroky.Push(new Pair(oldTile, line));

                    actualX += 1;

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

                    kroky.Push(new Pair(oldTile, line));
                    actualY -= 1;

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

                    kroky.Push(new Pair(oldTile, line));
                    actualY += 1;

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

                    kroky.Push(new Pair(oldTile, line));
                    actualX -= 1;

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
        int i = 0;
        int j = 0;
        foreach (string s in strings)
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


        _cam.transform.position = new Vector3((float)_width / 2 - 0.5F, (float)_height / 2 - 0.5F, -10);
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
        Line.SetPosition(0, new Vector3(actualX + 0.5F, actualY - 0.5F, -2));
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
            var pair = kroky.Pop();

            actualTile._znak = '.';
            actualTile._Player.SetActive(false);

            actualTile = pair.getTile();
            actualTile._Player.SetActive(true);
            actualX = actualTile._x;
            actualY = actualTile._y;
            Destroy(pair.getLine());
        }
    }

    public void showMenu()
    {
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
    }

    public void NewGame()
    {

        NacitajLevel("map1");
        VytvorGrid();
    }

    private string saveLine()
    {
        string line = "S";

        foreach (Pair p in kroky)
        {
            Vector3 v0 = p.getLine().GetPosition(0);
            Vector3 v1 = p.getLine().GetPosition(1);

            line += v0.x.ToString().Replace(',', '.') + "-" + v0.y.ToString().Replace(',', '.') + "-" + v1.x.ToString().Replace(',', '.') + "-" + v1.y.ToString().Replace(',', '.') + "|";
        }
        line = line.Remove(line.Length - 1);
        line += "\n";
        return line;
    }


    public void loadSave()
    {
        string fileName = dialogWindow();
        var spl = fileName.Split('/');
        name = spl[spl.Length - 1];


    }

    private string dialogWindow()
    {
        var path = EditorUtility.OpenFilePanel("Vyber si level", Application.dataPath + "/Resources/", "txt");
        if (string.IsNullOrEmpty(path))
            return "";
        return path;
    }
}
