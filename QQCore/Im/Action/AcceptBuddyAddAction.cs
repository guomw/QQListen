﻿using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// 接受别人的加好友请求
    /// </summary>
    public class AcceptBuddyAddAction : AbstractHttpAction
    {
        private readonly string _account;

        public AcceptBuddyAddAction(IQQContext context, QQActionListener listener, string account)
            : base(context, listener)
        {
            this._account = account;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest("POST", QQConstants.URL_ACCEPET_BUDDY_ADD);
            var json = new JObject
            {
                {"account", _account},
                {"gid", "0"},
                {"mname", ""},
                {"vfwebqq", session.Vfwebqq}
            };
            req.AddPostValue("r", json.ToString());
            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var str = response.GetResponseString();
            var json = JObject.Parse(str);
            if (json["retcode"].ToString() == "0")
            {
                NotifyActionEvent(QQActionEventType.EvtOK, json);
            }
            else
            {
                throw new QQException(QQErrorCode.UnexpectedResponse, str);
            }
        }

    }
}
