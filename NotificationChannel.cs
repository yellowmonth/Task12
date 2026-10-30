using System;

namespace NotificationGateway
{
    // Абстрактный базовый класс — контракт для всех каналов уведомлений.
    public abstract class NotificationChannel
    {
        protected string ChannelName;
        public int DeliveredCount { get; protected set; }

        // Абстрактное свойство — обязательная реализация у потомков
        public abstract bool IsOnline { get; }

        protected NotificationChannel(string name)
        {
            ChannelName = name;
        }

        // Абстрактный метод отправки
        public abstract bool Deliver(string recipient, string message);

        // Виртуальный метод форматирования — может быть переопределён
        public virtual string FormatPayload(string rawMessage)
        {
            return $"[{ChannelName}] {rawMessage}";
        }

        // Обычный метод отчёта (общий для всех)
        public void PrintAudit()
        {
            Console.WriteLine($"[AUDIT] Канал '{ChannelName}': доставлено {DeliveredCount} оповещ.");
        }
    }
}