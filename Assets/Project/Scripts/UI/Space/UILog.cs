using System.Collections;
using System.Collections.Generic;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.Space.UI
{
    public class UILog : UIRootBase
    {
        protected override Context InitializeDataContext()
        {
            // TODO: ContextManager에서 Get
            return new LogContext();
        }
    }
}
