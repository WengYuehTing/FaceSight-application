using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthManager : MonoBehaviour {

	private const int FIELD_SIZE = 14;

	private const string MAP = "00000000000000," +
	                           "01110111110110," +
	                           "01111100110100," +
	                           "01110000100110," +
	                           "00000111110110," +
	                           "01111100000110," +
	                           "00010101011110," +
	                           "01000101010110," +
	                           "01111111110110," +
	                           "00000100000110," +
	                           "01110000011110," +
	                           "01111111111110," +
	                           "01110000011110," +
	                           "00000000000000";
	
	public MoverioUnityPlugin moverioUnityPlugin;

	public GameObject mainCamera;

	public GameObject field;

	private GameObject player;
	public GameObject playerPrefab;

	public GameObject wall;
	public GameObject wallPrefab;

	public GameObject targetPrefab;
	public GameObject signboardPrefab;
	public GameObject trickWallPrefab;
	public GameObject fencePrefab;

	public GameObject goalPrefab;

	public GameObject cautionView;
	public GameObject titleView;
	public GameObject playView;
	public GameObject pauseView;
	public GameObject endView;
	public GameObject fadeInOut;

	public AudioSource correct;
	public AudioSource inCorrect;
	public AudioSource swing;
	public AudioSource shot;

	public Material[] signboardMaterials;
	public Material[] targetMaterials;

	private List<GameObject> fieldItemList;
	private List<GameObject> trickWallList;
	private List<GameObject> orderTargetList;

	private List<int> hitFlagList;

	private int wallCount;

	private GameState gameState;

	private enum GameState
	{
		Title,
		Play,
		Pause,
		End
	}

	private WallClear wallClear;

	private enum WallClear
	{
		None,
		FirstWall,
		SecondWall,
		ThirdWall,
		LastWall
	}

	///------------------------------------------------------------------------------------------------
		
	void Start () {

		moverioUnityPlugin.SensorData_HeadSet_Tap += SensorData_HeadSet_Tap;
		moverioUnityPlugin.SensorData_Controller_Shake += SensorData_Controller_Shake;

		CreateLabyrintWall ();
		Caution ();

		wallClear = WallClear.None;

	}

	void OnDisable(){

		moverioUnityPlugin.SensorData_HeadSet_Tap -= SensorData_HeadSet_Tap;
		moverioUnityPlugin.SensorData_Controller_Shake -= SensorData_Controller_Shake;

	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			AppFinish ();
		}

	}

	public void PauseButton(){
		
		player.GetComponentInChildren<HeadSetWalkFilter> ().enabled = false;
		player.GetComponentInChildren<HeadSetTracking> ().enabled = false;

		gameState = GameState.Pause;
		pauseView.SetActive (true);

	}

	public void ReStartButton(){

		player.GetComponentInChildren<HeadSetWalkFilter> ().enabled = true;
		player.GetComponentInChildren<HeadSetTracking> ().enabled = true;

		pauseView.SetActive (false);

		gameState = GameState.Play;

	}

	public void TitleButton(){

		StartCoroutine (PauseToTitle());

	}

	///------------------------------------------------------------------------------------------------


	public void Caution(){

		gameState = GameState.Title;
		cautionView.SetActive (true);

	}

	public void Accept(){

		StartCoroutine (CautionToTitle());

	}

	IEnumerator CautionToTitle(){

		fadeInOut.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		cautionView.SetActive (false);
		titleView.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		fadeInOut.SetActive (false);

	}

	public void PlayStart(){

		gameState = GameState.Play;

		StartCoroutine (TitleToPlay());

	}

	IEnumerator TitleToPlay(){

		fadeInOut.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		titleView.SetActive (false);
		playView.SetActive (true);

		mainCamera.SetActive (false);

		CreateLabyrintField ();

		yield return new WaitForSeconds (1.5f);

		fadeInOut.SetActive (false);

	}

	public void PlayEnd(){

		gameState = GameState.End;

		StartCoroutine (EndToTitle());

	}

	IEnumerator EndToTitle(){

		playView.SetActive (false);
		endView.SetActive (true);
		player.GetComponentInChildren<HeadSetWalkFilter> ().enabled = false;
		player.GetComponentInChildren<HeadSetTracking> ().enabled = false;

		yield return new WaitForSeconds (3.0f);

		fadeInOut.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		endView.SetActive (false);
		titleView.SetActive (true);

		ClearLabyrintField ();

		mainCamera.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		fadeInOut.SetActive (false);

		gameState = GameState.Title;

	}

	IEnumerator PauseToTitle(){

		pauseView.SetActive (false);
		playView.SetActive (false);
		fadeInOut.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		endView.SetActive (false);
		titleView.SetActive (true);

		ClearLabyrintField ();

		mainCamera.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		fadeInOut.SetActive (false);

		gameState = GameState.Title;

	}

	///------------------------------------------------------------------------------------------------
		
	public void TargetHit(int flag){

		HitFlagManagement (flag);

	}

	private void HitFlagManagement(int flag){

		switch (wallClear) {
		case WallClear.None:

			correct.Play ();
			TrickWallAnimation ();

			wallClear = WallClear.FirstWall;

			break;
		case WallClear.FirstWall:

			correct.Play ();
			TrickWallAnimation ();

			hitFlagList = new List<int> ();

			wallClear = WallClear.SecondWall;

			break;
		case WallClear.SecondWall:

			hitFlagList.Add (flag);

			if(hitFlagList.Count >= 2){

				correct.Play ();
				TrickWallAnimation ();

				hitFlagList = new List<int> ();
				wallClear = WallClear.ThirdWall;
			}

			break;

		case WallClear.ThirdWall:

			hitFlagList.Add (flag);

			if(hitFlagList.Count >= 3){
				if (hitFlagList [0] == 5 && hitFlagList [1] == 4 && hitFlagList [2] == 6) {
					correct.Play ();
					TrickWallAnimation ();
				} else {

					inCorrect.Play ();

					foreach(GameObject go in orderTargetList){
						go.GetComponent<Animator> ().SetTrigger ("ReSet");
						hitFlagList = new List<int> ();
					}
				}
			}

			break;
		}
	}

	public void Goal(){

		PlayEnd ();

	}

	private void TrickWallAnimation(){

		trickWallList [wallCount].GetComponent<Animator> ().SetTrigger ("Down");
		wallCount++;

	}

	///------------------------------------------------------------------------------------------------

	private void CreateLabyrintField(){

		fieldItemList = new List<GameObject> ();

		player = Instantiate (playerPrefab, new Vector3 (2, playerPrefab.transform.position.y, 2), Quaternion.identity);
		player.transform.parent = field.transform;

		CreateGoal ();
		CreateSignboard ();
		CreateTarget ();
		CreateTrickWall ();
		CreateFence ();
	
	}

	private void ClearLabyrintField(){

		wallCount = 0;
		wallClear = WallClear.None;

		Destroy (player);

		foreach(GameObject go in fieldItemList){
			Destroy (go);
		}

	}

	void CreateLabyrintWall(){

		string[] matrixs = MAP.Split(',');

		for(int x = 0; x < FIELD_SIZE; x++){
			string matrix_X = matrixs[x];
			for(int z = 0; z < FIELD_SIZE; z++){
				int index = int.Parse(matrix_X.Substring(z, 1));
				if(index == 0){
					GameObject prefab = Instantiate (wallPrefab, new Vector3 (x, 0, z), Quaternion.identity);
					prefab.transform.parent = wall.transform;
				}
			}
		}
	}

	private void CreateGoal(){

		float y = goalPrefab.transform.position.y;

		GameObject prefab = AddPrefab (goalPrefab, new Vector3 (11, y, 2), Quaternion.identity);
		prefab.GetComponent<Goal> ().Setting(Goal);

	}

	private void CreateSignboard(){

		List<GameObject> signboardList = new List<GameObject>();

		float y = signboardPrefab.transform.position.y;

		signboardList.Add(AddPrefab (signboardPrefab, new Vector3 (1, y, 9), Quaternion.identity));
		signboardList.Add(AddPrefab (signboardPrefab, new Vector3 (6, y, 3), Quaternion.Euler(new Vector3(0,90,0))));
		signboardList.Add(AddPrefab (signboardPrefab, new Vector3 (6, y, 12), Quaternion.identity));

		for (int i = 0; i < signboardList.Count; i++) {
			signboardList[i].GetComponent<Signboard> ().Setting(signboardMaterials[i]);
		}

	}

	private void CreateTarget(){

		orderTargetList = new List<GameObject> ();

		List<GameObject> targetList = new List<GameObject>();

		float y = targetPrefab.transform.position.y;

		targetList.Add(AddPrefab (targetPrefab, new Vector3 (2, y, 9), Quaternion.identity));
		targetList.Add(AddPrefab (targetPrefab, new Vector3 (5, y, 1), Quaternion.Euler(new Vector3(0,180,0))));
		targetList.Add(AddPrefab (targetPrefab, new Vector3 (6, y, 7), Quaternion.Euler(new Vector3(0,-90,0))));
		targetList.Add(AddPrefab (targetPrefab, new Vector3 (7, y, 1), Quaternion.Euler(new Vector3(0,-90,0))));
		targetList.Add(AddPrefab (targetPrefab, new Vector3 (1, y, 12), Quaternion.identity));
		targetList.Add(AddPrefab (targetPrefab, new Vector3 (5, y, 12), Quaternion.identity));
		targetList.Add(AddPrefab (targetPrefab, new Vector3 (12, y, 12), Quaternion.Euler(new Vector3(0,90,0))));

		for (int i = 0; i < targetList.Count; i++) {
			targetList[i].GetComponent<Target> ().Setting(i,TargetHit,targetMaterials[i]);

			if(i >= 4){
				orderTargetList.Add(targetList[i]);
			}
		}
	}

	private void CreateTrickWall(){

		trickWallList = new List<GameObject> ();

		float y = trickWallPrefab.transform.position.y;

		trickWallList.Add(AddPrefab (trickWallPrefab, new Vector3 (3, y, 8), Quaternion.identity));
		trickWallList.Add(AddPrefab (trickWallPrefab, new Vector3 (6, y, 5), Quaternion.identity));
		trickWallList.Add(AddPrefab (trickWallPrefab, new Vector3 (7, y, 9), Quaternion.identity));
		trickWallList.Add(AddPrefab (trickWallPrefab, new Vector3 (11, y, 8), Quaternion.identity));

	}

	private void CreateFence(){

		float y = fencePrefab.transform.position.y;

		AddPrefab (fencePrefab, new Vector3 (5, y, 2), Quaternion.Euler(new Vector3(0,180,0)));
		AddPrefab (fencePrefab, new Vector3 (7, y, 7), Quaternion.Euler(new Vector3(0,-90,0)));
		AddPrefab (fencePrefab, new Vector3 (11 + 0.25f, y, 11 + 0.25f), Quaternion.Euler(new Vector3(0,45,0)));
		AddPrefab (fencePrefab, new Vector3 (11, y, 12), Quaternion.Euler(new Vector3(0,90,0)));
		AddPrefab (fencePrefab, new Vector3 (12, y, 11), Quaternion.Euler(new Vector3(0,0,0)));

	}

	private GameObject AddPrefab(GameObject prefab,Vector3 position,Quaternion quaternion){

		GameObject addPrefab = Instantiate (prefab, position, quaternion);
		addPrefab.transform.parent = field.transform;
		fieldItemList.Add (addPrefab);

		return addPrefab;

	}

	///------------------------------------------------------------------------------------------------

	private void SensorData_Controller_Shake(float speed){

		if(gameState == GameState.Play){
			Animator animator = player.GetComponentInChildren<Animator> ();
			animator.SetTrigger ("Swing");

			swing.Play ();

			StartCoroutine (WaitWalk());

		}

	}
		
	private void SensorData_HeadSet_Tap(){

		if (gameState == GameState.Play) {
			StartCoroutine (WaitWalk ());

			shot.Play ();

			player.GetComponentInChildren<Shoot> ().Fire ();
		}

	}

	IEnumerator WaitWalk(){

		Rigidbody rigidbody = player.GetComponent<Rigidbody> ();

		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		player.GetComponent<HeadSetWalkFilter> ().enabled = false;

		yield return new WaitForSeconds (1.0f);

		player.GetComponent<HeadSetWalkFilter> ().enabled = true;

	}

	///------------------------------------------------------------------------------------------------

	public void AppFinish(){

		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
			AndroidJavaObject alertDialogBuilder = new AndroidJavaObject ("android.app.AlertDialog$Builder", activity);
			alertDialogBuilder.Call<AndroidJavaObject> ("setMessage", "Do you want to quit the app?");
			alertDialogBuilder.Call<AndroidJavaObject> ("setTitle", "Confirmation");
			alertDialogBuilder.Call<AndroidJavaObject> ("setPositiveButton", "OK", new PositiveButtonListner (this));
			alertDialogBuilder.Call<AndroidJavaObject> ("setNegativeButton", "Cancel", null);
			AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject> ("create");
			dialog.Call ("show");
		}));

	}

	private class PositiveButtonListner :AndroidJavaProxy{
		public PositiveButtonListner(LabyrinthManager d): base("android.content.DialogInterface$OnClickListener"){

		}

		public void onClick(AndroidJavaObject obj, int value){
			Application.Quit();
		}
	}
		
}
