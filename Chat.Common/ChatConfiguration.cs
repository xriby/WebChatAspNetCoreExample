namespace Chat.Common
{
    /// <summary>
    /// Настройки чата.
    /// </summary>
    public static class ChatConfiguration
    {
        /// <summary>
        /// Минимимальная длина сообщения.
        /// </summary>
        public static int MinTextLength => 2;

        /// <summary>
        /// Максимальная длина сообщения.
        /// </summary>
        public static int MaxTextLength => 512;
    }
}
