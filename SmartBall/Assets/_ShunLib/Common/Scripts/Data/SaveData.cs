using System;
using UnityEngine;

using ShunLib.Data.Game;
using ShunLib.Data.User;
using ShunLib.Data.Setting;

namespace ShunLib.Data.Save
{
    [Serializable]
    public class SaveData : BaseData
    {
        [SerializeField] private GameData _gameData = default;
        [SerializeField] private UserData _userData = default;
        [SerializeField] private SettingData _settingData = default;

        public GameData Game
        {
            get { return _gameData; }
            set { _gameData = value; }
        }

        public UserData User
        {
            get { return _userData; }
            set { _userData = value; }
        }

        public SettingData Setting
        {
            get { return _settingData; }
            set { _settingData = value; }
        }

        public void Initialize()
        {
            _gameData = new GameData();
            _userData = new UserData();
            _settingData = new SettingData();

            _gameData.Initialize();
            _userData.Initialize();
            _settingData.Initialize();
        }
    }
}
