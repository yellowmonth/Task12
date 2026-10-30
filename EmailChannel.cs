using System;

namespace NotificationGateway
{
    public class EmailChannel : NotificationChannel
    {
        private readonly bool _smtpAvailable;

        public override bool IsOnline => _smtpAvailable;

        public EmailChannel(string name, bool smtpAvailable = true) : base(name)
        {
            _smtpAvailable = smtpAvailable;
        }

        public override string FormatPayload(string rawMessage)
        {
            // Вызов базового форматирования + почтовый колонтитул
            return base.FormatPayload(rawMessage) + "\n---\nС уважением, Почтовый Сервис";
        }

        public override bool Deliver(string recipient, string message)
        {
            if (!IsOnline)
            {
                Console.WriteLine($"[{ChannelName}] SMTP-сервер недоступен.");
                return false;
            }

            if (string.IsNullOrEmpty(recipient) || !recipient.Contains('@'))
            {
                Console.WriteLine($"[{ChannelName}] Некорректный адрес: '{recipient}'. Отправка отменена.");
                return false;
            }

            DeliveredCount++;
            Console.WriteLine($"[{ChannelName}] -> {recipient}");
            Console.WriteLine(FormatPayload(message));
            Console.WriteLine();
            return true;
        }
    }
}