/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2017. All rights reserved.
 * author guomw
**/


using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQLogin
{

    /// <summary>
    /// QQ登录成功通知
    /// </summary>
    public delegate void QQNotifyLoginSuccessEventHandler();
    
    /// <summary>
    /// QQ群消息通知
    /// </summary>
    /// <param name="msgContent">消息内容</param>
    /// <param name="urls">消息包含的url</param>
    public delegate void QQNotifyGroupMsgEventHandler(string msgContent, List<string> urls);

    /// <summary>
    /// 关闭QQ
    /// </summary>
    public delegate void CloseQQEventHandler();



    /// <summary>
    /// 全局类
    /// </summary>
    public static class QQGlobal
    {
        /// <summary>
        /// QQ登录对象
        /// </summary>
        public static QQLogin loginForm { get; set; }

        /// <summary>
        /// 群加载完成
        /// </summary>
        public static bool QQGroupLoadSuccess { get; set; }
        /// <summary>
        /// 好友加载完成
        /// </summary>
        public static bool QQBuddyLoadSuccess { get; set; }

        /// <summary>
        /// qq数据
        /// </summary>
        public static QQStore store
        {
            get
            {
                return client.Store;
            }
        }
        /// <summary>
        /// QQ对象
        /// </summary>
        public static WebQQClient client { get; set; }

        /// <summary>
        /// 监控的群数量
        /// </summary>
        public static List<QQGroup> listenGroups { get; set; } = new List<QQGroup>();


        /// <summary>
        /// 获取自己账户实体
        /// </summary>
        public static QQAccount account
        {
            get
            {
                return client.Account;
            }
        }

        /// <summary>
        /// 颜色值 248, 248, 248
        /// </summary>
        public static readonly Color backColorSelected = Color.FromArgb(248, 248, 248);
        /// <summary>
        /// 颜色值 255, 255, 255
        /// </summary>
        public static readonly Color backColor = Color.FromArgb(255, 255, 255);

    }
}
