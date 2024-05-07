using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System;

public class HolderGrid : MonoBehaviour
{
    public SpriteRenderer gridRenderer_1;
    public SpriteRenderer gridRenderer_2;
    public SpriteRenderer board_1;
    public SpriteRenderer board_2;
    public TMP_Text txt_1;
    public TMP_Text txt_2;

    private void Awake()
    {
        gridRenderer_1.enabled = true;
        txt_1.enabled = true;
        board_1.enabled = true;
        gridRenderer_2.enabled = false;
        txt_2.enabled = false;
        board_2.enabled = false;
    }
    
    public void Disable_1()
    {
        gridRenderer_1.enabled = false;
        txt_2.enabled = false;
        board_1.enabled = false;
    }
    public void Enable_1()
    {
        gridRenderer_1.enabled = true;
        txt_2.enabled = true;
        board_1.enabled = true;
    }
    public void Disable_2()
    {
        gridRenderer_2.enabled = false;
        txt_2.enabled = false;
        board_2.enabled = false;
    }
    public void Enable_2()
    {
        gridRenderer_2.enabled = true;
        txt_2.enabled = true;
        board_2.enabled = true;
    }
}
