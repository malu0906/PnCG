﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using Fungus;

public class compareAns : MonoBehaviour {

	public Flowchart flowchart;

	public GameObject SkillFrameStairs;
	public GameObject SkillFramePCH;
	public GameObject SkillStairs;
	public GameObject SkillP;
	public GameObject SkillC;
	public GameObject SkillH;
	public GameObject NumberFrameStairs;
	public GameObject NumberFramePCH1;
	public GameObject NumberFramePCH2;
	public GameObject NumberFrameFP;


	public GameObject SkillChartStairs;
	public GameObject SkillChartP;
	public GameObject SkillChartC;
	public GameObject SkillChartH;
	public GameObject NW;
	public GameObject ansBUt;

	[SerializeField] private Text NumStairsT = null;
	[SerializeField] private Text NumPCH01T = null;
	[SerializeField] private Text NumPCH02T = null;
	[SerializeField] private Text NumFPT = null;
	[SerializeField] private Text Result = null;

	private int currentProblem;
	private bool firstProblem;

	private int currentFunc;

	private int currentN;
	private int currentM;
	private int currentResult;

	public int ansFunc;
	public int ansN;
	public int ansM;
	public int ansResult;
	public int feedbackNum;


	private string jsonString;
	private JsonData jsonData;
	void Start () {
		getFlowchartNums ();
		searchAnswer ();
	}
	void getFlowchartNums(){
		firstProblem = flowchart.GetBooleanVariable ("FirstProblem");
		currentProblem = flowchart.GetIntegerVariable ("CurrentProblem");
	}

	public void searchAnswer(){
		jsonString = File.ReadAllText (Application.dataPath + "/Resources/myJson.json");//(1)
		jsonData = JsonMapper.ToObject (jsonString);//(2)

		ansFunc = (int) jsonData ["ans"] [currentProblem] ["Func"];
		ansN = (int)jsonData ["ans"] [currentProblem] ["N"];
		ansM = (int)jsonData ["ans"] [currentProblem] ["M"];
		ansResult = (int)jsonData ["ans"] [currentProblem] ["Result"];

		Debug.Log ("ansFunc = " + ansFunc);
		Debug.Log ("ansN = " + ansN);
		Debug.Log ("ansM = " + ansM);
		Debug.Log ("ansResult = " + ansResult);
	}

	void setFBNum(){
		flowchart.SetIntegerVariable ("FBNum",feedbackNum);
		int FBNumNow = flowchart.GetIntegerVariable ("FBNum");
		Debug.Log ("FBNumNow = " + FBNumNow);
	}


	void callFlowchartBlock(){
		Block target = flowchart.FindBlock ("AnsFeedBack");
		flowchart.ExecuteBlock (target);
	}

	void compareAnswerFP(){
		if (currentResult == ansResult) {
			feedbackNum = 4;
		} else {
			feedbackNum = 1;
		}
		setFBNum ();
		Debug.Log (feedbackNum);

	}

	void compareAnswerStairs(){
		if (currentFunc != ansFunc) {
			feedbackNum = 1;
		} else if ((currentFunc == ansFunc) && (currentN != ansN)) {
			feedbackNum = 2;
		} else if ((currentFunc == ansFunc) && (currentResult == ansResult)) {
			feedbackNum = 4;
		}
		setFBNum ();
		Debug.Log (feedbackNum);
		callFlowchartBlock ();
	}

	void compareAnswerPCH(){
		if (currentFunc != ansFunc) {
			feedbackNum = 1;
		} else if ((currentFunc == ansFunc) && (currentN != ansN)) {
			feedbackNum = 2;
		} else if ((currentFunc == ansFunc) && (currentN == ansN) && (currentM != ansM) && (currentResult == ansResult)) {
			feedbackNum = 3;
		}else if ((currentFunc == ansFunc) && (currentN == ansN) && (currentM == ansM) && (currentResult == ansResult)) {
			feedbackNum = 4;
		}
		setFBNum ();
		Debug.Log (feedbackNum);
		callFlowchartBlock ();
	}


	public void countResult(){
		currentFunc = GameObject.Find ("Selected").GetComponent<calculator> ().FrameOn;
		if (firstProblem) {
			currentResult = int.Parse (NumFPT.text);
			compareAnswerFP ();
		} else {

			if (currentFunc == 1) {
			
				currentN = int.Parse (NumStairsT.text);	
				currentM = -1;
				currentResult = countStairs (currentN);
				compareAnswerStairs ();
				//PrintStairs (currentResult, currentN);

			} else if (currentFunc == 2) {
			
				currentN = int.Parse (NumPCH01T.text);
				currentM = int.Parse (NumPCH02T.text);
				currentResult = countP (currentN, currentM);
				compareAnswerPCH ();
				//PrintP (currentResult,currentN , currentM);

			} else if (currentFunc == 3) {
			
				currentN = int.Parse (NumPCH01T.text);
				currentM = int.Parse (NumPCH02T.text);
				currentResult = countC (currentN, currentM);
				compareAnswerPCH ();
				//PrintC (currentResult, currentN, currentM);

			} else if (currentFunc == 4) {
			
				currentN = int.Parse (NumPCH01T.text);
				currentM = int.Parse (NumPCH02T.text);
				currentResult = countH (currentN, currentM);
				compareAnswerPCH ();
				//PrintH (currentResult, currentN, currentM, currentN+currentM-1);

			}
		}


		SkillFrameStairs.SetActive (false);
		SkillFramePCH.SetActive (false);
		SkillStairs.SetActive (false);
		SkillP.SetActive (false);
		SkillC.SetActive (false);
		SkillH.SetActive (false);
		NumberFrameStairs.SetActive (false);
		NumberFramePCH1.SetActive (false);
		NumberFramePCH2.SetActive (false);
		NumberFrameFP.SetActive (false);

		NW.transform.Rotate (new Vector3 (NW.transform.rotation.x, NW.transform.rotation.y, -NW.transform.eulerAngles.z));
		NW.SetActive (false);

		SkillChartStairs.SetActive (false);
		SkillChartP.SetActive (false);
		SkillChartC.SetActive (false);
		SkillChartH.SetActive (false);

		SkillChartStairs.GetComponent<Button> ().enabled = false;
		SkillChartP.GetComponent<Button> ().enabled = false;
		SkillChartC.GetComponent<Button> ().enabled = false;
		SkillChartH.GetComponent<Button> ().enabled = false;

		ansBUt.GetComponent<Image> ().enabled = false;
		ansBUt.GetComponent<Button> ().enabled = false;

	}

	int countStairs(int n){
		if (n == 0) {
			return 1;
		} else {
			int ans = n;
			for (int i = 1; i < n; i++) {
				ans *= i;
			}
			return ans;
		}
	}
	int countP(int n, int m){
		int ans = 0;
		ans = countStairs (n) / countStairs (n - m);
		return ans;
	}
	int countC(int n, int m){
		int ans = 0;
		ans = countStairs (n) / (countStairs (n - m) * countStairs(m));
		return ans;
	}
	int countH(int n, int m){
		int ans = 0;
		ans = countC (n + m - 1, m);
		return ans;
	}

	void PrintStairs(int result, int n){
		string p = "";

		p= n+"!\n= ";

		if (n == 0) {
			p += "1";
		} else {

			for (int i = n; i > 1; i--) {
				p += i + " * ";
			}
			p += "1\n= " + result;
		}
		Result.text = p;
	}

	void PrintP(int result, int n, int m){
		string p = "";
		p = "P(" + n + ", " + m + ")\n= " + n + "! / (" + n + " - " + m + ")!\n= " + result;
		Result.text = p;
	}
	void PrintC(int result, int n, int m){
		string p = "";
		p = "C(" + n + ", " + m + ")\n= " + n + "! / (" + m + "! (" + n + " - " + m + ")! )\n= " + result;
		Result.text = p;
	}
	void PrintH(int result, int n, int m, int cn){
		string p = "";
		p = "H(" + n + ", " + m + ")\n= C(" + n + " + " + m + " - 1, " + m + ")= " + "C(" + cn + ", " + m + ")\n= " + cn + "! / (" + m + "! (" + cn + " - " + m + ")! )\n= " + result;
		Result.text = p;
	}
}
