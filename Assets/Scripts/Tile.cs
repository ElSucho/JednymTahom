using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public Color _baseColor, _offsetColor;
    public Color _baseColor2, _offsetColor2;
    public SpriteRenderer _renderer;
    public GameObject _HighLight;
    public GameObject _Tree, _Tree1, _Tree2;
    public GameObject _Player, _Player1, _Player2;
    public Color _selectTile;

    public char _znak;
    public int _x, _y;


    public delegate void SelectAction(Tile target);
    public static event SelectAction OnSelectedEvent;

    public void Init(bool isOffset, int x, int y, char znak, int serie)
    {
        if (serie == 1)
        {
            _renderer.color = isOffset ? _offsetColor : _baseColor;
            _Tree = _Tree1;
            _Player = _Player1;
        }

        if (serie == 2)
        {
            _renderer.color = isOffset ? _offsetColor2 : _baseColor2;
            _Tree = _Tree2;
            _Player = _Player2;
        }

        _x = x;
        _y = y;
        _znak = znak;

        if (znak == 's') _Tree.SetActive(true);
        if (znak == 'z') _Player.SetActive(true);
    }

    public void refresh() {
        _Tree.SetActive(false);
        _Player.SetActive(false);

        if (_znak == 's') _Tree.SetActive(true);
        if (_znak == 'z') _Player.SetActive(true);
    }

    private void OnMouseEnter()
    {
        _HighLight.SetActive(true);
    }

    private void OnMouseExit()
    {
        _HighLight.SetActive(false);
    }

    private void OnMouseDown()
    {
       
        if (OnSelectedEvent != null)
        {
            OnSelectedEvent(this);
        }
    }
}
