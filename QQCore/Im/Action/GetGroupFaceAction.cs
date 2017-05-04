﻿using System;
using System.Drawing;
using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>个人签名</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetGroupFaceAction : AbstractHttpAction
    {

        private readonly QQGroup _group;

        public GetGroupFaceAction(IQQContext context, QQActionListener listener,
                QQGroup group)
            : base(context, listener)
        {

            this._group = group;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get,
                    QQConstants.URL_GET_USER_FACE);
            req.AddGetValue("uin", _group.Code);
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());
            req.AddGetValue("cache", "0");
            req.AddGetValue("type", "4");
            req.AddGetValue("fid", "0");
            req.ResultType = ResponseResultType.Stream;
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            try
            {
                var image = Image.FromStream(response.ResponseStream);
                _group.Face = image;
            }
            catch (IOException e)
            {
                throw new QQException(QQErrorCode.IOError, e);
            }
            NotifyActionEvent(QQActionEventType.EvtOK, _group);
        }
    }
}
