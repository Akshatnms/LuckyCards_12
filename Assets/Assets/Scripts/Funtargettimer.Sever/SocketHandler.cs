using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;
using Com.BigWin.Frontend.Data;

namespace Socket
{

    public class SocketHandler : MonoBehaviour
    {

        [SerializeField] SocketIOComponent socket;
        public static SocketHandler intance;

        public bool isConnected;
        private void Awake()
        {
            if (intance != null)
            {
                Destroy(this.gameObject);
            }
            intance = this;
            socket.On("open", OnConnected);
            socket.On(Constant.OnDisconnect, OnDissconnect);
        }
        private void Start()
        {

        }
        /// <summary>
        /// use this socket method when the request and response route is different
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        /// <param name="json"></param>
        /// <param name="resposeEvent"></param>
        public void SendEvent(string eventName, object data, Action<string> json, string resposeEvent = null)
        {
            if (isApplicationPaused) return;
            var req = JsonConvert.SerializeObject(data);
            print("sending event " + eventName + " req: " + req);
            socket.Emit(eventName, new JSONObject(req));
            eventName = resposeEvent == null ? eventName : resposeEvent;
            socket.On(eventName, (res) =>
            {
                print("received event " + eventName + " res: " + res.data); json(res.data.ToString());
                socket.RemoveRouteAllListners(eventName);
            });
        }

        /// <summary>
        /// Use this socket function when the request and response route are the same
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="json"></param>
        public void SendEvent(string eventName, Action<string> json)
        {
            if (isApplicationPaused) return;
            print("sending event " + eventName);
            socket.Emit(eventName);
            socket.On(eventName, (res) =>
            {
                print("received event " + eventName + " res: " + res.data);
                json(res.data.ToString());
                socket.RemoveRouteAllListners(eventName);
            });

        }
        
        public void SendEvent<T>(string eventName, Action<T> json,Action onSomethingWentWrong) where T:class
        {
            if (isApplicationPaused) return;
            print("sending event " + eventName);
            socket.Emit(eventName);
            socket.On(eventName, (res) =>
            {
                print("received event " + eventName + " res: " + res.data);
                var o = JsonConvert.DeserializeObject<BackEndData<T>>(res.data.ToString());
                o.eventName = eventName;

                if (o.ValidateData(onSomethingWentWrong))
                {
                    json(o.data);
                    return;
                }
                socket.RemoveRouteAllListners(eventName);
            });

        }
        /// <summary>
        /// use this socket when there are only request
        /// </summary>
        /// <param name="eventName"></param>
        public void SendEvent(string eventName)
        {
            //if (isApplicationPaused) return;

            socket.Emit(eventName);

        }

        public void RemoveListners(string eventName)
        {
            Debug.Log(eventName +" event removed ");
            socket.RemoveRouteAllListners(eventName);
        }
        public void SendEvent(string eventName, object o)
        {
            //if (isApplicationPaused) return;
            var req = JsonConvert.SerializeObject(o);
            print("sending event " + eventName + " req: " + req);

            socket.Emit(eventName, new JSONObject(req));

        }


        public void ListenEvent(string eventName, Action<string> result)
        {
            Debug.Log("Listening evnet " + eventName);
            socket.On(eventName, (res) =>
            {

                print("received event " + eventName + " res: " + res.data);
                try
                {

                    if (!string.IsNullOrEmpty(res.data.ToString()))
                        result(res.data.ToString());
                }
                catch (Exception)
                {

                    result(string.Empty);
                    throw;
                }

            });
        }
        public void ListenEvent<T>(string eventName, Action<T> result, Action onSomethingWentWrong) where T : class
        {
            Debug.Log("Listening evnet " + eventName);
            socket.On(eventName, (res) =>
            {
                print("received event " + eventName + " res: " + res.data);
                var o = JsonConvert.DeserializeObject<BackEndData<T>>(res.data.ToString());
                o.eventName = eventName;

                if (o.ValidateData(onSomethingWentWrong))
                {
                    result(o.data);
                    return;
                }
            });
        }
        /// <summary>
        /// This will remove the listner as soon as it gets ther response 
        /// from the server
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="result"></param>
        public void TempListners(string eventName, Action<string> result)
        {
            Debug.Log("Listening evnet " + eventName);
            socket.On(eventName, SocketEvent);
            void SocketEvent(SocketIOEvent e)
            {
                print("received event " + eventName + " res: " + e.data);
                result(e.data.ToString());
                socket.Off(eventName, SocketEvent);
            }
        }

        public void Timer(string eventName, Action<string> result)
        {
            socket.On(eventName, (res) =>
            {
                result(res.data.ToString());
            });
        }
        void OnConnected(SocketIOEvent e)
        {
            print("connected");
            isConnected = true;
            __onConnected?.Invoke();
        }
        Action __onConnected;
        void OnDissconnect(SocketIOEvent e)
        {
            isConnected = false;
            if (isDissconnectingOnPurpose)
            {
                isDissconnectingOnPurpose = false;
                return;
            }
            print("dissconnect");

        }

        bool isDissconnectingOnPurpose;
        public void Disconnect()
        {
            socket.socket.Close();
            isDissconnectingOnPurpose = true;

        }
        public void Connect()
        {
            socket.socket.Connect();
        }

        public bool isApplicationPaused = false;



    }

}