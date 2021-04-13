using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Common.ServiceContracts
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Result = new OperationResult();
        }

        public OperationResult Result { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return !Result.Messages.Any(q => q.Type == OperationResult.OperationalMessage.MessageType.Error);
            }
        }

        #region Helper Functions
        private void AddMessage(OperationResult.OperationalMessage.MessageType type, string message)
        {
            Result.Messages.Add(new OperationResult.OperationalMessage()
            {
                Type = type,
                Text = message
            });
        }
        public void AddInfo(string message)
        {
            AddMessage(OperationResult.OperationalMessage.MessageType.Info, message);
        }
        public void AddWarning(string message)
        {
            AddMessage(OperationResult.OperationalMessage.MessageType.Warning, message);
        }
        public void AddError(string message)
        {
            AddMessage(OperationResult.OperationalMessage.MessageType.Error, message);
        }
        #endregion Helper Functions

        #region OperationResult
        public class OperationResult
        {
            internal OperationResult()
            {
                Messages = new List<OperationalMessage>();
            }

            public List<OperationalMessage> Messages { get; set; }

            public class OperationalMessage
            {
                internal OperationalMessage() { }

                public MessageType Type { get; set; }
                public string Text { get; set; }

                public enum MessageType
                {
                    Info = 1,
                    Warning = 2,
                    Error = 3
                }
            }
        }
        #endregion OperationResult
    }
}
