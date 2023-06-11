using System.Collections.Generic;
using UnityEngine;

using PlayFab.ClientModels;

namespace ShunLib.Data.User
{
    // [Serializable]
    public class UserData : BaseData
    {
        [SerializeField] private List<string> _friendIdList = default;// フレンドリスト
        [SerializeField] private UserAccountInfo _userAccountInfo = default;// アカウント情報
        [SerializeField] private List<CharacterInventory> _inventoryList = default;// インベントリリスト
        [SerializeField] private List<CharacterResult> _characterList = default;// キャラクターリスト
        [SerializeField] private PlayerProfileModel _profile = default;// プロフィール情報
        [SerializeField] private List<ItemInstance> _userInventory = default;// ユーザインベントリ
        [SerializeField] private Dictionary<string, int> _virtualCurrency = default;// ゲーム内通貨
        [SerializeField] private string _mailaddress = default;// メアドの場所


        public List<string> FriendIdList
        {
            get { return _friendIdList; }
            set { _friendIdList = value; }
        }

        public UserAccountInfo UserAccountInfo
        {
            get { return _userAccountInfo; }
            set { _userAccountInfo = value; }
        }

        public List<CharacterInventory> InventoryList
        {
            get { return _inventoryList; }
            set { _inventoryList = value; }
        }

        public List<CharacterResult> CharacterList
        {
            get { return _characterList; }
            set { _characterList = value; }
        }

        public PlayerProfileModel Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }

        public List<ItemInstance> UserInventory
        {
            get { return _userInventory; }
            set { _userInventory = value; }
        }

        public Dictionary<string, int> VirtualCurrency
        {
            get { return _virtualCurrency; }
            set { _virtualCurrency = value; }
        }
        public string Mailaddress
        {
            get { return _mailaddress; }
            set { _mailaddress = value; }
        }

        public void Initialize()
        {
            _friendIdList = new List<string>();
            _userAccountInfo = new UserAccountInfo();
            _inventoryList = new List<CharacterInventory>();
            _characterList = new List<CharacterResult>();
            _profile = new PlayerProfileModel();
            _userInventory = new List<ItemInstance>();
            _virtualCurrency = new Dictionary<string, int>();
            _mailaddress = null;
        }
    }
}
