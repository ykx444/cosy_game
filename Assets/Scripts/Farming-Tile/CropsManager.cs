using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class CropTile
{
    public Crop crop;
    public int growTimer;//state of crop growing on a tile
    public SpriteRenderer spriteRenderer;
    public int growStage;
    public bool isSeeded;

    public bool isHarvestable
    {
        get
        {
            if (crop == null) return false;
            return growTimer >= crop.timeToGrow;
        }
    }
    //icon to show harvestable
    public GameObject harvestIcon;

    internal void Collected()
    {
        isSeeded = false;
        growTimer = 0;
        growStage = 0;
        crop = null;
        spriteRenderer.gameObject.SetActive(false);
        harvestIcon.SetActive(false);
    }
}
public class CropsManager : TimeAgent
{
    //the time agent is central agent controlling all crop tiles
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] Tilemap targetPlowTilemap;
    [SerializeField] Tilemap targetSeedTilemap;

    public GameObject cropstiles;

    //set tilemap as plowed tile
    //to interact with crop, find the position of the crop
    Dictionary<Vector2Int, CropTile> crops;

    //crop to change sprite
    [SerializeField] GameObject cropSpritePrefab;
    public GameObject harvestIconPrefab;

    public int sceneIndexToShow = 1;
    private void Start()
    {
        crops = new Dictionary<Vector2Int, CropTile>();
        onTimeRun += Tick;
        Init();
        //harvestIconPrefab = cropSpritePrefab.transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        foreach (CropTile tile in crops.Values)
        {
            if (tile.isHarvestable)
            {
                //set icon to be harvestable
                tile.harvestIcon.SetActive(true);
            }
        }
    }
    public void Tick()
    {
        targetSeedTilemap.gameObject.SetActive(false);
        //tick once every phase in game
        foreach (CropTile tile in crops.Values)
        {
            if (tile.crop == null || tile.isHarvestable) { continue; } //skip tile if not seeded                                 //hide the harvestable icon

            else
            {
                tile.growTimer += 1;

                if (tile.growStage > 4) continue;
                if (tile.growTimer >= tile.crop.growStageTime[tile.growStage])
                {
                    //if it is time to advance to next stage of growth
                    tile.spriteRenderer.sprite = tile.crop.sprites[tile.growStage];
                    tile.growStage += 1;
                }
            }


        }
    }

    public bool CheckPlowed(Vector3Int position)
    {
        //we can only seed plowed tile
        return crops.ContainsKey((Vector2Int)position);
    }

    public bool CheckSeeded(Vector3Int position)
    {

        // Check if the position exists in the dictionary and get the CropTile
        if (crops.TryGetValue((Vector2Int)position, out CropTile cropTile))
        {
            // Check if true, it is seeded, not seed again
            return cropTile.isSeeded == true;
        }
        return false;
    }
    public void Plow(Vector3Int position)
    {

        //plow a tile
        if (crops.ContainsKey((Vector2Int)position))
        {
            //see if a tile is already plowed
            return;
        }

        GameObject obj = Instantiate(cropSpritePrefab, cropstiles.transform);

        harvestIconPrefab = obj.transform.GetChild(0).gameObject;
        harvestIconPrefab.SetActive(false);

        // Offset to ensure the sprite is at the center of the tile
        //Vector3 offset = new Vector3(cellSize.x/2, cellSize.y / 2, 0);
        obj.transform.position = targetSeedTilemap.CellToWorld(position);


        //create the plowed tile
        CropTile crop = new()
        {
            harvestIcon = harvestIconPrefab
        };
        harvestIconPrefab.SetActive(false);

        //hide it and send reference to Crop class object
        crop.spriteRenderer = obj.GetComponent<SpriteRenderer>();
        crop.harvestIcon = obj.transform.GetChild(0).gameObject;
        obj.SetActive(false);

        crops.Add((Vector2Int)position, crop);
        targetPlowTilemap.SetTile(position, plowed);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        //play sound
        GameManager.instance.audioManager.PlaySoundEffect("place");
        //targetSeedTilemap.gameObject.SetActive(true);
        // assign seed to tile
        targetSeedTilemap.SetTile(position, seeded);
        // store a reference
        crops[(Vector2Int)position].crop = toSeed;
        crops[(Vector2Int)position].isSeeded = seeded;

        //set the seed sprite
        crops[(Vector2Int)position].spriteRenderer.gameObject.SetActive(true);
        crops[(Vector2Int)position].growStage = 0;
        crops[(Vector2Int)position].growTimer = 0;
        crops[(Vector2Int)position].spriteRenderer.sprite = toSeed.sprites[0];

    }

    public bool Harvest(Vector3Int tilePosition)
    {
        Vector2Int position = (Vector2Int)tilePosition;
        if (crops.ContainsKey(position))
        {
            CropTile cropTile = crops[position];
            if (cropTile.isHarvestable)
            {
                //seed
                SpawnDroppedItemManager.instance.SpawnItem(targetSeedTilemap.CellToWorld(tilePosition),
                    cropTile.crop.yield,
                    cropTile.crop.count, cropTile.crop.seedDropRate);

                //crop
                SpawnDroppedItemManager.instance.SpawnItem(targetSeedTilemap.CellToWorld(tilePosition),
                    cropTile.crop.yield,
                    cropTile.crop.count);

                cropTile.Collected();
                //Destroy(cropTile.gameObject);

                return true;
            }
        }
        return false;
    }

    //************Hide the grass tilemap if not at overworld scene
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene targetScene = SceneManager.GetSceneByName("Overworld");
        bool isLoaded = targetScene.isLoaded;
        targetSeedTilemap.gameObject.SetActive(isLoaded);
        targetPlowTilemap.gameObject.SetActive(isLoaded);
        GameManager.instance.tilemapReadManager.tilemap.gameObject.SetActive(isLoaded);
        cropstiles.SetActive(isLoaded);
    }
}
