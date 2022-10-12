using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public Color _baseColor, _offsetColor;
    public SpriteRenderer _renderer;
    public GameObject _HighLight;
    public GameObject _Tree;
    public GameObject _Player;
    public Color _selectTile;

    public char _znak;
    public int _x, _y;


    public delegate void SelectAction(Tile target);
    public static event SelectAction OnSelectedEvent;

    public void Init(bool isOffset, int x, int y, char znak)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;

        _x = x;
        _y = y;
        _znak = znak;

        if (znak == 's') _Tree.SetActive(true);
        if (znak == 'z') _Player.SetActive(true);
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
