using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    public int _width, _height;

    public Tile _prefab;

    public GameObject win;

    public LineRenderer LineRenderer;

    public Transform _cam;

    private List<string> mapa;

    private Dictionary<Vector2, Tile> _tiles;

    private int actualX, actualY;

    private Tile actualTile;

    private Stack<Pair> kroky;

    public Button backButton;

    public Button hint;




    // Start is called before the first frame update
    void Start()
    {
        Tile.OnSelectedEvent += SelectAction;
        mapa = new List<string>();
        kroky = new Stack<Pair>();

        Button bckBtn = backButton.GetComponent<Button>();
        bckBtn.onClick.AddListener(back);
        Button hintBtn = hint.GetComponent<Button>();
        hintBtn.onClick.AddListener(showHint);

        NacitajLevel(2);
        VytvorGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (actualX + 1 < _width) {
                if ((GetTileAtPosition(new Vector2(actualX + 1, actualY))._znak != 's') & (GetTileAtPosition(new Vector2(actualX + 1, actualY))._znak != '1')){
                    actualTile._znak = '1';
                    actualTile._Player.SetActive(false);

                    var oldTile = actualTile;

                    actualTile = GetTileAtPosition(new Vector2(actualX + 1, actualY));
                    actualTile._znak = 'z';
                    actualTile._Player.SetActive(true);

                    var line = CreateLine(1, 0);

                    kroky.Push(new Pair(oldTile, line));

                    actualX += 1;

                    if (Check()) {
                        win.SetActive(true);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            if (actualY - 1 >= 0)
            {
                if ((GetTileAtPosition(new Vector2(actualX, actualY - 1))._znak != 's') & (GetTileAtPosition(new Vector2(actualX, actualY - 1))._znak != '1')) { 

                    actualTile._znak = '1';
                    actualTile._Player.SetActive(false);

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
                if (( GetTileAtPosition(new Vector2(actualX - 1, actualY))._znak  != 's') & (GetTileAtPosition(new Vector2(actualX - 1, actualY))._znak != '1'))
                {
                    actualTile._znak = '1';
                    actualTile._Player.SetActive(false);

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

        if (Input.GetKeyDown(KeyCode.Space)) {
            back();
        }
    }

    void NacitajLevel(int lvl) {

        var file = (TextAsset)Resources.Load("map1");
        string[] strings = file.ToString().Split('\n');
        foreach (string s in strings) {
            mapa.Add(s.Replace("/r", ""));
        }

        _width = mapa[1].Length - 1;
        _height = mapa.Count;
    }

    void VytvorGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var tile = Instantiate(_prefab, new Vector3(x + 0.5F, y - 0.5F), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    
                if (mapa[_height - y - 1][x] == 'z'){
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


    public void back() {
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

    public void showHint() {
        SceneManager.LoadScene("Hint");

    }
}
