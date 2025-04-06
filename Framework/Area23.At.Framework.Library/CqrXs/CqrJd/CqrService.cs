﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Area23.At.Framework.Library.CqrXs.CqrJd
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "CqrServiceSoap",
          Namespace = "https://cqrjd.eu/cqrsrv/cqrjd/")]
    public partial class CqrService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        /// <remarks/>
        public CqrService()
        {
            this.Url = LibPaths.CqrServiceSoap;
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/Send1StSrvMsg",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Send1StSrvMsg(string cryptMsg)
        {
            object[] results = this.Invoke("Send1StSrvMsg", new object[] {
                        cryptMsg});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSend1StSrvMsg(string cryptMsg, System.AsyncCallback
              callback, object asyncState)
        {
            return this.BeginInvoke("Send1StSrvMsg", new object[] {
                        cryptMsg}, callback, asyncState);
        }

        /// <remarks/>
        public string EndSend1StSrvMsg(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/ChatRoomInvite",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ChatRoomInvite(string cryptMsg)
        {
            object[] results = this.Invoke("ChatRoomInvite", new object[] {
                        cryptMsg});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginChatRoomInvite(string cryptMsg, System.AsyncCallback
              callback, object asyncState)
        {
            return this.BeginInvoke("ChatRoomInvite", new object[] {
                        cryptMsg}, callback, asyncState);
        }

        /// <remarks/>
        public string EndChatRoomInvite(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/ChatRoomPoll",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ChatRoomPoll(string cryptMsg)
        {
            object[] results = this.Invoke("ChatRoomPoll", new object[] {
                        cryptMsg});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginChatRoomPoll(string cryptMsg, System.AsyncCallback callback,
              object asyncState)
        {
            return this.BeginInvoke("ChatRoomPoll", new object[] {
                        cryptMsg}, callback, asyncState);
        }

        /// <remarks/>
        public string EndChatRoomPoll(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/ChatRoomPushMessage",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ChatRoomPushMessage(string cryptMsg, string chatRoomMembersCrypted)
        {
            object[] results = this.Invoke("ChatRoomPushMessage", new object[] {
                        cryptMsg,
                        chatRoomMembersCrypted});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginChatRoomPushMessage(string cryptMsg, string
              chatRoomMembersCrypted, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ChatRoomPushMessage", new object[] {
                        cryptMsg,
                        chatRoomMembersCrypted}, callback, asyncState);
        }

        /// <remarks/>
        public string EndChatRoomPushMessage(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/ChatRoomClose",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ChatRoomClose(string cryptMsg)
        {
            object[] results = this.Invoke("ChatRoomClose", new object[] {
                        cryptMsg});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginChatRoomClose(string cryptMsg, System.AsyncCallback
              callback, object asyncState)
        {
            return this.BeginInvoke("ChatRoomClose", new object[] {
                        cryptMsg}, callback, asyncState);
        }

        /// <remarks/>
        public string EndChatRoomClose(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/TestService",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string TestService()
        {
            object[] results = this.Invoke("TestService", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginTestService(System.AsyncCallback callback, object
              asyncState)
        {
            return this.BeginInvoke("TestService", new object[0], callback, asyncState);
        }

        /// <remarks/>
        public string EndTestService(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/GetIPAddress",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetIPAddress()
        {
            object[] results = this.Invoke("GetIPAddress", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetIPAddress(System.AsyncCallback callback, object
              asyncState)
        {
            return this.BeginInvoke("GetIPAddress", new object[0], callback, asyncState);
        }

        /// <remarks/>
        public string EndGetIPAddress(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
             "https://cqrjd.eu/cqrsrv/cqrjd/TestCache",
              RequestNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              ResponseNamespace = "https://cqrjd.eu/cqrsrv/cqrjd/",
              Use = System.Web.Services.Description.SoapBindingUse.Literal,
              ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string TestCache()
        {
            object[] results = this.Invoke("TestCache", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginTestCache(System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("TestCache", new object[0], callback, asyncState);
        }

        /// <remarks/>
        public string EndTestCache(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        }

}