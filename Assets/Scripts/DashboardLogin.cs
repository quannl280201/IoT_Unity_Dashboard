using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace Dashboard
{
    public class DashboardLogin : MonoBehaviour
    {
        public static DashboardLogin LoginInstance;
        public InputField brokerURLInputField;
        public InputField usernameInputField;
        public InputField passwordInputField;
        public GameObject loginObject;
        public Text errorMessageObject;
        public Button loginButton;
        private string brokerURL;
        private string username;
        private string password;

        void Start()
        {
            LoginInstance.errorMessageObject.GetComponent<Text>().text = "";
        }
        public void Login()
        {
            Debug.Log("Login Click!");
            errorMessageObject.GetComponent<Text>().text = "";
            brokerURL = LoginInstance.brokerURLInputField.GetComponent<InputField>().text;
            username = LoginInstance.usernameInputField.GetComponent<InputField>().text;
            password = LoginInstance.passwordInputField.GetComponent<InputField>().text;
            
            if(brokerURL != "" && username != "")
            {
                DashboardMqtt.MqttInstance.brokerAddress = brokerURL;
                DashboardMqtt.MqttInstance.mqttUserName = username;
                DashboardMqtt.MqttInstance.mqttPassword = password;
                DashboardMqtt.MqttInstance.Connect();
            }
            if(brokerURL != "" && username == "")
            {
                LoginInstance.errorMessageObject.GetComponent<Text>().text = "Please enter your username!";
            }
            if(brokerURL == "" && username == "")
            {
                LoginInstance.errorMessageObject.GetComponent<Text>().text = "Please enter broker address!";
            }
        }
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            LoginInstance = this;
        }
    }
}
