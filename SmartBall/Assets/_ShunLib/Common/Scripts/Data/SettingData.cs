using System;
using UnityEngine;

namespace ShunLib.Data.Setting
{
    [Serializable]
    public class SettingData : BaseData
    {
        [SerializeField] private int _windowWidth = default;
        [SerializeField] private int _windowHeight = default;
        [SerializeField] private float _volumeMaster = default;
        [SerializeField] private float _volumeSE = default;
        [SerializeField] private float _volumeBGM = default;
        [SerializeField] private int _fps = default;

        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }
        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }
        public float VolumeMaster
        {
            get { return _volumeMaster; }
            set { _volumeMaster = value; }
        }
        public float VolumeSE
        {
            get { return _volumeSE; }
            set { _volumeSE = value; }
        }
        public float VolumeBGM
        {
            get { return _volumeBGM; }
            set { _volumeBGM = value; }
        }
        public int Fps
        {
            get { return _fps; }
            set { _fps = value; }
        }

        public void Initialize()
        {
            _windowWidth = 1280;
            _windowHeight = 720;
            _volumeMaster = 100f;
            _volumeSE = 100f;
            _volumeBGM = 100f;
            _fps = 60;
        }
    }
}


