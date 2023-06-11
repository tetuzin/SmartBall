using System.Collections.Generic;
using UnityEngine;

namespace ShunLib.Model.Room
{
    [System.Serializable]
    public class BaseRoomInteriorModel
    {
        // インテリアID
        [SerializeField] private int _interiorId;
        // 座標_X
        [SerializeField] private float _positionX;
        // 座標_Y
        [SerializeField] private float _positionY;
        // 座標_Z
        [SerializeField] private float _positionZ;
        // 向き_X
        [SerializeField] private float _rotateX;
        // 向き_Y
        [SerializeField] private float _rotateY;
        // 向き_Z
        [SerializeField] private float _rotateZ;
        // 向き_W
        [SerializeField] private float _rotateW;
        // サイズ_X
        [SerializeField] private float _scaleX;
        // サイズ_Y
        [SerializeField] private float _scaleY;
        // サイズ_Z
        [SerializeField] private float _scaleZ;
        
        public int InteriorId
        {
            get { return _interiorId; }
            set { _interiorId = value; }
        }
        public float PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }
        public float PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }
        public float PositionZ
        {
            get { return _positionZ; }
            set { _positionZ = value; }
        }
        public float RotateX
        {
            get { return _rotateX; }
            set { _rotateX = value; }
        }
        public float RotateY
        {
            get { return _rotateY; }
            set { _rotateY = value; }
        }
        public float RotateZ
        {
            get { return _rotateZ; }
            set { _rotateZ = value; }
        }
        public float RotateW
        {
            get { return _rotateW; }
            set { _rotateW = value; }
        }
        public float ScaleX
        {
            get { return _scaleX; }
            set { _scaleX = value; }
        }
        public float ScaleY
        {
            get { return _scaleY; }
            set { _scaleY = value; }
        }
        public float ScaleZ
        {
            get { return _scaleZ; }
            set { _scaleZ = value; }
        }
    }

    [System.Serializable]
    public class BaseRoomInteriorModelList
    {
        // インテリアモデルリスト
        [SerializeField] private List<BaseRoomInteriorModel> _list  = new List<BaseRoomInteriorModel>();

        public List<BaseRoomInteriorModel> List
        {
            get { return _list; }
            set { _list = value; }
        }
    }

    [System.Serializable]
    public class InteriorItemModel
    {
        // インテリアID
        [SerializeField] private int _interiorId;
        // 所持数
        [SerializeField] private int _stockCount;
        // 配置数
        [SerializeField] private int _placeCount;

        public int InteriorId
        {
            get { return _interiorId; }
            set { _interiorId = value; }
        }
        public int StockCount
        {
            get { return _stockCount; }
            set { _stockCount = value; }
        }
        public int PlaceCount
        {
            get { return _placeCount; }
            set { _placeCount = value; }
        }
    }

    [System.Serializable]
    public class InteriorItemModelList
    {
        // インテリアアイテムモデルのリスト
        [SerializeField] private List<InteriorItemModel> _interiorItemModelList = new List<InteriorItemModel>();

        public List<InteriorItemModel> List
        {
            get { return _interiorItemModelList; }
            set { _interiorItemModelList = value; }
        }
    }
}