﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HomeInv.Common.Models;

public class MailRequest
{
    public string ToEmail { get; set; }
    public List<string> ToEmailList { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<IFormFile> Attachments { get; set; }
}
