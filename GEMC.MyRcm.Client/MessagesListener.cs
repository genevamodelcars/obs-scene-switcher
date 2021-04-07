﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GEMC.Common;
using WebSocketSharp;

namespace GEMC.MyRcm.Client
{
    public class MessagesListener : IDisposable
    {
        public EventHandler<SceneInfoEventArgs> SceneChanging;

        private static ILogger logger;
        private WebSocket webSocket;

        public List<Message> Messages { get; set; }

        public MessagesListener(string address, ILogger log)
        {
            logger = log;
            this.webSocket = new WebSocket(address, onMessage: OnMessage, onError: OnError, onClose:OnClose, onOpen:OnOpen);
            this.webSocket.WaitTime = TimeSpan.FromMinutes(5);
        }

        public void Start()
        {
            this.Messages = new List<Message>();
            this.webSocket.Connect().Wait();
        }

        public void Stop()
        {
            this.webSocket.Close(CloseStatusCode.Normal).Wait();
        }

        private Task OnError(ErrorEventArgs errorEventArgs)
        {
            logger.Error(this.GetType(), errorEventArgs.Message, errorEventArgs.Exception);
            return Task.FromResult(0);
        }

        private Task OnMessage(MessageEventArgs messageEventArgs)
        {
            string json = messageEventArgs.Text.ReadToEnd();
            Message newMessage = Message.FromJson(json);

            this.HandlesScenicChange(newMessage);

            this.Messages.Add(newMessage);
            logger.Debug(this.GetType(), json);
            return Task.FromResult(0);
        }

        public void OnSceneChanging(string text)
        {
            this.SceneChanging?.Invoke(this, new SceneInfoEventArgs(text));
        }

        private void HandlesScenicChange(Message newMessage)
        {
            // TODO: Implements the rules to trigger the change
        }

        private Task OnClose(CloseEventArgs closeEventArgs)
        {
            logger.Info(this.GetType(), "Socket closed.");
            return Task.FromResult(0);
        }

        private Task OnOpen()
        {
            logger.Info(this.GetType(), "Socket open.");
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            this.webSocket.Close();
            this.webSocket?.Dispose();
        }
    }
}