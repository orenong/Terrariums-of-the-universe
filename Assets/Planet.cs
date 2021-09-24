using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Planet
{
    //there is no Y
    static string[] letters = {"sha","b","c","d","f","g","j","k","l","m","n","p","q","r","s","t","v","w","x","z" };
    static string[] Vowels = { "a","e","i","o","u","a","a","e","au","ea","ee","o","i","o","u","o","i","e","a","a","e","i","o","a" };

    public int seed;
    public string name;
    public float averageTemprature;
    public float liquidCoverPrecentage; //how much of the planet is covered with liquid
    public Soil soil;
    public Liquid liquid;
    public bool hasLife;
    public LifeForm[] lifeForms;
    public Grass grass;
    public Grass grassUnderWater;




    Liquid water = new Liquid("water", new Color(0.066f, 0.648f, 0.906f),0.4f);





    public struct Soil
    {

        public Soil (Color color, float colorVariation, Color subColor, float subColorVariation, float chunkiness){

        
            this.color = color;
            this.colorVariation = colorVariation;
            this.subColor = subColor;
            this.subColorVariation = subColorVariation;
            this.chunkiness = chunkiness;

        }
    

        public Color color;
        public float colorVariation; // 0 to 1

        public Color subColor; //the secondery color, like small stones in the soil
        public float subColorVariation;


        public float chunkiness; //high values - made of large chunks, like huge stones or something, and low value is smooth and powdery


    }

    public struct Grass
    {
         


        public Color color;

        public GrassType type;



        public enum GrassType
        {
            none, mold, grass, tallGrass, mushroom
            // mold should also apear underwater
        }

        public Grass(Color color, GrassType type)
        {

            this.color = color;
            this.type = type;

        }



        public static Grass GenerateGrass(float precentageOfLiquid, Color liquidColor)
        {

            Color color = new Color(Random.Range(-0.1f, 1f),(Random.Range(0f, 1.2f) + Random.Range(0f, 1.2f) / 2f), Random.Range(-0.1f, 1f));
            if (precentageOfLiquid > 5f)
            {
                color = new Color(liquidColor.r + Random.Range(-0.3f, 0.3f), liquidColor.g + Random.Range(-0.1f, 0.5f), liquidColor.b + Random.Range(-0.3f, 0.3f));
            }

            if (precentageOfLiquid <=5f || (color.r == 1f && color.g ==1f && color.b ==1f && Random.Range(0,100) <81))
                /**if the color is completley white there is 80% for reroll and if there is not enough water re roll too
                    re rolled color is random (but higher chance for 1f in green)
                 */
            {
                color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1.3f), Random.Range(0f, 1f));
            }

            GrassType type = GrassType.none;

            if (precentageOfLiquid < 100)
            {
                int luck = Random.Range(-2, 10);
                switch (luck)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3: type = GrassType.grass; break;
                    case 4:
                    case 5:
                    case 6: type = GrassType.tallGrass; break;
                    case 7:
                    case 8: type = GrassType.mold; break;
                    case 9: type = GrassType.mushroom; break;
                    default: break;
                }

            }

            return new Grass(color, type);
        }


    }
    public struct LifeForm
    {

        public LifeForm(string name, bool isCarnivore, int amountOfLimbs, int amountOfEyes, Color[] colors, lifeType type,float size) {

            this.name = name;
            this.isCarnivore = isCarnivore;
            this.amountOfLimbs = amountOfLimbs;
            this.amountOfEyes = amountOfEyes;
            this.colors = colors;
            this.type = type;
            this.size = size;
        
        }



        public string name;

        public bool isCarnivore;
        public float size;
        public int amountOfLimbs;
        public int amountOfEyes;
        public Color[] colors;
        public lifeType type;

        public enum lifeType
        {
            air, Land, liquid
        }


        public static LifeForm GenerateLifeForm(lifeType type,Color enviromentColor)
        {

            string name = GenerateLifeFormName(Random.Range(3, 8));
            int density = 20;
            float size = 0;
            for (int i = 0; i < density; i++)
            {
                size += Random.Range(0, 1f);
            }
            size = size / density;
            int amountOfLimbs = 0;
            density = 30;
            for (int i = 0; i < density; i++)
            {
                amountOfLimbs += (int)Random.Range(-1.5f, 1.66f);
            }
            int amountOfEyes = 0;
            amountOfLimbs *= 2;
            amountOfLimbs = amountOfLimbs < 0 ? 0 : amountOfLimbs;
            density = 10;
            for (int i = 0; i < density; i++)
            {
                amountOfEyes += (int)Random.Range(-5f, 6f);
            }
            amountOfEyes = amountOfEyes / 2;
            amountOfEyes = amountOfEyes < 0 ? 0 : amountOfEyes;

            Color[] colors = new Color[4];
            for (int i = 0; i < 4; i++)
            {
                if (Random.Range(0f,100f) > 57f) //this color will look similar to the enviroment
                {
                    colors[i] = new Color(enviromentColor.r + Random.Range(-0.2f, 0.2f), enviromentColor.g + Random.Range(-0.2f, 0.2f), enviromentColor.b + Random.Range(-0.2f, 0.2f));
                }
                else
                {
                    colors[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                }

            }
            bool isCarnivore = Random.Range(0f, 100f) > 50f;


            return new LifeForm(name, isCarnivore, amountOfLimbs, amountOfEyes, colors, type, size);
        }



        private static string GenerateLifeFormName(int nameLength)
        {
            int lettersInARow = Random.Range(0, 2);
            string nameTmp = "";

            for (int i = 0; i < nameLength; i++)
            {



                if (lettersInARow >= 2 || (lettersInARow == 1 && Random.Range(0f, 100f) < 60f))//no more than 2 letters in a row 60% chance for only 1 letter 
                {

                    nameTmp += Vowels[Random.Range(0, Vowels.Length)];
                    lettersInARow = 0;

                }
                else
                {

                    nameTmp += letters[Random.Range(0, letters.Length)];
                    lettersInARow++;

                }

            }

            return nameTmp;
        }


    }


    public struct Liquid
    {

        public float opacity;
        public string name;
        public Color color;

        public Liquid(string name, Color color, float opacity)
        {

            this.name = name;
            this.color = color;
            this.opacity = opacity;

        }

        public static Liquid GenerateLiquid() {


            Color color = new Color((Random.Range(0, 1f) + Random.Range(0, 1f)) / 2f, Random.Range(0, 1f), Random.Range(0.166f, 1f));
            int nameLength = Random.Range(3, 8);
            string name = GenerateLiquidName(nameLength);
            float opacity = 0.5f;
            opacity += Random.Range(-0.3f, 0.3f);
            opacity += Random.Range(-0.1f, 0.1f);
            opacity += Random.Range(-0.1f, 0.1f);
            opacity = opacity < 0.1f ? 0.1f : opacity;

            return new Liquid(name, color, opacity);
        
        }




        private static string GenerateLiquidName(int nameLength)
        {
            int lettersInARow = Random.Range(0, 2);
            string nameTmp = "";

            for (int i = 0; i < nameLength; i++)
            {



                if (lettersInARow >= 2 || (lettersInARow == 1 && Random.Range(0f, 100f) < 60f))//no more than 2 letters in a row 60% chance for only 1 letter 
                {

                    nameTmp += Vowels[Random.Range(0, Vowels.Length)];
                    lettersInARow = 0;

                }
                else
                {

                    nameTmp += letters[Random.Range(0, letters.Length)];
                    lettersInARow++;

                }

            }

            return nameTmp;
        }


    }


    public Planet(int seed)
    {

        Random.InitState(seed);
        this.seed = seed;
        averageTemprature = GenerateAverageTemprature();
        liquidCoverPrecentage = Mathf.Clamp(Random.Range(-5f, 110f), 0 ,100);
        soil = GenerateSoil(seed, averageTemprature);

        if (Random.Range(0f, 100f) > 50f || averageTemprature>119f || averageTemprature < -9f)
        {
            liquid = Liquid.GenerateLiquid();
        }
        else//water is the liquid of the planet
        {
            liquid = water;
        }
        if (liquidCoverPrecentage == 0)//if there is no liquids on the planet
        {
            liquid = new Liquid("None", Color.black,1);
        }
        hasLife = Random.Range(0f, Mathf.Abs(averageTemprature - 18f) + Random.Range(-166, 300f)) < 166f;
        name = GenerateName(hasLife);


        grass = Grass.GenerateGrass(liquidCoverPrecentage,liquid.color);
        grassUnderWater = Grass.GenerateGrass(liquidCoverPrecentage, liquid.color);



        int counter =0;
        if (hasLife && liquidCoverPrecentage > 0)
        {
            lifeForms = new LifeForm[6];
            lifeForms[0] = LifeForm.GenerateLifeForm(LifeForm.lifeType.liquid, liquid.color);
            lifeForms[1] = LifeForm.GenerateLifeForm(LifeForm.lifeType.liquid, liquid.color);
            counter = 2;
        }
        else if (hasLife)
        {

            lifeForms = new LifeForm[4];

        }

        if (hasLife)
        {
            lifeForms[counter] = LifeForm.GenerateLifeForm(LifeForm.lifeType.Land, soil.color);
            lifeForms[counter + 1] = LifeForm.GenerateLifeForm(LifeForm.lifeType.Land, soil.color);
            lifeForms[counter + 2] = LifeForm.GenerateLifeForm(LifeForm.lifeType.air, grass.color);
            lifeForms[counter + 3] = LifeForm.GenerateLifeForm(LifeForm.lifeType.air, grass.color);
        }

    }


    private Soil GenerateSoil(int seed, float temprature) {

        Soil tmpSoil = new Soil();

        float Brightness = Random.Range(0.166f, 0.8f);
        Color color = new Color(Brightness * Random.Range(0.7f,1.2f), Brightness * Random.Range(0.7f, 1.2f), (Random.Range(0, 0.5f) - ((temprature - 20) / Random.Range(1f, 2000f))));

        tmpSoil.color = color;

        tmpSoil.colorVariation = Random.Range(0, 1f);
        tmpSoil.subColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        tmpSoil.subColorVariation = Random.Range(0, 1f);
        tmpSoil.chunkiness = (Random.Range(0, 1f) + Random.Range(0, 1f) + Random.Range(0, 1f) + Random.Range(0, 1f) + Random.Range(0, 1f)) / 5f;
        Debug.Log("chunkiness " + tmpSoil.chunkiness);
        return tmpSoil;

    }

    private float GenerateAverageTemprature() {
        const float ABSOLUTE_ZERO = -273.15f;
        const float MEDIAN_TEMPRATURE = 18f;
        const int AVERAGE_TEMPRATURE_CONSISTENCY = 20; //The higher the number, the more similar the results will be
        float averageTempratureTmp = 0;
        for (int i = 0; i < AVERAGE_TEMPRATURE_CONSISTENCY; i++)
        {
            averageTempratureTmp += Random.Range(ABSOLUTE_ZERO, (-ABSOLUTE_ZERO) + MEDIAN_TEMPRATURE);
        }
        averageTempratureTmp = averageTempratureTmp / (float)AVERAGE_TEMPRATURE_CONSISTENCY; //average the values
        averageTempratureTmp = averageTempratureTmp < ABSOLUTE_ZERO ? ABSOLUTE_ZERO : averageTempratureTmp; //temprature can't be less than absolute zero
        return averageTempratureTmp;

    }

    private string GenerateName(bool hasLife)
    {

        int nameLength = Random.Range(4, 14);
        int lettersInARow = Random.Range(0, 2); //this is not always 0 to give chance for names starting with a vowel
        string nameTmp = "";
        for (int i = 0; i < nameLength; i++)
        {



            if (lettersInARow >= 2 || (lettersInARow == 1 && Random.Range(0f, 100f) < 80f))//no more than 2 letters in a row 80% chance for only 1 letter 
            {

                nameTmp += Vowels[Random.Range(0, Vowels.Length)];
                lettersInARow = 0;

            }
            else
            {

                nameTmp += letters[Random.Range(0, letters.Length)];
                lettersInARow++;

            }

            if (i == 0)
            {
                nameTmp = nameTmp.ToUpper();

            }

            if (Random.Range(0, 100f) < 5f)//add a number;
            {

                nameTmp += "-" + Random.Range(0, 999) + " ";

            }
            else if (Random.Range(0, 100f) < 10f)
            {//add a space or a hyphen

                if (Random.Range(0, 100f) > 50f)
                {
                    nameTmp += "-";

                }
                else
                {

                    nameTmp += " ";
                }


            }




        }

        if (nameTmp[nameTmp.Length-1] == ' ' || nameTmp[nameTmp.Length - 1] == '-')
        {
            int random = Random.Range(1, 11);
            switch (random)
            {
                case 1: nameTmp += hasLife?"(Planet currently at war)": "(Life destroyed by war)"; break;
                case 2: nameTmp += "Alpha"; break;
                case 3: nameTmp += "Beta"; break;
                case 4: nameTmp += "Extra"; break;
                case 5: nameTmp += "Minor"; break;
                case 6: nameTmp += "Main"; break;
                case 7: nameTmp += "I"; break;
                case 8: nameTmp += "II"; break;
                case 9: nameTmp += "III"; break;
                case 10: nameTmp += "IV"; break;
            }
        }
        return nameTmp;

    }


}
