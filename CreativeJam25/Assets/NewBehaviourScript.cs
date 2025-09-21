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

        //Check all tiles in the players void aura, if they are not coloured, colour them gray and give xp
        

        a[0] = plr.position;
        CalculateVoidAura(plr.position);

        print(isColoured(a[0]));
        placeTiles(a, 0);
    }

    public void CalculateVoidAura(Vector3 Position) //calculates all the tiles in the players void aura
    {
        float radius = PlayerStats.instance.voidRadius;
        int tilesInRadius = Mathf.CeilToInt(radius); //rounds up to the nearest whole number

        var positions = new Vector3[(tilesInRadius * 2 + 1) * (tilesInRadius * 2 + 1)];
        int index = 0;

        for (int x = -tilesInRadius; x <= tilesInRadius; x++)
        {
            for (int y = -tilesInRadius; y <= tilesInRadius; y++)
            {
                Vector3Int tilePosition = tileset.WorldToCell(new Vector3(Position.x + x, Position.y + y, Position.z));
                Vector3 worldPosition = tileset.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0); //center of the tile

                if (Vector3.Distance(worldPosition, Position) <= radius)
                {
                    positions[index] = worldPosition;
                    index++;
                }
            }
        }

        Array.Resize(ref positions, index); //resize the array to the number of positions found
        //We need to check if the tile is colored already, if not we colour it gray and give xp
        for (int i = 0; i < positions.Length; i++)
        {
            if (isColoured(positions[i]))
            {
                // XP is already given in isColoured
            }
        }
        placeTiles(positions, 0); //call placeTiles with the positions found and color gray (0)
    }

    bool isColoured(Vector3 Position) //checks whether the tile at the position inputted is coloured. if so it returns true
    {
        if(PlayerState.instance.currentState == PlayerState.PlayerStates.InMenu) //if in menu dont give xp
        {
            return false;
        }
        if (tileset.GetColor(tileset.WorldToCell(Position)) == Color.gray)
        {
            return false;
        }
        uManager.AddXP(1.0f);
        return true;
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
  