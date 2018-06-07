using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class HelloWorldCSV : MonoBehaviour {
	// Use this for initialization

	float sumTerrain = 0;
   	float sumWater = 0;
	float sumTrees = 0;
    	float sumRocks = 0;
    	float sumCorrectFoxes = 0;
    	float sumIncorrectFoxes = 0;
	float sumSky = 0;
	private TagHunter dataCollector;
	public GameObject player;
	public GameObject master;
	private bool sessionState = false;
	private bool timeToExport = false;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		TagHunter dataCollector = player.GetComponent<TagHunter>();
      	gameProggresion endSession = master.GetComponent<gameProggresion>();

		sessionState = endSession.end;

		if(sessionState == true && timeToExport == false){
			sumTerrain = dataCollector.getTerrain();			
			sumWater = dataCollector.getWater();			
			sumTrees = dataCollector.getTrees();			
    			sumRocks = dataCollector.getRocks();
    			sumCorrectFoxes = dataCollector.getCorrectFoxes();
    			sumIncorrectFoxes = dataCollector.getIncorrectFoxes();
			sumSky = dataCollector.getSky();
			timeToExport = true;
			csvMaker();
		}		
	}

	void csvMaker(){
		string mydocpath = "";
		
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
            mydocpath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
		} else {
            mydocpath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			mydocpath = mydocpath+"/Downloads/example.csv";
  		}
		
		Debug.Log(mydocpath);
		//using (TextWriter sw = new StreamWriter("/Users/carlosruiz/Downloads/example.csv"))
		//using (TextWriter sw = new StreamWriter(mydocpath+"/Downloads/example.csv"))
		using (TextWriter sw = new StreamWriter(mydocpath))
            {
			string zorrosCorrectos = "ZorrosCorrectos";
			string zorrosIncorrectos = "ZorrosIncorrectos";
			string arboles = "Arboles";
			string agua = "Agua";
			string terreno = "Terreno";
			string piedras = "Piedras";
			string cielo = "Cielo";					            	
            	//float floatData = 25.63F;//Note it's a float not string
            	//sw.WriteLine("{0},{1}", strData, floatData.ToString("F2"));
			sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", zorrosCorrectos,zorrosIncorrectos,arboles,agua,terreno,piedras,cielo);
			sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", sumCorrectFoxes.ToString(), sumIncorrectFoxes.ToString(), sumTrees.ToString(),sumWater.ToString(), sumTerrain.ToString(), sumRocks.ToString(), sumSky.ToString());

            }
	}
}
