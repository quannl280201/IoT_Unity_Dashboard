using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

namespace Dashboard
{
    public class Status_Data
    {
        public string project_id { get; set; }
        public string project_name { get; set; }
        public string station_id { get; set; }
        public string station_name { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string volt_battery { get; set; }
        public string volt_solar { get; set; }
        public List<data_ss> data_ss { get; set; }
        public string device_status { get; set; }
    }

    public class data_ss
    {
        public string ss_name { get; set; }
        public string ss_unit { get; set; }
        public string ss_value { get; set; }
    }

    public class Config_Data
    {
        public float temperature_max { get; set; }
        public float temperature_min { get; set; }
        public int mode_fan_auto { get; set; }
    }

    public class ControlDevice_Data
    {
        public string device { get; set; }
        public string status { get; set; }

    }

    public class DashboardMqtt : M2MqttUnityClient
    {
        public static DashboardMqtt MqttInstance;
        public List<string> topics_to_subscribe = new List<string>();
        public List<string> topics_to_publish = new List<string>();


        public string msg_received_from_topic_status = "";
        public string msg_received_from_topic_control = "";


        private List<string> eventMessages = new List<string>();
        public Status_Data _status_data;
        public Config_Data _config_data;
        public ControlDevice_Data _controlPump_data = new ControlDevice_Data();
        public ControlDevice_Data _controlLED_data = new ControlDevice_Data();



        // public void PublishConfig()
        // {
        //     _config_data = new Config_Data();
        //     GetComponent<DashboardManager>().Update_Config_Value(_config_data);
        //     string msg_config = JsonConvert.SerializeObject(_config_data);
        //     client.Publish(topics_to_subscribe[1], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        //     Debug.Log("publish config");
        // }

        public void PublishPumpControl()
        {
            MqttInstance._controlPump_data = DashboardManager.MainDashboard.GetComponent<DashboardManager>().Update_ControlPump_Value(MqttInstance._controlPump_data);
            string msg_config = JsonConvert.SerializeObject(MqttInstance._controlPump_data);
            client.Publish(topics_to_publish[1], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log(msg_config);
        }

         public void PublishLEDControl()
        {
            MqttInstance._controlLED_data = DashboardManager.MainDashboard.GetComponent<DashboardManager>().Update_ControlLED_Value(MqttInstance._controlLED_data);
            string msg_config = JsonConvert.SerializeObject(MqttInstance._controlLED_data);
            client.Publish(topics_to_publish[0], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log(msg_config);
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            SubscribeTopics();
            SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
        }

        protected override void SubscribeTopics()
        {

            foreach (string topic in topics_to_subscribe)
            {
                if (topic != "")
                {
                    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
            }
        }

        protected override void UnsubscribeTopics()
        {
            foreach (string topic in topics_to_subscribe)
            {
                if (topic != "")
                {
                    client.Unsubscribe(new string[] { topic });
                }
            }

        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }




        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg + " from " + topic);
            //StoreMessage(msg);
            if (topic == topics_to_subscribe[0])
                ProcessMessageStatus(msg);

            // if (topic == topics_to_subscribe[2])
            //     ProcessMessageControl(msg);
        }

        private void ProcessMessageStatus(string msg)
        {
            _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
            msg_received_from_topic_status = msg;
            if(DashboardManager.MainDashboard.GetComponent<DashboardManager>() != null){
                DashboardManager.MainDashboard.GetComponent<DashboardManager>().Update_Status(_status_data);
                Debug.Log("Successfully process!");
            }

        }

        // private void ProcessMessageControl(string msg)
        // {
        //     _controlFan_data = JsonConvert.DeserializeObject<ControlFan_Data>(msg);
        //     msg_received_from_topic_control = msg;
        //     GetComponent<DashboardManager>().Update_Control(_controlFan_data);

        // }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            //if (autoTest)
            //{
            //    autoConnect = true;
            //}
        }

        public void UpdateConfig()
        {

        }

        public void UpdateControl()
        {

        }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            MqttInstance = this;
        }
    }
}
