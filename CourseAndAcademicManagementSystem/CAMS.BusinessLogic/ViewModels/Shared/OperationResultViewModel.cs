using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.Shared
{
    public class OperationResultViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public static OperationResultViewModel Ok(string? message = null)
            => new OperationResultViewModel { Success = true, Message = message };

        public static OperationResultViewModel Fail(string message)
            => new OperationResultViewModel { Success = false, Message = message };
    }
}
