using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class HelloWorldCSV : MonoBehaviour {
	// Use this for initialization
	void Start () {
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
                string strData = "Zorrito";
                float floatData = 25.63F;//Note it's a float not string
                sw.WriteLine("{0},{1}", strData, floatData.ToString("F2"));
            }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
