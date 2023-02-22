using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GanShin.UI
{
    public class UIHpBar : UIRootBase
    {
        protected override INotifyPropertyChanged InitializeDataContext()
            => new UIHpBarContext();

        private UIHpBarContext _context;
        
        public UIHpBarContext Context => _context ?? DataContext as UIHpBarContext;
    }
}
