using System;

namespace NotificationGateway
{
    public class TelegramChannel : NotificationChannel
    {
        private readonly string _botToken;

        public override bool IsOnline => true;

        public TelegramChannel(string name, string botToken) : base(name)
        {
            _botToken = botToken;
        }

        public override bool Deliver(string recipient, string message)
        {
            if (!IsOnline)
            {
                Console.WriteLine($"[{ChannelName}] Бот оффлайн.");
                return false;
            }

            if (string.IsNullOrEmpty(recipient) || !recipient.StartsWith("@"))
            {
                Console.WriteLine($"[{ChannelName}] Получатель должен начинаться с '@': '{recipient}'");
                return false;
            }

            if (message.Length > 100)
            {
                Console.WriteLine($"[{ChannelName}] Сообщение длиннее 100 символов ({message.Length}). Отправка отменена.");
                return false;
            }

            DeliveredCount++;
            Console.WriteLine($"[{ChannelName}] *{recipient}* (символов: {message.Length})");
            Console.WriteLine(FormatPayload($"*{message}*"));
            Console.WriteLine();
            return true;
        }
    }
}