using System;

namespace KafkaProducer
{
    public class MessageModel
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
