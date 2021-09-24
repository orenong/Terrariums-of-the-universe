using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    
    [SerializeField] private InputField planetIDInput;
    [SerializeField] private Slider seedChooser;
    const int NO_PLANET_ID = -2000000002;
    int planetID;
    private int lastPlanetID;

    [SerializeField] Text planetSum;

    public Manager manager;
    static Planet planet;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        planetID = NO_PLANET_ID;
        planet = null;
    }

    private void Update()
    {

        if (long.TryParse(planetIDInput.text, out long tmp) )
        {
            if (long.Parse(planetIDInput.text) > 4000000000)
            {
                planetIDInput.text = "4000000000";
            }else if (long.Parse(planetIDInput.text) < 0)
            {
                planetIDInput.text = "0";
            }

            lastPlanetID = planetID;
            planetID = (int)((long.Parse(planetIDInput.text) - 2000000000));
        }

        if (lastPlanetID != planetID && planetID != NO_PLANET_ID)
        {

            planet = new Planet(planetID);
            planetSum.text = GenerateSum(planet);


        }


    }

    string GenerateSum(Planet planet)
    {
        string sum = "The planet of "+planet.name;



        return sum;

    } 

    public void GeneratePlanet() {
        
        if (planetID != NO_PLANET_ID && planetID != NO_PLANET_ID)
        {
            planet = new Planet(planetID);
        }

    }
    public void Randomize()
    {
        long tmp = ((long)Random.Range(-2000000000, 2000000001));
        planetIDInput.text = "" + (tmp + 2000000000L);

    }


    public void GetTerrarium()
    {
        if (planet != null)
        {

            Manager.planet = planet;
            SceneManager.LoadScene("Terrarium2D");
        }

        else {
            Debug.LogError("No seed for planet or planet did not generate");
        }

    }






}
