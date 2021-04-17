namespace MetricsManager.DBSettings
{
    /// <summary> Ключи имен таблиц </summary>
    public enum Tables
	{
		Agent,
		CpuMetric,
		DotNetMetric,
		HddMetric,
		NetworkMetric,
		RamMetric,
	}

	/// <summary> Ключи имен столбцов </summary>
	public enum Columns
	{
		Id,
		Value,
		Time,
		AgentId,
		Name,
		Status,
		IpAddress
	}
}
