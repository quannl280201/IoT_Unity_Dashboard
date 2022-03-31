using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

public class LoginScript : M2MqttUnityClient
{
    // Start is called before the first frame update
    public InputField brokerURLInputField;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public GameObject loginObject;
    public GameObject errorMessageObject;
    public Button loginButton;

    private string brokerURL;
    private string username;
    private string password;
    private bool updateUI = false;
    protected override void Start()
    {
        errorMessageObject.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    protected override void Update()
    {
        brokerURL = brokerURLInputField.GetComponent<InputField>().text;
        username = usernameInputField.GetComponent<InputField>().text;
        password = passwordInputField.GetComponent<InputField>().text;

        SetBrokerAddress(brokerURL);
        SetMqttUsername(username);
        SetMqttPassword(password);
        
        loginButton = loginObject.GetComponent<Button>();
        loginButton.onClick.AddListener(LoginProcess);
    }
    public void SetBrokerAddress(string brokerAddress)
    {
        if (brokerURLInputField && !updateUI)
        {
            this.brokerAddress = brokerAddress;
        }
    }
    public void SetMqttUsername(string _username)
    {
        if (usernameInputField && !updateUI)
        {
            this.mqttUserName = _username;
        }
    }
    public void SetMqttPassword(string _password)
    {
        if (passwordInputField && !updateUI)
        {
            this.mqttPassword = _password;
        }
    }
    protected override void OnConnected()
    {
        base.OnConnected();
        SceneManager.LoadScene(1);
    }
    private void LoginProcess()
    {
        if (brokerAddress != "" && username != "")
        {
            base.Connect();
        }
        if (brokerAddress == "")
        {
            errorMessageObject.GetComponent<Text>().text = "Login information is required";
        }
        if (brokerAddress != "" && username == "")
        {
            errorMessageObject.GetComponent<Text>().text = "Please enter your username";
        }
        else
        {
            
        }
    }
}
