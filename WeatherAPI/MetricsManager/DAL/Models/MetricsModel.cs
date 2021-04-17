using System.Collections.Generic;

namespace MetricsManager.DAL.Models
{
    /// <summary>
    /// Контейнер для передачи списка с метриками
    /// </summary>
    public class MetricsModel<T>
	{
		public List<T> Metrics { get; set; }

		public MetricsModel()
		{
			Metrics = new List<T>();
		}
	}
}
