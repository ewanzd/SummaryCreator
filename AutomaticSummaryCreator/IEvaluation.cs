using AutomaticSummaryCreator.Excel;

namespace AutomaticSummaryCreator
{
    public interface IEvaluation
    {
        void LoadData(string path);

        void SaveData(SheetDataInsert insert, string targetRow);
    }
}
