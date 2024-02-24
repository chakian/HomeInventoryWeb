using HomeInv.Language;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HomeInv.Common.ServiceContracts;

public class BaseResponse
{
    public OperationResult Result { get; set; } = new();

    public bool IsSuccessful
    {
        get
        {
            return Result.Messages.All(q => q.Type != OperationResult.OperationalMessage.MessageTypeEnum.Error);
        }
    }

    #region Helper Functions
    private void AddMessage(OperationResult.OperationalMessage.MessageTypeEnum type, string message)
    {
        Result.Messages.Add(new OperationResult.OperationalMessage()
        {
            Type = type,
            Text = message
        });
    }
    public void AddInfo(string message)
    {
        AddMessage(OperationResult.OperationalMessage.MessageTypeEnum.Info, message);
    }
    public void AddWarning(string message)
    {
        AddMessage(OperationResult.OperationalMessage.MessageTypeEnum.Warning, message);
    }
    public void AddError(string message)
    {
        AddMessage(OperationResult.OperationalMessage.MessageTypeEnum.Error, message);
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

            [JsonIgnore]
            public MessageTypeEnum Type { get; set; }

            public string MessageType => Type.ToString();

            public string Text { get; set; }

            public enum MessageTypeEnum
            {
                Info = 1,
                Warning = 2,
                Error = 3,
            }

            public override string ToString()
            {
                return Text;
            }
        }

        public override string ToString()
        {
            return Messages.Count == 0 ? Resources.Success_Generic : string.Join(". ", Messages);
        }
    }
    #endregion OperationResult
}