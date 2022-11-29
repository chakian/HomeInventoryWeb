using System.Collections.Generic;
using System.Linq;

namespace WebUI.ApiContracts
{
    public class ApiResponseBase
    {
        public bool IsSuccessful { get { return ApiMessages == null || !ApiMessages.Any(m => m.Type == ApiMessage.MessageType.Error); } }
        public List<ApiMessage> ApiMessages { get; set; }

        public class ApiMessage
        {
            public enum MessageType
            {
                Info,
                Warning,
                Error
            }

            public MessageType Type { get; set; }
            public string Message { get; set; }
        }
    }
}
