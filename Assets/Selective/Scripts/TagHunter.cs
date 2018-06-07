using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHunter : MonoBehaviour {
	float startTime = Time.time;
    float sumTerrain = 0;
    float sumWater = 0;
    float sumTrees = 0;
    float sumRocks = 0;
    float sumCorrectFoxes = 0;
    float sumIncorrectFoxes = 0;
    float sumSky = 0;
    string foxTag;
    private animalSelector player;
    private GameObject selectedAnimal;
    public GameObject master;
    //private GameObject selectedAnimal;
    //float curTime = Time.time - startTime;
	// Use this for initialization
	// Update is called once per frame

    void start(){
        
    }

	void Update () {
        animalSelector player = master.GetComponent<animalSelector>();
        GameObject selectedAnimal = player.selectedAnimal;
        //GameObject selectedAnimal = player.GetComponent<animalSelector>().selectedAnimal.gameObject.transform.GetChild(0).gameObject;
        //foxTag = player.getFoxTag();
		Ray myRay = new Ray(transform.position,gameObject.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (myRay, out hit) && hit.transform.tag == "Terrain"){            			
            sumTerrain = sumTerrain + ((Time.time - startTime)% 60);            
        }              
        else if(Physics.Raycast (myRay, out hit) && hit.transform.tag == "Water"){
			sumWater = sumWater + ((Time.time - startTime)% 60);           
		}
        else if (Physics.Raycast(myRay, out hit) && hit.transform.tag == "Trees")
        {
            sumTrees = sumTrees + ((Time.time - startTime)% 60);            
        }
        else if (Physics.Raycast(myRay, out hit) && hit.transform.tag == "Rocks")
        {
            sumRocks = sumRocks + ((Time.time - startTime)% 60);            
        }
        else if (Physics.Raycast(myRay, out hit) && hit.transform.tag == "Black")
        {           
            if(selectedAnimal.gameObject.transform.GetChild(0).tag == "Black"){
                sumCorrectFoxes = sumCorrectFoxes + ((Time.time - startTime)% 60);
            }else{
                sumIncorrectFoxes = sumIncorrectFoxes + ((Time.time - startTime)% 60);
            }                        
        }
        else if (Physics.Raycast(myRay, out hit) && hit.transform.tag == "Red")
        {
            if(selectedAnimal.gameObject.transform.GetChild(0).tag == "Red"){
                sumCorrectFoxes = sumCorrectFoxes + ((Time.time - startTime)% 60);
            }else{
                sumIncorrectFoxes = sumIncorrectFoxes + ((Time.time - startTime)% 60);
            }            
        }
        else if (Physics.Raycast(myRay, out hit) && hit.transform.tag == "Fennec")
        {
            if(selectedAnimal.gameObject.transform.GetChild(0).tag == "Fennec"){
                sumCorrectFoxes = sumCorrectFoxes + ((Time.time - startTime)% 60);
                
            }else{
                sumIncorrectFoxes = sumIncorrectFoxes + ((Time.time - startTime)% 60);
            }     
        }
        else if (Physics.Raycast(myRay, out hit) && hit.transform.tag == "Snow")
        {
            if(selectedAnimal.gameObject.transform.GetChild(0).tag == "Snow"){
                sumCorrectFoxes = sumCorrectFoxes + ((Time.time - startTime)% 60);
            }else{
                sumIncorrectFoxes = sumIncorrectFoxes + ((Time.time - startTime)% 60);
                
            }  
        }
        else if (Physics.Raycast(myRay, out hit) && hit.transform.tag == "Corgi")
        {
            if(selectedAnimal.gameObject.transform.GetChild(0).tag == "Corgi"){
                sumCorrectFoxes = sumCorrectFoxes + ((Time.time - startTime)% 60);
            }else{
                sumIncorrectFoxes = sumIncorrectFoxes + ((Time.time - startTime)% 60);
            }
        } 
        else{
            sumSky = sumSky + ((Time.time - startTime)% 60);
        }
    }
    
    public float getTerrain(){
        return (sumTerrain/100);
    }

    public float getWater(){
        return (sumWater/100);
    }

    public float getTrees(){
        return (sumTrees/100);
    }

    public float getRocks(){
        return (sumRocks/100);
    }

    public float getCorrectFoxes(){
        return (sumCorrectFoxes/100);
    }

    public float getIncorrectFoxes(){
        return (sumIncorrectFoxes/100);
    }

    public float getSky(){
        return (sumSky/100);
    }

}
