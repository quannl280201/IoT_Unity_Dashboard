using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Dashboard
{
    public class DashboardManager : MonoBehaviour
    {

        [SerializeField]
        private GameObject mainDashboard;
        public static GameObject MainDashboard {get; private set;}    

        [SerializeField]
        private Text station_name;
        [SerializeField]
        private Text temperature;
        [SerializeField]
        private Text humidity;
        public ToggleSwitch led_switch;
        public ToggleSwitch pump_switch;

        public void Update_Status(Status_Data _status_data)
        {
            station_name.text = _status_data.station_name;
            foreach(data_ss _data in _status_data.data_ss)
            {
                switch (_data.ss_name)
                {
                    case "garden_temperature":
                        temperature.text = _data.ss_value + "°C";
                        break;

                    case "garden_humidity":
                        humidity.text = _data.ss_value + "%";
                        break;
                }
                
            }
        }


        public ControlDevice_Data Update_ControlPump_Value(ControlDevice_Data _controlPump)
        {
            _controlPump.device = MainDashboard.GetComponent<DashboardManager>().pump_switch.GetComponent<ToggleSwitch>().deviceName.GetComponent<Text>().text;
            _controlPump.status = MainDashboard.GetComponent<DashboardManager>().pump_switch.GetComponent<ToggleSwitch>().SwitchState == 1 ? "ON" : "OFF";
            return _controlPump;
        }

        public ControlDevice_Data Update_ControlLED_Value(ControlDevice_Data _controlLED)
        {
            _controlLED.device = MainDashboard.GetComponent<DashboardManager>().led_switch.GetComponent<ToggleSwitch>().deviceName.GetComponent<Text>().text;
            _controlLED.status = MainDashboard.GetComponent<DashboardManager>().led_switch.GetComponent<ToggleSwitch>().SwitchState == 1 ? "ON" : "OFF";
            return _controlLED;
        }
        public void LogOut()
        {
            Debug.Log("Log Out!");
            DashboardMqtt.MqttInstance.Disconnect();
            SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
        }
        void Awake(){
            DashboardManager.MainDashboard = mainDashboard;
        }
    }
}