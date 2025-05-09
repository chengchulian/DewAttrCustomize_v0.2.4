using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Mirror;
using UnityEngine;  
public class PubBuyUtil
{
    // 从消息中获取玩家
    public static DewPlayer GetPlayer(ChatManager.Message obj)
    {
        if (obj.args == null || obj.args.Length == 0)
        {
            return null;
        }

        if (!uint.TryParse(obj.args[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var _))
        {
            return null;
        }

        return ((Component)NetworkClient.spawned[uint.Parse(obj.args[0], CultureInfo.InvariantCulture)])
            .GetComponent<DewPlayer>();
    }

    // 验证玩家是否可以执行操作
    public static bool ValidatePlayer(DewPlayer player, bool checkAlive = false)
    {
        if (player == null)
        {
            BroadcastMessage("无效玩家!");
            return false;
        }

        if (checkAlive && player.hero.isDead)
        {
            BroadcastMessage("没有人会为死掉的人卖命!");
            return false;
        }

        return true;
    }

    // 扣除玩家金币并进行验证
    public static bool DeductGold(DewPlayer player, int cost, string errorMessage)
    {
        if (player == null) return false;

        if (player.gold >= cost)
        {
            player.gold -= cost;
            return true;
        }

        BroadcastMessage(errorMessage);
        return false;
    }

    // 向所有玩家广播消息
    public static void BroadcastMessage(string content)
    {
        NetworkedManagerBase<ChatManager>.instance.BroadcastChatMessage(new ChatManager.Message
        {
            type = ChatManager.MessageType.Raw,
            content = content
        });
    }
    
    // 计算百分比，并保留两位小数
    public static float Percentage(float x)
    {
        // 如果x小于等于-100，则返回0
        if (x <= -100)
        {
            return 0f;
        }
    
        // 计算结果
        float result = (100f / (100f + x)) * 100f;
        // 返回结果，保留两位小数
        return (float)Math.Round(result, 2);
    }

    // 计算反百分比，并保留两位小数
    public static float RePercentage(float x)
    {
        if (x <= -100)
        {
            return 0f;
        }
    
        float result = 100f - (100f / (100f + x)) * 100f;
        return (float)Math.Round(result, 2);
    }
}