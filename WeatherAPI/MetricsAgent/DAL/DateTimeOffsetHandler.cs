using Dapper;
using System;
using System.Data;

namespace MetricsAgent.DAL
{
    /// <summary>
	/// Хэндлер для парсинга значений в DateTimeOffset в классах моделей
	/// </summary>
    public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value)
            => DateTimeOffset.FromUnixTimeSeconds((long)value);

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
            => parameter.Value = value.ToUnixTimeSeconds();
    }
}