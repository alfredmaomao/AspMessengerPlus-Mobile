using System;
using System.Collections.Generic;
using System.Text;

namespace AspMessengerPlus.Maui.Models;

public class Message
{
    public string Text { get; set; } = "";
    public bool IsMine { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    public string TimeText => Time.ToString("HH:mm");
    public bool IsTyping { get; set; }
}
