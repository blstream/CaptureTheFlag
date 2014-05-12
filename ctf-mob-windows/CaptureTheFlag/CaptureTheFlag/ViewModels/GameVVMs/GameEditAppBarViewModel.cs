﻿using Caliburn.Micro;
using CaptureTheFlag.Messages;
using CaptureTheFlag.Models;
using CaptureTheFlag.Services;
using RestSharp;
using System;
using System.Reflection;
using System.Windows;

namespace CaptureTheFlag.ViewModels.GameVVMs
{
    public class GameEditAppBarViewModel : Screen, IHandle<GameModelMessage>
    {
        private readonly INavigationService navigationService;
        private readonly ICommunicationService communicationService;
        private readonly IGlobalStorageService globalStorageService;
        private readonly IEventAggregator eventAggregator;
        private RestRequestAsyncHandle requestHandle;// TODO: implement abort

        public GameEditAppBarViewModel(INavigationService navigationService, ICommunicationService communicationService, IGlobalStorageService globalStorageService, IEventAggregator eventAggregator)
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "");
            this.navigationService = navigationService;
            this.communicationService = communicationService;
            this.globalStorageService = globalStorageService;
            this.eventAggregator = eventAggregator;

            //TODO: Implement can execute for actions
            UpdateAppBarItemText = "update";
            UpdateIcon = new Uri("/Images/upload.png", UriKind.Relative);

            StartGameAppBarItemText = "start";
            StartGameIcon = new Uri("/Images/share.png", UriKind.Relative);

            DeleteAppBarMenuItemText = "delete";

            IsFormAccessible = true;
        }

        #region ViewModel States
        protected override void OnActivate()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            base.OnActivate();
            eventAggregator.Subscribe(this);
            Authenticator = globalStorageService.Current.Authenticator;
        }

        protected override void OnDeactivate(bool close)
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }
        #endregion

        #region Message handling
        public void Handle(GameModelMessage message)
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            GameModelKey = message.GameModelKey;
            switch (message.Status)
            {
                case GameModelMessage.STATUS.SHOULD_GET:
                    if (!String.IsNullOrEmpty(GameModelKey) && !globalStorageService.Current.Games.ContainsKey(GameModelKey))
                    {
                        ReadAction();
                    }
                    else
                    {
                        eventAggregator.Publish(new GameModelMessage() { GameModelKey = GameModelKey, Status = ModelMessage.STATUS.IN_STORAGE });
                    }
                    break;
                case GameModelMessage.STATUS.UPDATED:
                    PatchGameAction();
                    break;
            }
        }
        #endregion

        #region Actions
        public async void ReadAction()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            IsFormAccessible = false;
            if (Authenticator.IsValid(Authenticator))
            {
                IRestResponse response = await communicationService.GetGameAsync(Authenticator.token, new PreGame() { Url = GameModelKey });
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "READ: {0}", response.Content);
                    PreGame responseGame = new CommunicationService.JsondotNETDeserializer().Deserialize<PreGame>(response);
                    globalStorageService.Current.Games[responseGame.Url] = responseGame;
                    eventAggregator.Publish(new GameModelMessage() { GameModelKey = responseGame.Url, Status = ModelMessage.STATUS.IN_STORAGE });
                }
                else
                {
                    DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "{0}", response.StatusDescription);
                    //TODO: new CommunicationService.JsondotNETDeserializer().Deserialize<ItemErrorType>(response);
                }
                IsFormAccessible = true;
            }
        }

        public async void DeleteAction()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            IsFormAccessible = false;
            if (Authenticator.IsValid(Authenticator))
            {
                IRestResponse response = await communicationService.DeleteGameAsync(Authenticator.token, new PreGame() { Url = GameModelKey });
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "DELETED: {0}", response.Content);
                    PreGame responseGame = new CommunicationService.JsondotNETDeserializer().Deserialize<PreGame>(response);
                    globalStorageService.Current.Games.Remove(GameModelKey);
                    eventAggregator.Publish(new GameModelMessage() { GameModelKey = GameModelKey, Status = ModelMessage.STATUS.DELETED });
                    GameModelKey = null;
                    if (navigationService.CanGoBack)
                    {
                        navigationService.GoBack();
                        navigationService.RemoveBackEntry();
                    }
                    MessageBox.Show("OK", "deleted", MessageBoxButton.OK);
                }
                else
                {
                    DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "{0}", response.StatusDescription);
                    //TODO: new CommunicationService.JsondotNETDeserializer().Deserialize<ItemErrorType>(response);
                }
                IsFormAccessible = true;
            }
        }

        public async void PatchGameAction()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            IsFormAccessible = false;
            if (Authenticator.IsValid(Authenticator))
            {
                PreGame patchGame = globalStorageService.Current.Games[GameModelKey]; //TODO: selective fields
                IRestResponse response = await communicationService.PatchGameAsync(Authenticator.token, patchGame);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "{0}", response.StatusDescription);
                    MessageBox.Show("OK", "updated", MessageBoxButton.OK);
                }
                else
                {
                    DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "{0}", response.StatusDescription);
                    //TODO: new CommunicationService.JsondotNETDeserializer().Deserialize<GameErrorType>(response);
                }
                IsFormAccessible = true;
            }
        }

        public void StartGameAction()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            navigationService
                .UriFor<GeoMapViewModel>()
                .Navigate();
        }

        public void UpdateAction()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            eventAggregator.Publish(new GameModelMessage() { GameModelKey = GameModelKey, Status = ModelMessage.STATUS.UPDATE });
        }
        #endregion

        #region Properties
        #region Model Properties
         private Authenticator authenticator;
        public Authenticator Authenticator
        {
            get { return authenticator; }
            set
            {
                if (authenticator != value)
                {
                    authenticator = value;
                    NotifyOfPropertyChange(() => Authenticator);
                }
            }
        }

        private string gameModelKey;
        public string GameModelKey
        {
            get { return gameModelKey; }
            set
            {
                if (gameModelKey != value)
                {
                    gameModelKey = value;
                    NotifyOfPropertyChange(() => GameModelKey);
                }
            }
        }
        #endregion

        #region UI Properties
        private string deleteAppBarMenuItemText;
        public string DeleteAppBarMenuItemText
        {
            get { return deleteAppBarMenuItemText; }
            set
            {
                if (deleteAppBarMenuItemText != value)
                {
                    deleteAppBarMenuItemText = value;
                    NotifyOfPropertyChange(() => DeleteAppBarMenuItemText);
                }
            }
        }

        private Uri updateIcon;
        public Uri UpdateIcon
        {
            get { return updateIcon; }
            set
            {
                if (updateIcon != value)
                {
                    updateIcon = value;
                    NotifyOfPropertyChange(() => UpdateIcon);
                }
            }
        }

        private Uri startGameIcon;
        public Uri StartGameIcon
        {
            get { return startGameIcon; }
            set
            {
                if (startGameIcon != value)
                {
                    startGameIcon = value;
                    NotifyOfPropertyChange(() => StartGameIcon);
                }
            }
        }

        private string updateAppBarItemText;
        public string UpdateAppBarItemText
        {
            get { return updateAppBarItemText; }
            set
            {
                if (updateAppBarItemText != value)
                {
                    updateAppBarItemText = value;
                    NotifyOfPropertyChange(() => UpdateAppBarItemText);
                }
            }
        }

        private string startGameAppBarItemText;
        public string StartGameAppBarItemText
        {
            get { return startGameAppBarItemText; }
            set
            {
                if (startGameAppBarItemText != value)
                {
                    startGameAppBarItemText = value;
                    NotifyOfPropertyChange(() => StartGameAppBarItemText);
                }
            }
        }

        //TODO: remove when can execute is available
        private bool isFormAccessible;
        public bool IsFormAccessible
        {
            get { return isFormAccessible; }
            set
            {
                if (isFormAccessible != value)
                {
                    isFormAccessible = value;
                    NotifyOfPropertyChange(() => IsFormAccessible);
                }
            }
        }
        #endregion
        #endregion
    }
}
