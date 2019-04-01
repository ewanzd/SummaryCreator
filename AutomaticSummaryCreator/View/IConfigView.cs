namespace AutomaticSummaryCreator.View
{
    public interface IConfigView
    {
        ConfigPresenter Presenter { get; set; }

        string ExcelPath { get; set; }
        string MeteoPath { get; set; }
        string SensorDirectoryPath { get; set; }
        string TableName { get; set; }
        int IdRow { get; set; }

        string Status { set; }
        double RemainingTime { get; set; }
        bool TimerIsEnabled { get; set; }
        bool ActionButtonEnabled { get; set; }
        string ActionButtonText { get; set; }
    }
}
