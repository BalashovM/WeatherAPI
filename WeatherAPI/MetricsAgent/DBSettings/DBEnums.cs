namespace MetricsAgent.DBSettings
{
    /// <summary> Ключи имен таблиц </summary>
    public enum Tables
	{
		CpuMetric,
		DotNetMetric,
		HddMetric,
		NetworkMetric,
		RamMetric,
	}

	/// <summary> Ключи имен рядов </summary>
	public enum Columns
	{
		Id,
		Value,
		Time,
	}
}
