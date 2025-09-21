using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tileset;
    [SerializeField] private Transform plr;
    [SerializeField] private UpgradeManager uManager;

    [SerializeField] private int xpOnConsume = 1;

    private int Grey = -1;

    public static TileManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (tileset == null)
        {
            tileset = GameObject.Find("Tileset").GetComponent<Tilemap>();
        }
        if (plr == null)
        {
            plr = Level.Instance.Player.transform;
        }
        if (uManager == null)
        {
            uManager = UpgradeManager.Instance;
        }
    }

    void Update()
    {
        var a = new Vector3[1]; //testing purposes

        //Check all tiles in the players void aura, if they are not coloured, colour them gray and give xp
        

        a[0] = plr.position;
        CalculateVoidAura(plr.position);
        if(isColoured(a[0])){}
        placeTiles(a, Grey); //-1 is to reset the tile to black
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
        placeTiles(positions, Grey); //call placeTiles with the positions found and color gray (0)
    }

    bool isColoured(Vector3 Position) //checks whether the tile at the position inputted is coloured. if so it returns true
    {
        if(PlayerState.instance.currentState == PlayerState.PlayerStates.InMenu) //if in menu dont give xp
        {
            return false;
        }
        if (tileset.GetColor(tileset.WorldToCell(Position)) == Color.black) //black is default tile colour
        {
            return false;
        }
        uManager.AddXP(xpOnConsume);
        return true;
    }

    void placeTiles(Vector3[] positions, int color)
    {

        for (int i = 0; i < positions.Length; i++)
        {
            tileset.SetTileFlags(tileset.WorldToCell(positions[i]), TileFlags.None);
            switch (color)
            {
                case 0:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.red);
                    break;
                case 1:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.blue);
                    break;
                case 2:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.green);
                    break;
                case 3:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.yellow);
                    break;
                default:
                    tileset.SetColor(tileset.WorldToCell(positions[i]), Color.black);
                    break;
            }
        }
    }
}
  