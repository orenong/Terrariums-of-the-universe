using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrariumGenerator : MonoBehaviour
{

    float[] landHeights;
    Texture2D Terrarium;

    const int WIDTH = 860;
    const int HEIGHT = 600;
    float FADE_IN_TIME = 0.5f; //x2 seconds. 1 = 2sec



    private void Update()
    {
        Color TerrariumColor = gameObject.GetComponent<Image>().color;

        if (TerrariumColor.a < 1)
        {
            gameObject.GetComponent<Image>().color = new Color(TerrariumColor.r, TerrariumColor.g, TerrariumColor.b, Mathf.Min(TerrariumColor.a+Time.deltaTime / FADE_IN_TIME,1));

        }
    }



    private void Start()
    {
        Debug.Log("seed: " + Manager.planet.seed);
        Terrarium = new Texture2D(WIDTH, HEIGHT);
        gameObject.GetComponent<Image>().sprite = Sprite.Create(Terrarium, new Rect(new Vector2(0, 0), new Vector2(WIDTH,HEIGHT)), new Vector2(WIDTH / 2, HEIGHT / 2));

        Generate();

    }

    public void Generate()
    {
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, -1f);
        landHeights = GenerateHeights();
        DrawLand(Manager.planet, Terrarium, landHeights);
        int liquidLevel = (int)(HEIGHT * (Manager.planet.liquidCoverPrecentage / 100f));
        DrawGrass(Manager.planet.grass,Manager.planet.grassUnderWater, liquidLevel, Terrarium);
        DrawFrame(5, Terrarium);
        Terrarium.Apply();

    }

    private void DrawGrass(Planet.Grass grass, Planet.Grass waterGrass, int liquidLevel,Texture2D terraium)
    {

        Debug.Log("water grass = " + waterGrass.type + " | land grass = " + grass.type);

        int grassHeight = 3;
        int grassRootDepth = 3;
        bool isUnderWater = false;
        for (int x = 0; x < landHeights.Length; x++)
        {
            int landHeightPixel = (int)(landHeights[x] * HEIGHT);

            if (landHeightPixel > liquidLevel) {
                isUnderWater = false;
                grassHeight = MakeGrassHeight(grass);
                grassRootDepth = MakeGrassDepth(grass);
            }
            else
            {
                isUnderWater = true;
                grassHeight = MakeGrassHeight(waterGrass);
                grassRootDepth = MakeGrassDepth(waterGrass);

            }

            int MakeGrassDepth(Planet.Grass grass)
            {
                switch (grass.type)
                {
                    case Planet.Grass.GrassType.none: return 0;
                    case Planet.Grass.GrassType.mold: return 3;
                    case Planet.Grass.GrassType.grass: return 2;
                    case Planet.Grass.GrassType.tallGrass: return 3;
                    case Planet.Grass.GrassType.mushroom: return Random.Range(0, 16); //todo

                }

                return 0;


            }

            int MakeGrassHeight(Planet.Grass grass)
            {
                switch (grass.type)
                {
                    case Planet.Grass.GrassType.none: return 0;
                    case Planet.Grass.GrassType.mold: return 3;
                    case Planet.Grass.GrassType.grass: return Random.Range(1,10);
                    case Planet.Grass.GrassType.tallGrass: return Random.Range(2, 27);
                    case Planet.Grass.GrassType.mushroom: return Random.Range(0, 2); //todo

                }

                return 0;

            }




            for (int y = (int)(landHeightPixel - grassRootDepth); y < (landHeightPixel + grassHeight); y++)
            {
                terraium.SetPixel(x, y, isUnderWater? grass.color:waterGrass.color);

            }
        }
    }


    private void DrawFrame(int width, Texture2D screen)
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {

                if (WIDTH-width < x || x- width <= 0 || HEIGHT - width < y || y - width <= 0)
                {
                    screen.SetPixel(x, y, Color.blue);
                }

            }
        }
    }
    private void DrawLand(Planet planet, Texture2D land, float[] landHeights)
    {


        //decide points for rocks / stones / chunks

        const int MAX_STONES = 10000;

        Vector2[] stones = new Vector2[(int)(Mathf.Pow(planet.soil.chunkiness,6) * MAX_STONES)];
        Debug.Log("chunkiness: " + planet.soil.chunkiness + " " + stones.Length + " stones");

        for (int i =0;i< stones.Length; i++)
        {
            stones[i] = new Vector2(Random.Range(0, WIDTH), Random.Range(0, HEIGHT));
        }


        //generate the land and water

        const float MAX_COLOR_VARIATION = 0.1f;
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {

                Color tmpColor = new Color(0,0,0,0);
                if (y <= landHeights[x]*HEIGHT) {

                    tmpColor = new Color(planet.soil.color.r, planet.soil.color.g, planet.soil.color.b);
                    tmpColor.r += Random.Range(-1f, 1f) * MAX_COLOR_VARIATION * planet.soil.colorVariation;
                    tmpColor.g += Random.Range(-1f, 1f) * MAX_COLOR_VARIATION * planet.soil.colorVariation;
                    tmpColor.b += Random.Range(-1f, 1f) * MAX_COLOR_VARIATION * planet.soil.colorVariation;

                }


                else if ( y <= (planet.liquidCoverPrecentage/100f) * HEIGHT)
                {
                    tmpColor = planet.liquid.color;
                    tmpColor.a = planet.liquid.opacity;
                }

                land.SetPixel(x, y, tmpColor);

            }
        }



        //add stones
        for (int i = 0; i < stones.Length; i++)
        {
            Color tmpColor = planet.soil.subColor;

            const int MAX_STONE_SIZE = 5;
            int size = Random.Range(1, 6);
            for (int x = (int)stones[i].x- MAX_STONE_SIZE; x < (int)stones[i].x + MAX_STONE_SIZE; x++)
            {
                for (int y = (int)stones[i].y - MAX_STONE_SIZE; y < (int)stones[i].y + MAX_STONE_SIZE; y++)
                {
                   
                    if (landHeights[Mathf.Clamp(x, 0, WIDTH-1)] >= y/(float)HEIGHT && Vector2.Distance(new Vector2(x, y), stones[i]) < size){

                        tmpColor = planet.soil.subColor;
                        tmpColor.r += Random.Range(-2f, 2f) * MAX_COLOR_VARIATION * planet.soil.subColorVariation;
                        tmpColor.g += Random.Range(-2f, 2f) * MAX_COLOR_VARIATION * planet.soil.subColorVariation;
                        tmpColor.b += Random.Range(-2f, 2f) * MAX_COLOR_VARIATION * planet.soil.subColorVariation;

                        land.SetPixel(Mathf.Clamp(x, 0, WIDTH-1), Mathf.Clamp(y, 0, HEIGHT-1), tmpColor);
                    }

                }
            }





        }



    }


    private float[] GenerateHeights()
    {

        landHeights = new float[WIDTH];



        //first pass

        const float FirstPassWeight = 1f;

        Vector2 initPos = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        Vector2 direction = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        direction.Normalize();
        direction = direction * 0.00166f; //sharpness



        Vector2 pos = initPos;
        for (int i = 0; i < WIDTH; i++)
        {
            pos += direction;
            landHeights[i] = Mathf.PerlinNoise(pos.x, pos.y) * FirstPassWeight;
        }


        //second pass (the same thing, different values)

        const float SecondPassWeight = 0.166f;

        initPos = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        direction = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        direction.Normalize();
        direction = direction * 0.03f; //sharpness



        pos = initPos;
        for (int i = 0; i < WIDTH; i++)
        {
            pos += direction;
            landHeights[i] += Mathf.PerlinNoise(pos.x, pos.y) * SecondPassWeight; //adding and not setting because it's the 2nd pass
        }


        //normalazing


        for (int i = 0; i < WIDTH; i++)
        {
            landHeights[i] = landHeights[i] * (1f/(SecondPassWeight + FirstPassWeight)); //adding and not setting because it's the 2nd pass
        }

        return landHeights;


    }


}
