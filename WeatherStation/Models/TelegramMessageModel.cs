using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherStation.Models
{
    public class TelegramMessageModel
    {
        public long ChatId { get; set; }
        public string MessageText { get; set; }

        public TelegramMessageModel(long chatId, string messageText)
        {
            ChatId = chatId;
            MessageText = messageText;
        }
    }
}
