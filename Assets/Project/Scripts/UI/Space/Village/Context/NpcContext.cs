#nullable enable

using GanShin.Village.Base;
using JetBrains.Annotations;

namespace GanShin.UI.Village
{
    public class NpcContext : GanContext
    {
        public NpcContext(UINpcHUD owner)
        {
            _owner = owner;
        }

        public NpcContext(UINpcHUD owner, NpcInfo npcInfo)
        {
            _owner   = owner;
            _npcInfo = npcInfo;

            NpcName  = npcInfo.npcName;
            NpcTitle = npcInfo.npcTitle;

            CurrentDialogue = string.Empty;
        }

#region Fields

        private UINpcHUD _owner;
        private NpcInfo? _npcInfo;

        private bool _isShowMenu;
        private bool _isDialogue;

        private string _npcName         = string.Empty;
        private string _npcTitle        = string.Empty;
        private string _currentDialogue = string.Empty;

#endregion Fields

#region Properties

        [UsedImplicitly]
        public bool IsShowMenu
        {
            get => _isShowMenu;
            set
            {
                _isShowMenu = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public bool IsDialogue
        {
            get => _isDialogue;
            set
            {
                if (value) IsShowMenu = false;

                _isDialogue = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public string NpcName
        {
            get => _npcName;
            set
            {
                _npcName = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public string NpcTitle
        {
            get => _npcTitle;
            set
            {
                _npcTitle = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public string CurrentDialogue
        {
            get => _currentDialogue;
            set
            {
                _currentDialogue = value;
                OnPropertyChanged();
            }
        }

#endregion Properties

#region Event

        public void OnStartDialogue()
        {
        }

        public void OnEndDialogue()
        {
        }

#endregion Event
    }
}