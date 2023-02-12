using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewPackageScript : MonoBehaviour
{

	public GameObject textReview;
	public GameObject buttonReview;
	public Image reviewPanel;
	// YOU CAN DELETE THIS SCRIPT

	public void OnEnable()
	{
		textReview.SetActive(true);
		buttonReview.SetActive(true);
		reviewPanel.enabled = true;
	}
	public void ReviewPackage()
	{
		Application.OpenURL("https://assetstore.unity.com/packages/templates/packs/fps-retro-shooter-complete-kit-209391#reviews");
	}
}
