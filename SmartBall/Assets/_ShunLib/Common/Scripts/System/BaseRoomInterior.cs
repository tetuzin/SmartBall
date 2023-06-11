using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using SoftGear.Strix.Unity.Runtime;

using ShunLib.Const;
using ShunLib.Model.Room;

namespace ShunLib.Room
{
    public class BaseRoomInterior : StrixBehaviour, IPointerClickHandler
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("インテリアID")]
        [SerializeField] protected int _roomInteriorId = default;

        [Header("インテリア名")]
        [SerializeField] protected string _roomInteriorName = "Interior";

        [Header("インテリアサムネイル画像")]
        [SerializeField] protected Sprite _roomInteriorSprite = default;

        [Header("インテリアタイプ")]
        [SerializeField] protected RoomInteriorType _roomInteriorType = RoomInteriorType.NONE;

        [Header("Rendererリスト")]
        [SerializeField] protected List<Renderer> _rendererList = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // 編集モードフラグ
        public bool IsEdit { get; set; }

        // 配置可否フラグ
        public bool IsPlace { get; set; }

        // 左クリック時コールバック
        public Action OnLeftClickCallback { get; set; }

        // 右クリック時コールバック
        public Action OnRightClickCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------

        void Update()
        {
            if (IsEdit)
            {
                Vector3 mousePos = Input.mousePosition;
                this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            }
        }

        // オブジェクトが重なったとき
        void OnTriggerEnter(Collider other) {
            if (!IsEdit) return;
            IsPlace = false;
            ChanceRenderersColor(Color.red);
        }

        // オブジェクトが離れた時
        void OnTriggerExit(Collider other) {
            if (!IsEdit) return;
            IsPlace = true;
            ChanceRenderersColor(Color.blue);
        }

        // ---------- Public関数 ----------

        // 編集可能状態へ移行する処理
        public void EditState()
        {
            IsEdit = true;
            ChanceRenderersColor(Color.blue);
        }

        // 配置処理
        public void Place()
        {
            IsEdit = false;
            ChanceRenderersColor(Color.white);
        }

        // インテリアIDを設定
        public void SetRoomInteriorId(int id)
        {
            _roomInteriorId = id;
        }

        // インテリアIDを取得
        public int GetRoomInteriorId()
        {
            return _roomInteriorId;
        }

        // インテリア名を取得
        public string GetInteriorName()
        {
            return _roomInteriorName;
        }

        // インテリアサムネイル画像を取得
        public Sprite GetInteriorSprite()
        {
            return _roomInteriorSprite;
        }

        // インテリアクリック時の処理
        public void OnPointerClick(PointerEventData eventData)
        {
            switch(eventData.pointerId)
            {
                // 左クリック
                case -1:
                    OnLeftClickCallback?.Invoke();
                    break;

                // 右クリック
                case -2:
                    OnRightClickCallback.Invoke();
                    break;
            }
        }

        // 全てのRendererの色を変える
        public void ChanceRenderersColor(Color color)
        {
            foreach (Renderer r in _rendererList)
            {
                r.material.color = color;
            }
        }

        // シリアライズ可能クラスへ変換
        public BaseRoomInteriorModel ConvertInteriorModel()
        {
            BaseRoomInteriorModel model = new BaseRoomInteriorModel();
            model.InteriorId = _roomInteriorId;
            model.PositionX = this.gameObject.transform.position.x;
            model.PositionY = this.gameObject.transform.position.y;
            model.PositionZ = this.gameObject.transform.position.z;
            model.RotateX = this.gameObject.transform.rotation.x;
            model.RotateY = this.gameObject.transform.rotation.y;
            model.RotateZ = this.gameObject.transform.rotation.z;
            model.RotateW = this.gameObject.transform.rotation.w;
            model.ScaleX = this.gameObject.transform.localScale.x;
            model.ScaleY = this.gameObject.transform.localScale.y;
            model.ScaleZ = this.gameObject.transform.localScale.z;
            return model;
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


