using AutomaticSummaryCreator.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator
{
    public abstract class Evaluation
    {
        public abstract void LoadData(string path);

        public abstract void SaveData(SheetDataInsert insert, string targetRow);
    }
}
