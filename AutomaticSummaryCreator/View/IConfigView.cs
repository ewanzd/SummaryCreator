using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator.View
{
    public interface IConfigView
    {
        ConfigPresenter Presenter { get; set; }
    }
}
