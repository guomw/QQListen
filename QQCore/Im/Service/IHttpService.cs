﻿using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Service
{
    public enum ProxyType
    {
        HTTP,
        SOCK4,
        SOCK5
    }

    public interface IHttpService : IQQService
    {
        /// <summary>
        /// 设置HTTP代理
        /// </summary>
        /// <param name="proxyType">代理类型</param>
        /// <param name="proxyHost">代理主机</param>
        /// <param name="proxyPort">代理端口</param>
        /// <param name="proxyAuthUser">认证用户名， 如果不需要认证，设置为null</param>
        /// <param name="proxyAuthPassword">认证密码，如果不需要认证，设置为null</param>
        void SetHttpProxy(ProxyType proxyType, string proxyHost,
                int proxyPort, string proxyAuthUser, string proxyAuthPassword);

        string UserAgent { get; set; }

        /// <summary>
        /// 创建一个请求，这个方法会填充默认的HTTP头，比如User-Agent
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        QQHttpRequest CreateHttpRequest(string method, string url);

        /// <summary>
        /// 执行一个HTTP请求
        /// </summary>
        /// <param name="qqRequest"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQHttpResponse ExecuteHttpRequest(QQHttpRequest qqRequest, IQQHttpListener listener);
        Task<QQHttpResponse> ExecuteHttpRequestAsync(QQHttpRequest request, IQQHttpListener listener, CancellationToken token);
        
        /// <summary>
        /// 获取一个cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        QQHttpCookie GetCookie(string name, string url);
    }

}
