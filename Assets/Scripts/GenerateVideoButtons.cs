﻿/** Lives on Canvas
 * 
 * This program dynamically loads and configures buttons for each q3d file found in the executable's directory
 */
//TODO
//Use pictures instead of buttons
//Set this up so that we can change the name of the program w/o changing this code
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using QuaternionSoft.Q3D.UnityPlayer;

public class GenerateVideoButtons : MonoBehaviour {
	public Transform buttonPrefab;
	public Transform buttonParent;
	public GameObject debugPrompt;
	public GameObject notDebugPrompt;
	private string[] filePaths;
	public Transform Q3DPrefab;
	public float startingY = 1f;
	private Transform[] buttonList; 
	private Q3DRenderer rendererQ3D;
	public ScrollRayHandler[] rayHandlerList;
	void Start () {
		//Get this script's location
		string appLocation = Application.dataPath;
		//Strip off the last part of the file location which corresponds to where this script is
		if (appLocation.Contains ("/VRtube, Playa_Data")) {
			appLocation = appLocation.Substring (0, appLocation.Length - "/VRtube, Playa_Data".Length);
		} else if (appLocation.Contains ("/Assets")) {
			appLocation = appLocation.Substring (0, appLocation.Length - "/Assets".Length);
		}
		//Get list of .q3d file directories
		filePaths = Directory.GetFiles(appLocation + "/", "*.q3d");
		
		//Start up Q3D prefab
		UnityEngine.Transform Q3DInstance = (Transform)Instantiate(Q3DPrefab, new Vector3(0, startingY, 0), Quaternion.identity);
		rendererQ3D = Q3DInstance.GetComponent<Q3DRenderer> ();
		
		//Make the player inactive to start out with
		rendererQ3D.gameObject.SetActive (false);
		//Turn the list into only the filename
		for (int i=0; i < filePaths.Length; i++){
			filePaths[i] = filePaths[i].Substring(appLocation.Length);
			filePaths[i] = filePaths[i].Substring(1, filePaths[i].Length-".q3d".Length-1);
		}
		
		//make a buttonList for each file
		buttonList = new Transform[filePaths.Length];
		ButtonUtil buttonUtil;
		if (filePaths.Length > 0) {
			for (int i=0; i < filePaths.Length; i++) {
				//Make buttons from prefab, and plop it in the canvas
				buttonList [i] = Instantiate (buttonPrefab, new Vector3 (0, -27 * i + 40, -7), Quaternion.identity) as Transform;
				buttonList [i].SetParent (buttonParent, false);
				//Configure buttonUtil by giving it the info it needs
				buttonList [i].gameObject.SetActive (true);
				buttonUtil = buttonList [i].gameObject.GetComponentInChildren<ButtonUtil> ();
				buttonUtil.debugPrompt = debugPrompt;
				buttonUtil.notDebugPrompt = notDebugPrompt;
				buttonUtil.setMessage (filePaths [i]);
				//Set the button's on click behavior
				buttonUtil.setFile (filePaths [i], rendererQ3D);
				
			}
		} else {
			//if no files are found, tell the user to put the holographic files into the directory
			Transform emptyButton = Instantiate (buttonPrefab, new Vector3 (0,40, -7), Quaternion.identity) as Transform;
			emptyButton.SetParent (buttonParent, false);
			emptyButton.gameObject.SetActive (true);
			buttonUtil = emptyButton.gameObject.GetComponentInChildren<ButtonUtil> ();
			buttonUtil.setMessage ("No file found. \n Put the holographic files into \n" + appLocation + "\n Then restart this program");
			print ("no Q3D files found");
		}
		
	//Then we want to give each scrollRayHandler a list of the current buttons
		if (rayHandlerList != null){
			foreach( ScrollRayHandler scrollRayHandler in rayHandlerList){
				scrollRayHandler.populateButtonList(buttonList);
			}
		}
	}
	
}
/* This file is part of VRtube, Playa.

    VRtube, Playa is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VRtube, Playa is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with VRtube, Playa.  If not, see <http://www.gnu.org/licenses/>.
*/