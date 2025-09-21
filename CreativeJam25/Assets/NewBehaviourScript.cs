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
    [SerializeField] private UpgradeManager uManager;






    void Update()
    {
        var a = new Vector3[1]; //testing purposes

        a[0] = plr.position;

        placeTiles(a, 0); 
        print(isColoured(a[0]));
    }

    bool isColoured(Vector3 Position) //checks whether the tile at the position inputted is coloured. if so it returns true
    {
        if (tileset.GetColor(tileset.WorldToCell(Position)) == Color.gray)
        {
            return false;
            
        }
        else
        {
            return true;
            uManager.AddXP(67.0f);
        }
    }

    // position has to be an array, that way multiple tiles can be placed instantly, 0 is gray, 1 is red, 2 is blue, 3 is green, 4 is orange
    void placeTiles(Vector3[] positions, int color)
    {

        for (int i = 0; i < positions.Length; i++)
        {
            tileset.SetTileFlags(tileset.WorldToCell(positions[i]), TileFlags.None);
            switch (color)
            {
                case 0:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.gray);
                    
                    break;
                case 1:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.red);
                    break;
                case 2:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.blue);
                    break;
                case 3:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.green);
                    break;
                case 4:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.yellow);
                    break;
            }
            

            {

            }
        }
    }
}
  