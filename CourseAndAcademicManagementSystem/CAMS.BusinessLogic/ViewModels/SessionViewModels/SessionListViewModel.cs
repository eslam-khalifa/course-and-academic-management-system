using CAMS.BusinessLogic.ViewModels.GradeViewModels;
using CAMS.BusinessLogic.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.SessionViewModels
{
    public class SessionListViewModel
    {
        public PagedResultViewModel<SessionViewModel> PagedSessions { get; set; } = new PagedResultViewModel<SessionViewModel>();

        public string? SearchTerm { get; set; }
    }
}
