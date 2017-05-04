﻿using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>消息发送</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class SendMsgAction : AbstractHttpAction
    {
        private readonly QQMsg _msg;
        private static long _msgId = 81690000;
        public SendMsgAction(IQQContext context, QQActionListener listener, QQMsg msg)
            : base(context, listener)
        {
            _msg = msg;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            // r:{"to":2982077931,"face":0,"content":"[\"123\",[\"face\",1],\"456\",[\"face\",0],\"\",\"\\n【提示：此用户正在使用Q+ Web：http://web.qq.com/】\",[\"font\",{\"name\":\"微软雅黑\",\"size\":\"11\",\"style\":[0,0,0],\"color\":\"ffcc99\"}]]","msg_id":91310001,"clientid":"74131454","psessionid":"8368046764001e636f6e6e7365727665725f77656271714031302e3133332e34312e3230320000230700001f01026e04002aafd23f6d0000000a40484a526f4866467a476d00000028d954c71693cd99ae8c0c64b651519e88f55ce5075140346da7d957f3abefb51d0becc25c425d7cf5"}
            // r:{"group_uin":3408869879,"content":"[\"群消息发送测试\",[\"face\",13],\"\",\"\\n【提示：此用户正在使用Q+ Web：http://web.qq.com/】\",[\"font\",{\"name\":\"微软雅黑\",\"size\":\"11\",\"style\":[0,0,0],\"color\":\"ffcc99\"}]]","msg_id":91310002,"clientid":"74131454","psessionid":"8368046764001e636f6e6e7365727665725f77656271714031302e3133332e34312e3230320000230700001f01026e04002aafd23f6d0000000a40484a526f4866467a476d00000028d954c71693cd99ae8c0c64b651519e88f55ce5075140346da7d957f3abefb51d0becc25c425d7cf5"}
            // clientid、psessionid

            var session = Context.Session;
            var json = new JObject();
            QQHttpRequest req = null;
            if (_msg.Type == QQMsgType.BUDDY_MSG)
            {
                req = CreateHttpRequest("POST", QQConstants.URL_SEND_BUDDY_MSG);
                json.Add("to", _msg.To.Uin);
                json.Add("face", 0); // 这个是干嘛的？？
            }
            else if (_msg.Type == QQMsgType.GROUP_MSG)
            {
                req = CreateHttpRequest("POST", QQConstants.URL_SEND_GROUP_MSG);
                json.Add("group_uin", _msg.Group.Gin);
                json.Add("face", 573);
            }
            else if (_msg.Type == QQMsgType.DISCUZ_MSG)
            {
                req = CreateHttpRequest("POST", QQConstants.URL_SEND_DISCUZ_MSG);
                json.Add("did", _msg.Discuz.Did);
                json.Add("face", 573);
            }
            else if (_msg.Type == QQMsgType.SESSION_MSG)
            {	// 临时会话消息
                req = CreateHttpRequest("POST", QQConstants.URL_SEND_SESSION_MSG);
                var member = (QQStranger)_msg.To;
                json.Add("to", member.Uin);
                json.Add("face", 0); // 这个是干嘛的？？
                json.Add("group_sig", member.GroupSig);
                json.Add("service_type", member.ServiceType);
            }
            else
            {
                Context.Logger.LogWarning("unknown MsgType: " + _msg.Type);
            }

            json.Add("content", _msg.PackContentList());
            json.Add("msg_id", ++_msgId);
            json.Add("clientid", session.ClientId);
            json.Add("psessionid", session.SessionId);

            req.AddPostValue("r", JsonConvert.SerializeObject(json));
            req.AddPostValue("clientid", session.ClientId);
            req.AddPostValue("psessionid", session.SessionId);
            req.AddHeader("Referer", QQConstants.REFFER);

            // System.out.println("sendMsg: " + JsonConvert.SerializeObject(json));
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var str = response.GetResponseString();
            var json = JObject.Parse(str);
            var retcode = json["errCode"].ToString();
            if (retcode == "0")
            {
                var msg = json["msg"].ToString();
                if (msg == "send ok")
                {
                    NotifyActionEvent(QQActionEventType.EvtOK, _msg);
                }
            }
            else
            {
                // NotifyActionEvent(QQActionEventType.EVT_ERROR, ));
                throw new QQException(QQErrorCode.UnexpectedResponse, str);
            }
             
            
        }
    }
}
