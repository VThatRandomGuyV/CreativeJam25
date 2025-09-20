using NUnit.Framework.Constraints;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Tilemap tileset;
    [SerializeField] private Rigidbody2D plr;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var a = new Vector3[1];
        var b = new int[1];

        a[0] = plr.position;
        b[0] = 0;
        //print(plr.position);
        //print(a[0]);

        placeTiles(a, b);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isColoured(Vector3 Position)
    {
        if (tileset.GetTile(tileset.WorldToCell(Position)).name.EndsWith("0"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

// 0 is gray, 1 is red, 2 is blue, 3 is green, 4 is orange
    void placeTiles(Vector3[] positions, int[] tile)
    {
        var cellPosition = new Vector3Int[positions.Length];
        var actualTiles = new TileBase[tile.Length];
        print(positions.Length);
        for (int i = 0; i < positions.Length; i++) 
            {
                cellPosition[i] = tileset.WorldToCell(positions[i]);
            print("BBBBB");
            {

            }
            }
        print("CCCCC");
        for (int i = 0; i < tile.Length; i++)
            {
            print("DDDDD");
            var idk = Tile.Instantiate();
                idk.name = tile[i].ToString();
                idk.sprite = Resources.Load("Assets/Sprites/tempTileColours - Replace_" + tile[i].ToString()) as Sprite;
            actualTiles[i] = idk;
            }

        tileset.SetTiles(cellPosition, actualTiles);
    } 
}
