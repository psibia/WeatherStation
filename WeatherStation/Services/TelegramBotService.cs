using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace WeatherStation.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient botClient;
        private readonly long allowedChatId;

        public TelegramBotService(string token, long allowedChatId)
        {
            this.allowedChatId = allowedChatId;
            botClient = new TelegramBotClient(token);
        }

        public void Start()
        {
            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            if (chatId != allowedChatId)
                return;

            OnMessageReceived?.Invoke(this, new MessageReceivedEventArgs(update.Message.Text));
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка телеграм бота: {exception.Message}");
            return Task.CompletedTask;
        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public class MessageReceivedEventArgs : EventArgs
        {
            public string Message { get; }

            public MessageReceivedEventArgs(string message)
            {
                Message = message;
            }
        }

        public async Task SendMessageAsync(string message)
        {
            await botClient.SendTextMessageAsync(allowedChatId, message);
        }
    }
}
