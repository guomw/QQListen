﻿using System;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取QQ账号</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetFriendAccoutAction : AbstractHttpAction
    {

        private QQUser buddy;

        public GetFriendAccoutAction(IQQContext context, QQActionListener listener, QQUser buddy)
            : base(context, listener)
        {

            this.buddy = buddy;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            // tuin=4245757755&verifysession=&type=1&code=&vfwebqq=**&t=1361631644492
            var req = CreateHttpRequest(HttpConstants.Get,
                    QQConstants.URL_GET_USER_ACCOUNT);
            req.AddGetValue("tuin", buddy.Uin);
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());
            req.AddGetValue("verifysession", ""); // 验证码？？
            req.AddGetValue("type", 1);
            req.AddGetValue("code", "");

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                var obj = json["result"].ToObject<JObject>();
                buddy.QQ = obj["account"].ToObject<long>();
            }

            NotifyActionEvent(QQActionEventType.EvtOK, buddy);
        }

    }

}
