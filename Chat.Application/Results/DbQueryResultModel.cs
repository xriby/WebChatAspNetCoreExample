using System.Text.Json.Serialization;

namespace Chat.Application.Results
{
    /// <summary>
    /// Результат операции запроса к БД.
    /// </summary>
    public class DbQueryResultModel<T>
    {
        /// <summary>
        /// Статус операции.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EDbQueryStatus Status { get; set; }

        /// <summary>
        /// Текст ошибки, в случае возникновения ошибки.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Данные запроса к БД.
        /// </summary>
        public T Data { get; set; }
    }
}
