﻿using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>好友列表模块，处理好友的添加和删除</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class CategoryModule : AbstractModule
    {
        public IQQActionFuture GetBuddyList(QQActionListener listener = null)
        {
            return PushHttpAction(new GetBuddyListAction(Context, listener));
        }
    }
}
