using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudControl : MonoBehaviour {

	public int lifes = 3;

	// arrow
	bool activeArrow = false;
	int arrowDirection = 1;

	float width = 0;

	GameObject bar;
	RectTransform barRect;
	GameObject arrow;
	RectTransform arrowRect;
	GameObject redBar;
	RectTransform redBarRect;

	GameObject player;
	
	List<GameObject> hudLifes = new List<GameObject>();

	
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;

		player = GameObject.FindGameObjectWithTag("Player");

		bar = transform.FindChild ("Canvas/AtackBar").gameObject;
		barRect = bar.GetComponent<RectTransform> ();

		arrow = transform.FindChild ("Canvas/AtackBar/AtackBarArrow").gameObject;
		arrowRect = arrow.GetComponent<RectTransform> ();

		redBar = transform.FindChild ("Canvas/AtackBar/RedAtackBar").gameObject;
		redBarRect = redBar.GetComponent<RectTransform> ();

		GameObject heart = transform.FindChild ("Canvas/HudHeart").gameObject;

		for (int i = 0; i < lifes; i++) {
			GameObject instanciate = Instantiate (heart, heart.transform.position, heart.transform.rotation) as GameObject;
			instanciate.transform.parent = transform.FindChild ("Canvas/HudHearts");
			instanciate.SetActive (true);

			RectTransform instanciateBarRect = instanciate.GetComponent<RectTransform> ();
			instanciateBarRect.anchoredPosition = new Vector2 ( instanciateBarRect.anchoredPosition.x + ( i * 30 ), instanciateBarRect.anchoredPosition.y );

			hudLifes.Add( instanciate );
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (activeArrow) {
			AtackBarArrowAnimation ();
		}
	}

	// ativa a barra de ataque
	public void ActivateAtackBar( float dificultPercent ) {
		if ( ! activeArrow ) {
			bar.SetActive (true);

			// pego o tamanho da barra
			width = barRect.rect.width;
			activeArrow = true;
			SetRedAtackBar (dificultPercent);
		}
	}

	public void InativeAtackBar(){
		activeArrow = false;
		bar.SetActive (false);
	}

	// anima a setinha da barra de ataque
	void AtackBarArrowAnimation() {
		// defino a velocidade
		float tempArrowVelocity = (width * Time.deltaTime) * arrowDirection;

		// ando com a seta
		arrowRect.anchoredPosition = new Vector2 ( arrowRect.anchoredPosition.x + tempArrowVelocity, arrowRect.anchoredPosition.y );

		// mudo a direção da seta
		if ( arrowRect.anchoredPosition.x >=  (width + 15f) ) {
			arrowDirection = -1;
		} else if ( arrowRect.anchoredPosition.x <= 15f ) {
			arrowDirection = 1;
		}
	}

	// para a animação
	public void StopArrowAnimation () {
		activeArrow = false;
		CheckArrowPosition();
	}

	// confirma se a setinha esta dentro da area vermelha
	void CheckArrowPosition() {
		float arrowPosition = arrowRect.anchoredPosition.x - 15f;

		if ( arrowPosition >= redBarRect.anchoredPosition.x 
		    && arrowPosition <= redBarRect.anchoredPosition.x + redBarRect.rect.width ) {
			player.SendMessage( "setAtackSucess", true );
		}
	}

	// seta uma area vermelha
	void SetRedAtackBar( float dificultPercent ) {

		float size = (width * dificultPercent) / 100;
		redBarRect.sizeDelta = new Vector2(size, redBarRect.sizeDelta.y);

		float position = Random.Range(0, width - size);
		redBarRect.anchoredPosition = new Vector2 ( position, redBarRect.anchoredPosition.y);
	}

	void ApplyDamage() {
		lifes--;

		if ( lifes == 0 ){
			Application.LoadLevel ( Application.loadedLevel );
		}

		if ( hudLifes[lifes] ) {
			hudLifes[lifes].SetActive(false);
		}

	}
}
