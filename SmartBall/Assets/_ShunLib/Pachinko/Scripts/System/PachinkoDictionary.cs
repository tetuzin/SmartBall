using UnityEngine;

using ShunLib.Dict;

using Pachinko.Const;
using Pachinko.Model;
using Pachinko.HoldView.Icon;

namespace Pachinko.Dict
{
    public class PachinkoDictionary : MonoBehaviour{}

    /// PachinkoManager ///
    [System.Serializable]
    public class ReachDirectionTable : TableBase<string, ReachDirectionModel, ReachDirectionPair>{}

    [System.Serializable]
    public class ReachDirectionPair : KeyAndValue<string, ReachDirectionModel>{}

    [System.Serializable]
    public class NoticeDirectionTable : TableBase<string, NoticeDirectionModel, NoticeDirectionPair>{}

    [System.Serializable]
    public class NoticeDirectionPair : KeyAndValue<string, NoticeDirectionModel>{}

    /// HoldIcon ///
    [System.Serializable]
    public class HoldIconTable : TableBase<HoldIconState, HoldIcon, HoldIconPair>{}

    [System.Serializable]
    public class HoldIconPair : KeyAndValue<HoldIconState, HoldIcon>{}

    /// FinalBattleChargeIcon ///
    [System.Serializable]
    public class FinalBattleChargeIconModelTable : TableBase<FinalBattleChargeIconState, FinalBattleChargeIconModel, FinalBattleChargeIconModelPair>{}

    [System.Serializable]
    public class FinalBattleChargeIconModelPair : KeyAndValue<FinalBattleChargeIconState, FinalBattleChargeIconModel>{}
}


