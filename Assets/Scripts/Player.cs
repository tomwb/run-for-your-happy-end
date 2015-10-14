using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	[Header ("Configurações")]
	public float maxJumpHeight = 2;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	// atack
	bool atackSucess = false;

	Controller2D controller;
	GameObject gameControl;
	
	void Start() {

		gameControl = GameObject.FindGameObjectWithTag("GameController");

		controller = GetComponent<Controller2D> ();
		controller.useColliderFunctions = true;

		// defino a gravidade baseado no tamanho do pulo que quero e no tempo de queda
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}
	
	void Update() {

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;
		float targetVelocityX = input.x * 4;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		// pular com o teclado
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			jump ();
		}
		// quando solto jogo a velocidade minima de pulo, para pular baixo se soltar rapido
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			stopJump ();
		}

		// aplico a gravidade
		velocity.y += gravity * Time.deltaTime;

		// chamo a função que movimenta de verdade levando em conta as colisões
		controller.Move (velocity * Time.deltaTime, input);

		// caso colidir com o chao ou com o teto eu não ando em Y
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			detectEnemy();
		}
	}

	public void jump () {
		// pulo normal
		if (controller.collisions.below) {
			velocity.y = maxJumpVelocity;
		}
	}
	public void stopJump(){
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	void detectEnemy(){
		atackSucess = false;
		gameControl.SendMessage( "ActivateAtackBar", 50f );
	}

	public void setAtackSucess( bool temp ) {
		atackSucess = temp;
	}

	public void RaycastOnCollisionEnter( RaycastHit2D hit ){
		hit.collider.SendMessage("OnPlayerCollisionEnter", gameObject,SendMessageOptions.DontRequireReceiver );

		if ( hit.collider.tag == "Enemy"  ) {
			gameControl.SendMessage( "InativeAtackBar" );

			if ( atackSucess ) {
//				mato o inimigo
				Destroy( hit.collider.gameObject );
			} else {
				// levo dano
			}
		}
	}

	
}