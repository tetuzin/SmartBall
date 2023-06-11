using System.Collections.Generic;

using Pachinko.Model;
using Pachinko.Const;

namespace Pachinko.Utils
{
    public class PachinkoUtils
    {
        // リーチ、当たり演出が流れる保留が存在するかリストをチェックする
        public static bool CheckDirectHoldList(List<HoldModel> holdList)
        {
            foreach (HoldModel hold in holdList)
            {
                if (CheckDirectHold(hold)) return true;
            }
            return false;
        }

        // リーチ、当たり演出が流れる保留かチェックする
        public static bool CheckDirectHold(HoldModel hold)
        {
            return hold.ValueState == ValueState.PSEUDO || hold.ValueState == ValueState.HIT;
        }

        // テンパイする保留が存在するかリストをチェックする
        public static bool CheckReachHoldList(List<HoldModel> holdList)
        {
            foreach (HoldModel hold in holdList)
            {
                if (CheckReachHold(hold)) return true;
            }
            return false;
        }

        // テンパイする保留かチェックする
        public static bool CheckReachHold(HoldModel hold)
        {
            return hold.ValueState == ValueState.REACH || 
                hold.ValueState == ValueState.HIT ||
                hold.ValueState == ValueState.PSEUDO;
        }

        // 演出の疑似連がいくつ生成されているかを返す
        public static ReachDirectionState GetPseudoCount(DirectionModel direct)
        {
            if (direct.PseudoData == null)
            {
                return ReachDirectionState.REACH_PSEUDO_1;
            }
            else
            {
                if (direct.PseudoData.PseudoData == null)
                {
                    return ReachDirectionState.REACH_PSEUDO_2;
                }
                else
                {
                    if (direct.PseudoData.PseudoData.PseudoData == null)
                    {
                        return ReachDirectionState.REACH_PSEUDO_3;
                    }
                    else
                    {
                        return ReachDirectionState.REACH_PSEUDO_4;
                    }
                }
            }
        }
    }
}

