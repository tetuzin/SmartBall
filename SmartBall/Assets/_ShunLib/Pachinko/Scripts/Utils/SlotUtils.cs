using System.Collections.Generic;

using Pachinko.Const;

namespace Pachinko.Slot.Utils
{
    public class SlotUtils
    {
        // 出目を生成
        public static int[] CreateSlotValue(
            int designCount,    // 図柄の数
            int pseudoIndex = -1,   // 疑似連が起こる図柄番号
            ValueState valueState = ValueState.NONE,    // 役
            List<int> nonReachDesigns = default,        // リーチしない図柄リスト
            List<int> nonHitDesigns = default           // 大当たりしない図柄リスト
        )
        {
            int[] values = new int[3];
            System.Random r = new System.Random();

            switch (valueState)
            {
                // はずれ
                case ValueState.NONE:
                    int value = r.Next(0, designCount);
                    values[0] = value;
                    value = r.Next(0, designCount - 1);
                    if (values[0] <= value) value++;
                    values[1] = value;
                    values[2] = r.Next(0, designCount);
                    break;

                // 疑似連
                case ValueState.PSEUDO:
                    if (pseudoIndex <= -1)
                    {
                        int valuePseudo = r.Next(0, designCount);
                        values[0] = valuePseudo;
                        values[1] = valuePseudo;
                        valuePseudo = r.Next(0, designCount - 1);
                        if (values[0] <= valuePseudo) valuePseudo++;
                        values[2] = valuePseudo;
                    }
                    else
                    {
                        int valuePseudo = r.Next(0, designCount - 1);
                        if (pseudoIndex <= valuePseudo) valuePseudo++;
                        values[0] = valuePseudo;
                        values[1] = valuePseudo;
                        values[2] = pseudoIndex;
                    }
                    break;

                // リーチorテンパイ
                case ValueState.REACH:
                    int CreateReachValue()
                    {
                        int createReachValue = r.Next(0, designCount);
                        if (nonReachDesigns != default && nonReachDesigns.Count > 0)
                        {
                            if (nonReachDesigns.Contains(createReachValue)) return CreateReachValue();
                        }
                        return createReachValue;
                    }
                    int valueReach = CreateReachValue();
                    values[0] = valueReach;
                    values[1] = valueReach;
                    valueReach--;
                    if (valueReach < 0) valueReach = designCount - 1;
                    if (pseudoIndex == valueReach)
                    {
                        valueReach += 2;
                        if (valueReach >= designCount) valueReach -= designCount;
                    }
                    values[2] = valueReach;
                    break;
                
                // 当たり
                case ValueState.HIT:
                    int CreateHitValue()
                    {
                        int createHitValue = r.Next(0, designCount);
                        if (nonHitDesigns != default && nonHitDesigns.Count > 0)
                        {
                            if (nonHitDesigns.Contains(createHitValue)) return CreateHitValue();
                        }
                        return createHitValue;
                    }
                    int valueHit = CreateHitValue();
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = valueHit;
                    }
                    break;

                // はずれと同じ
                default:
                    int val = r.Next(0, designCount);
                    values[0] = val;
                    val = r.Next(0, designCount - 1);
                    if (values[0] <= val) val++;
                    values[1] = val;
                    values[2] = r.Next(0, designCount);
                    break;
            }
            return values;
        }
    }
}


