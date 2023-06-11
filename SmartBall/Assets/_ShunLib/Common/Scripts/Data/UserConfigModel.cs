using System;
using UnityEngine;

namespace ShunLib.Model.User
{
    [Serializable]
    public class UserConfigModel : BaseModel
    {
        [SerializeField] private int windowWidth;
        [SerializeField] private int windowHeight;
        [SerializeField] private float volumeSE;
        [SerializeField] private float volumeBGM;
        [SerializeField] private int fps;
        public int WindowWidth
        {
            get { return windowWidth; }
            set { windowWidth = value; }
        }
        public int WindowHeight
        {
            get { return windowHeight; }
            set { windowHeight = value; }
        }
        public float VolumeSE
        {
            get { return volumeSE; }
            set { volumeSE = value; }
        }
        public float VolumeBGM
        {
            get { return volumeBGM; }
            set { volumeBGM = value; }
        }
        public int Fps
        {
            get { return fps; }
            set { fps = value; }
        }
    }
}