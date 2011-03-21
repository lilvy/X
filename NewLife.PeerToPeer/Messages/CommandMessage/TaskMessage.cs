﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NewLife.Messaging;

namespace NewLife.PeerToPeer.Messages
{
    /// <summary>
    /// 任务执行状态 任务就是任务，区别于其他信息
    /// </summary>
    public class TaskMessage : CommandMessageBase<TaskMessage>
    {
        #region 属性
        /// <summary>消息类型</summary>
        public override MessageTypes MessageType { get { return MessageTypes.Task; } }

        private Int32 _State;
        /// <summary>执行状态</summary>
        public Int32 State
        {
            get { return _State; }
            set { _State = value; }
        }

        private Int32 _Percent;
        /// <summary>完成进度</summary>
        public Int32 Percent
        {
            get { return _Percent; }
            set { _Percent = value; }
        }

        private String _Remark;
        /// <summary>备注</summary>
        public String Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        #endregion

        #region 响应
        /// <summary>
        /// 响应
        /// </summary>
        public class Response : CommandMessageBase<Response>
        {
            /// <summary>消息类型</summary>
            public override MessageTypes MessageType { get { return MessageTypes.TaskResponse; } }

            private Int32 _State;
            /// <summary>执行状态</summary>
            public Int32 State
            {
                get { return _State; }
                set { _State = value; }
            }

            private byte[] _TaskMessage;
            /// <summary>任务信息</summary>
            public byte[] TaskMessage
            {
                get { return _TaskMessage; }
                set { _TaskMessage = value; }
            }
        }
        #endregion
    }
}