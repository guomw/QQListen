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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQLogin
{

    /// <summary>
    /// QQ登录成功通知
    /// </summary>
    public delegate void QQNotifyLoginSuccessEventHandler(IQQClient client);

    /// <summary>
    /// 群消息
    /// </summary>
    public delegate void QQNotifyGroupMsgEventHandler(IQQClient client,QQMsg msg);




    public static class QQGlobal
    {
        /// <summary>
        /// 群加载完成
        /// </summary>
        public static bool QQGroupLoadSuccess { get; set; }
        /// <summary>
        /// 好友加载完成
        /// </summary>
        public static bool QQBuddyLoadSuccess { get; set; }


        public static IQQClient client { get; set; }
    }
}
