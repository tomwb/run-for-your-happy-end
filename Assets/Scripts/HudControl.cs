using UnityEngine;
using System.Collections;

public class HudControl : MonoBehaviour {

	// arrow
	bool activeArrow = false;
	int arrowDirection = 1;
	float arrowVelocity = 250;

	float width = 0;

	GameObject bar;
	RectTransform barRect;
	GameObject arrow;
	RectTransform arrowRect;
	GameObject redBar;
	RectTransform redBarRect;

	GameObject player;

	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");

		bar = transform.FindChild ("Canvas/AtackBar").gameObject;
		barRect = bar.GetComponent<RectTransform> ();

		arrow = transform.FindChild ("Canvas/AtackBar/AtackBarArrow").gameObject;
		arrowRect = arrow.GetComponent<RectTransform> ();

		redBar = transform.FindChild ("Canvas/AtackBar/RedAtackBar").gameObject;
		redBarRect = redBar.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (activeArrow) {
			AtackBarArrowAnimation ();
			StopArrowAnimation();
		}
	}

	// ativa a barra de ataque
	public void ActivateAtackBar( float dificultPercent ) {
		if (! activeArrow) {
			bar.SetActive (true);

			// pego o tamanho da barra
			width = barRect.rect.width;
			activeArrow = true;
			SetRedAtackBar (dificultPercent);
		}
	}

	public void InativeAtackBar(){
		bar.SetActive (false);
	}

	// anima a setinha da barra de ataque
	void AtackBarArrowAnimation() {
		// defino a velocidade
		float tempArrowVelocity = (arrowVelocity * Time.deltaTime) * arrowDirection;

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
	void StopArrowAnimation () {
		if (Input.GetMouseButtonDown (0)) {
			activeArrow = false;
			CheckArrowPosition();
		}
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
}
