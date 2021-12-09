using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class UIController : MonoBehaviour
{
    [Header("UI Controls")]
    public GameObject panel;
    //public Slider gravityScaleSlider;
    public Text gravityValue;
    public Text information;
    public Text equipedBall;

    public GameController gameController;
    public MyPhysicsSystem myPhysicsSystem;

    public GameObject buttonsBox;
    public GameObject informationBox;

    public Button startButton;


    // Start is called before the first frame update
    void Start()
    {
        //panel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;

        //gravityScaleInputField.text = gravityScaleSlider.value.ToString();
        equipedBall.text = gameController.spherePrefab.name;
        information.text = gameController.spherePrefab.GetComponent<MyPhysicObject>().Mass.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Bounciness.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Friction.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            //panel.SetActive(!panel.activeInHierarchy); // toggle

            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                buttonsBox.SetActive(false);
            }
            else
            {
                buttonsBox.SetActive(true);
            }
        }

    }

    public void OnClickPingPong()
    {
        gameController.spherePrefab = gameController.pingpongBall;
        equipedBall.text = "PingPongBall";
        information.text = gameController.spherePrefab.GetComponent<MyPhysicObject>().Mass.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Bounciness.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Friction.ToString();

    }

    public void OnClickBaseBall()
    {
        gameController.spherePrefab = gameController.baseBall;
        equipedBall.text = "BaseBall";
        information.text = gameController.spherePrefab.GetComponent<MyPhysicObject>().Mass.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Bounciness.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Friction.ToString();
    }

    public void OnClickBasketBall()
    {
        gameController.spherePrefab = gameController.basketBall;
        equipedBall.text = "BasketBall";
        information.text = gameController.spherePrefab.GetComponent<MyPhysicObject>().Mass.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Bounciness.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Friction.ToString();
    }

    public void OnClickBowlingBall()
    {
        gameController.spherePrefab = gameController.bowlingBall;
        equipedBall.text = "BowlingBall";
        information.text = gameController.spherePrefab.GetComponent<MyPhysicObject>().Mass.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Bounciness.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Friction.ToString();
    }

    public void OnClickWoodBox()
    {
        gameController.spherePrefab = gameController.woodBox;
        equipedBall.text = "WoodBox";
        information.text = gameController.spherePrefab.GetComponent<MyPhysicObject>().Mass.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Bounciness.ToString()
            + "\n" + gameController.spherePrefab.GetComponent<MyPhysicObject>().Friction.ToString();
    }

    public void OnChangeGravitySlider(Slider slider)
    {
        gravityValue.text = slider.value.ToString();
        myPhysicsSystem.Gravity = slider.value;
    }


    public void OnOKButtonPressed()
    {
        panel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnClickStart()
    {
        informationBox.SetActive(true);
        startButton.gameObject.SetActive(false);
        myPhysicsSystem.IsStart = true;
    }
}
